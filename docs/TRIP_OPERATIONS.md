# Lịch chạy định kỳ và dữ liệu demo

Hệ thống chỉ đặt ghế cho toàn chuyến, giá theo tuyến. Không có bước chọn điểm đón/trả,
API quản lý điểm hay bộ điểm mẫu. Luồng khách hàng và bán vé tại quầy dùng cùng nghiệp vụ.

## Lịch chạy

Toàn bộ `/api/trip-schedules/**` chỉ dành cho Admin:

- GET danh sách; POST tạo; PUT `/{id}` sửa.
- DELETE `/{id}` tắt lịch, giữ các chuyến đã tạo.
- POST `/{id}/preview` và POST `/{id}/generate` nhận
  `{from:"YYYY-MM-DD",to:"YYYY-MM-DD"}`, tối đa 30 ngày tính cả hai đầu.

Body tạo/sửa:

```json
{
  "name": "Phương Trang | TP HCM → Vũng Tàu | 08:00",
  "vehicleId": 1,
  "routeId": 1,
  "driverId": 1,
  "departureTime": "08:00:00",
  "weekdays": [1, 2, 3, 4, 5, 6, 7],
  "validFrom": "2027-01-01",
  "validTo": "2027-01-31",
  "isActive": true,
  "excludedDates": ["2027-01-02"]
}
```

ID chỉ minh họa; chọn bản ghi thực tế trong quản trị. Không cần `templateId`.
Thứ dùng ISO: 1=thứ Hai, 7=Chủ nhật. Ngày nghỉ phải trong khoảng hiệu lực.
GET trả `weekdays` và `excludedDates` dạng chuỗi phân cách dấu phẩy,
thêm `scheduleId,lastRunAt,lastResult`; `lastResult` là chuỗi JSON kết quả lần sinh gần nhất.

Xem trước/sinh trả `[{serviceDate,status,tripId,message}]`:

- READY: dự kiến hợp lệ, chỉ trong xem trước; kiểm tra lại khi tạo.
- CREATED: vừa tạo.
- EXISTS: ngày đã có chuyến sinh từ lịch.
- EXCLUDED: ngày nghỉ; khi chạy sinh được lưu dấu và không tự khôi phục kể cả sửa lịch.
- DELETED: chuyến sinh từ lịch đã bị xóa; không tự sinh lại.
- SKIPPED: lịch tắt, ngoài hiệu lực hoặc không thuộc thứ chạy.
- PAST: giờ đã qua theo Asia/Ho_Chi_Minh.
- ERROR: xung đột hoặc lỗi cấu hình/lưu dữ liệu; có thể thử lại.

Mỗi ngày chạy trong transaction riêng. Khóa lịch và unique(schedule_id,service_date) chống sinh trùng.
Khóa xe rồi tài xế bảo vệ việc tạo/sửa chuyến thủ công lẫn tự động.
Kiểm tra chồng khoảng chạy; hai chuyến được nối tiếp đúng giờ đến.
Nhà xe, loại xe, xe, tuyến và tài xế phải hoạt động, giá phải lớn hơn 0.
Chưa tính vị trí xe hoặc thời gian quay đầu; thời lượng tuyến hiện dùng TIME, dưới 24 giờ.

Xóa chuyến giữ dấu DELETED. Muốn thay ngày đã loại trừ/xóa thì tạo chuyến tăng cường thủ công.
Tự bổ sung 14 ngày khi ứng dụng sẵn sàng và 00:05 giờ Việt Nam hằng ngày.
Sửa/tắt lịch không cập nhật hoặc xóa chuyến đã sinh.

`GET /api/trip-conflicts` cho Admin/NhanVien trả
`[{firstTripId,secondTripId,reason}]` để rà soát chồng lịch và thiếu giờ đến.
API chỉ báo cáo, không tự sửa.

## Nâng cấp khi bỏ điểm đón/trả

Giữ nguyên V7/V8 đã áp dụng. V9 bỏ yêu cầu NOT NULL của tham chiếu bộ điểm trên lịch;
ứng dụng không đọc/ghi tham chiếu này nữa. Các bảng điểm và cột bản chụp trên vé cũ
được giữ ở database để không xóa lịch sử, không còn endpoint hoặc giao diện sử dụng.
Không sửa checksum Flyway, không reset dữ liệu khi nâng cấp.

