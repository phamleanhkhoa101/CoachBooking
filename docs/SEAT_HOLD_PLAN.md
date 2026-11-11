# Kế hoạch cải thiện đặt vé bằng cơ chế giữ ghế 10 phút

## Tóm tắt

- Khi khách đã đăng nhập và bấm tiếp tục sau khi chọn ghế, backend giữ toàn bộ ghế trong 10 phút.
- Ghế đang giữ được cập nhật gần thời gian thực bằng SSE; người khác không thể chọn hoặc mua ghế đó.
- Khi xác nhận, hold được chuyển thành vé/hóa đơn trong cùng transaction. Hết hạn thì tự giải phóng.
- Cổng khách hàng chỉ giữ phương thức `Chuyển khoản`; tạm thời vẫn tự đánh dấu đã chuyển. Bỏ `Tiền mặt` và `Thanh toán sau`. Luồng nhân viên tại quầy không tạo hold nhưng phải tôn trọng hold của khách.

## Thay đổi chính

### Backend và cơ sở dữ liệu

- Thêm Flyway migration mới tạo bảng `seat_holds` gồm token UUID, tài khoản, chuyến, số ghế, thời điểm tạo/hết hạn; unique theo `(trip_id, seat_number)`.
- Khóa pessimistic bản ghi chuyến xe khi tạo hold hoặc chốt booking, sau đó kiểm tra cả `tickets` và hold còn hiệu lực. Unique constraint hiện có trên `tickets` tiếp tục là lớp bảo vệ cuối.
- Tạo hold theo nguyên tắc all-or-nothing: nếu một ghế không còn trống thì không giữ ghế nào. Hold chỉ thuộc tài khoản tạo ra và không được gia hạn quá 10 phút.
- Scheduler dọn hold hết hạn mỗi phút; mọi thao tác tạo/chốt cũng dọn hold hết hạn của chuyến trước khi kiểm tra.
- Phát sự kiện SSE sau khi transaction commit khi ghế được giữ, hết hạn, giải phóng hoặc bán; endpoint SSE gửi snapshot ban đầu và tự reconnect.
- Nhân viên đặt trực tiếp không cần token hold, nhưng booking bị từ chối nếu ghế đang được khách giữ. Không cho phép ghi đè hold.

### API và hợp đồng

- Thêm `POST /api/seat-holds` với `{ tripId, seatNumbers }`, trả `{ holdToken, tripId, seatNumbers, expiresAt }`.
- Thêm `GET /api/seat-holds/{holdToken}` để khôi phục checkout của chính chủ.
- Thêm `DELETE /api/seat-holds/{holdToken}` để chủ động nhả ghế.
- Thêm `GET /api/trips/{tripId}/seats/events` dạng SSE, phát snapshot `{ tripId, bookedSeats, heldSeats, occurredAt }`.
- Mở rộng seat map với `heldSeats`.
- Thêm `holdToken` vào `BookingRequest`: bắt buộc với khách hàng, không dùng với Admin/Nhân viên.
- Khi booking khách hàng, kiểm tra token đúng tài khoản, chuyến, tập ghế và chưa hết hạn; chuyển hold thành vé/hóa đơn atomically.
- Chuẩn hóa lỗi `409` riêng cho ghế đang giữ, hold hết hạn và ghế vừa được bán; cập nhật `docs/API.md`.
- SSE chỉ công khai trạng thái số ghế, không phát tài khoản hay dữ liệu khách hàng.

### Frontend

- Bắt buộc đăng nhập trước khi vào bước chọn/giữ ghế.
- Khi bấm tiếp tục, gọi API tạo hold rồi mới vào checkout; lưu token và `expiresAt` trong `sessionStorage` để chịu được refresh.
- Bộ đếm lấy thời gian hết hạn từ server, không dùng bộ đếm 15 phút giả hiện tại. Hết hạn thì khóa nút xác nhận, thông báo rõ và quay về chọn ghế.
- Kết nối SSE tại sơ đồ ghế; phân biệt ghế trống, đang được người khác giữ và đã bán. Hold của chính người dùng vẫn được chọn.
- Khi quay lại hoặc hủy checkout, gọi API nhả hold; đóng tab không được coi là bảo đảm nhả ghế, backend vẫn tự hết hạn.
- Tất cả luồng khách hàng hiện có phải dùng hold trước khi gọi `/bookings`.
- Cổng khách hàng chỉ hiển thị `Chuyển khoản`, ghi rõ đây là xác nhận chuyển khoản tạm thời chưa có đối soát tự động; bỏ tuyên bố “xác nhận trong 30 giây”, `Tiền mặt`, `Thanh toán sau` và nội dung giữ vé 24 giờ.
- Luồng bán vé nhân viên giữ các phương thức hiện tại.

## Kiểm thử

- Hai khách đồng thời giữ cùng ghế: đúng một hold thành công, request còn lại nhận `409`.
- Giữ nhiều ghế có một ghế bận: rollback toàn bộ hold.
- Hold hết hạn: SSE phát ghế trống, khách khác giữ được, token cũ không booking được.
- Chốt ngay sát thời điểm hết hạn: chỉ một trong booking hoặc expiry thắng; không bán trùng và không sinh hóa đơn rác.
- Khách dùng token của tài khoản khác, sửa chuyến hoặc sửa danh sách ghế: bị từ chối.
- Nhân viên đặt ghế đang được khách giữ: nhận `409`; sau khi hold hết hạn thì đặt được.
- Booking thành công: hold biến mất, một hóa đơn/một bộ vé được tạo và trạng thái SSE chuyển sang đã bán.
- Transaction lỗi ở bước hóa đơn, vé hoặc thanh toán: rollback toàn bộ và không làm mất hold còn hiệu lực.
- SSE reconnect nhận snapshot đúng; nhiều tab không hiển thị trạng thái cũ.
- Frontend: refresh checkout khôi phục hold; hết giờ vô hiệu hóa xác nhận; cổng khách chỉ còn chuyển khoản.
- Chạy backend tests/build, frontend build và kiểm thử thủ công bằng hai phiên trình duyệt.

## Giả định đã chốt

- Thời gian giữ ghế cố định 10 phút, tính theo đồng hồ backend.
- Khách phải đăng nhập trước khi chọn ghế.
- Dùng SSE thay vì WebSocket.
- Triển khai hiện tại có một backend instance; nếu scale nhiều instance sau này, cần Redis/pub-sub cho SSE và cơ chế cleanup phân tán.
- Chuyển khoản vẫn tự đánh dấu đã chuyển trong giai đoạn mô phỏng; tích hợp cổng thanh toán và callback idempotent là hạng mục riêng sau này.