Deploy backend và frontend cùng phiên bản; mở lại trang chọn ghế sau khi nâng cấp.
Yêu cầu đặt vé chỉ cần dữ liệu trước khi thêm điểm (tripId, seatNumbers, thông tin khách,
paymentMethod và holdToken với khách hàng). Thông tin vé trả về không còn trường điểm.
Các vé, hóa đơn, chuyến, lịch và hồ sơ hiện có vẫn được giữ nguyên.

## Làm mới dữ liệu demo — tùy chọn, có xóa dữ liệu

Không cần chạy script này để bỏ điểm. Chỉ chạy trên môi trường demo local khi
chấp nhận xóa dữ liệu nghiệp vụ cũ:

```powershell
# Chỉ xem phạm vi, không thay đổi dữ liệu
.\scripts\reset-demo.ps1
# Sao lưu, dừng backend, reset và chạy backend trở lại
.\scripts\reset-demo.ps1 -Apply
```

Chỉ nhắm container `coachbooking-db`, database/user `coachbooking`, Compose project
`coachbooking`; yêu cầu Flyway V9. Bản sao lưu custom-format ở
`backend/build/demo-backups/`, Git bỏ qua. Không commit bản sao lưu.

Giữ tài khoản, vai trò, hồ sơ khách hàng/nhân viên và dữ liệu xác thực.
Làm mới danh mục vận tải, giá vé, chuyến và lịch; xóa vé, hóa đơn, thanh toán,
lượt giữ ghế và dữ liệu điểm cũ. Không xóa volume hoặc lịch sử Flyway.
ID tiếp tục tăng để liên kết cũ không trỏ sang dữ liệu mới.

Bộ mới có 2 nhà xe, 3 tuyến hai chiều, 6 xe, 12 tài xế và 12 lịch.
V10 đổi nhãn cũ sang tên tự nhiên: tên lịch gồm nhà xe, tuyến và giờ; tài xế có họ tên
tiếng Việt, biển số dạng 51B-310.01. ID, vé và lịch chạy hiện có không đổi.
Mỗi xe chạy một cặp đi/về; mỗi chiều có tài xế riêng.
Chuyến được tạo sẵn trong cửa sổ 14 ngày, chỉ lấy giờ chưa qua.
Lịch có hiệu lực 180 ngày. Không tạo dữ liệu điểm đón/trả.
Tên, phương tiện và giá là dữ liệu mô phỏng, không phải thông tin khai thác thật.

Reset nằm trong một transaction, kiểm tra chồng lịch và cấu hình trước commit;
lỗi sẽ rollback. Backend được khởi động lại cả khi reset thất bại.
Tải lại trang và bỏ phiên checkout cũ sau reset. Nếu hôm nay hết giờ chạy, chọn ngày mai.
Backend chỉ khởi tạo tài khoản demo còn thiếu, không tự sinh vé/hóa đơn/thanh toán mẫu.

## Kiểm thử PostgreSQL độc lập

Không trỏ test vào database ứng dụng. Test tích hợp chỉ bật với URL kết thúc bằng
`/coachbooking_feature_test`. Container không gắn volume ứng dụng.

```powershell
docker run -d --name coachbooking-trip-feature-test -p 127.0.0.1:55439:5432 -e POSTGRES_DB=coachbooking_feature_test -e POSTGRES_USER=feature_test -e POSTGRES_PASSWORD=feature_test_local_only -e TZ=Asia/Ho_Chi_Minh postgres:16-alpine
cd backend
$env:TRIP_TEST_DB_URL='jdbc:postgresql://localhost:55439/coachbooking_feature_test'
.\gradlew.bat --no-daemon test
Remove-Item Env:TRIP_TEST_DB_URL
```

Nếu container đã có từ lần kiểm thử trước, dùng `docker start coachbooking-trip-feature-test`.
Lần đầu dùng database rỗng; các lần sau tạo fixture riêng biệt.
Nếu không có biến môi trường, PostgreSQL test skip; unit/H2 test vẫn chạy.
