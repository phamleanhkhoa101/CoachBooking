# Hợp đồng API — CoachBooking (Spring Boot + React)

Base URL: `/api`. Mọi endpoint (trừ nhóm `auth` công khai) yêu cầu header
`Authorization: Bearer <token>`.

Lịch chạy định kỳ và hướng dẫn nâng cấp:
[TRIP_OPERATIONS.md](TRIP_OPERATIONS.md).

## Tài liệu tương tác (Swagger / OpenAPI 3)

- **Swagger UI:** `http://localhost:8080/swagger-ui.html`
- **OpenAPI JSON:** `http://localhost:8080/v3/api-docs`
- Bật/tắt qua biến môi trường `APP_SWAGGER_ENABLED` (mặc định `true`).
- Để gọi các endpoint yêu cầu xác thực trên Swagger UI: đăng nhập qua nhóm **Xác thực**, đặt header `X-Auth-Request: 1`, lấy `token`, bấm **Authorize** và dán token vào `bearerAuth`. Origin của Swagger (ví dụ `http://localhost:8080`) phải nằm trong `APP_CORS_ALLOWED_ORIGINS`.

Lỗi trả về thống nhất:

```json
{ "message": "Nội dung lỗi hiển thị được cho người dùng" }
```

Mã trạng thái: `400` dữ liệu không hợp lệ, `401` chưa đăng nhập, `403` không đủ quyền,
`404` không tìm thấy, `409` xung đột nghiệp vụ.

## Vai trò

| Mã quyền | Tên quyền | Quyền hạn |
|---|---|---|
| 1 | `Admin` | Toàn quyền |
| 2 | `NhanVien` | Bán vé, hóa đơn, khách hàng, xem danh mục, thống kê |
| 3 | `KhachHang` | Tự đặt vé, xem lịch sử, thanh toán/hủy vé chưa thanh toán, hồ sơ cá nhân |

## 1. Xác thực

Access token có hạn tối đa **15 phút** và chỉ được frontend lưu trong bộ nhớ. Backend
kiểm tra chữ ký, thời hạn, `accountId`, `sid`, trạng thái phiên và trạng thái/quyền tài khoản
ở mỗi request. JWT cũ không có `sid` không còn được chấp nhận.

- Khách hàng: hết phiên sau **7 ngày không refresh**, tối đa **30 ngày từ lúc đăng nhập**.
- Admin/nhân viên: hết phiên cố định sau **12 giờ**.
- Login hoặc refresh thành công cập nhật mốc hoạt động; refresh không kéo dài hạn tuyệt đối.
- `rememberMe=true`: cookie tồn tại sau khi đóng trình duyệt đến hạn còn lại của phiên.
  `false` hoặc thiếu trường: cookie phiên trình duyệt, vẫn chịu giới hạn thời gian của backend.
  Trình duyệt có thể khôi phục cookie phiên khi bật tính năng khôi phục cửa sổ.
- Không refresh định kỳ; request nền ở tab ẩn không tự refresh.

Ba API login, `/auth/refresh` và `/auth/logout` yêu cầu **POST**, header
`X-Auth-Request: 1` và `Origin` khớp danh sách cho phép; thiếu/sai trả `403`.
Trình duyệt tự gửi Origin; client ngoài trình duyệt phải cung cấp cả hai header.
Phản hồi có `Cache-Control: no-store`.

### `POST /api/auth/user/login` (công khai)

Đăng nhập dành cho cổng Khách hàng (`KhachHang`). Nếu tài khoản có quyền `Admin` hoặc `NhanVien`, hệ thống trả về HTTP 403 yêu cầu đăng nhập tại cổng Quản trị.

```json
{ "username": "kh1", "password": "KhachHang@123", "rememberMe": true }
```

### `POST /api/auth/admin/login` (công khai)

Đăng nhập dành cho cổng Quản trị viên (`Admin`) và Nhân viên (`NhanVien`). Nếu tài khoản là `KhachHang`, hệ thống trả về HTTP 403 từ chối quyền truy cập.

```json
{ "username": "admin", "password": "Admin@123", "rememberMe": false }
```

### `POST /api/auth/login` (công khai - kế thừa)

Đăng nhập tổng quát (hỗ trợ mọi vai trò hợp lệ).

```json
{ "username": "admin", "password": "Admin@123", "rememberMe": false }
```

Trả về:

```json
{
  "token": "eyJhbGciOi...",
  "expiresIn": 900,
  "user": {
    "accountId": 1, "username": "admin", "email": "admin@coachbooking.com",
    "role": "Admin", "roleId": 1, "employeeId": 1, "customerId": null,
    "fullName": "Nguyễn Văn An", "isActive": true
  }
}
```

`employeeId`/`customerId` là `null` nếu tài khoản chưa gắn với hồ sơ.
`expiresIn` tính bằng giây, có thể nhỏ hơn 900 khi gần hết phiên.
Login đặt cookie `coachbooking_refresh`: `HttpOnly`, `SameSite=Lax`, path `/api/auth`,
không có Domain; `Secure=true` mặc định. Chỉ local HTTP được cấu hình
`APP_AUTH_COOKIE_SECURE=false`. Refresh token không xuất hiện trong JSON.

### `POST /api/auth/refresh` (không cần access token)

Không có body; dùng refresh cookie và hai header chống CSRF ở trên. Trả về cùng
shape `{token, user, expiresIn}` như login và thay refresh cookie. Token cũ chỉ dùng
được một lần; tái sử dụng sẽ thu hồi cả phiên, kể cả access token còn hạn. Token
thiếu/sai/hết hạn hoặc phiên bị thu hồi trả `401` và xóa cookie.

### `POST /api/auth/logout` (không cần access token)

Dùng refresh cookie và hai header chống CSRF. Thu hồi phiên hiện tại, xóa cookie,
trả `204`. Cookie không còn tồn tại hoặc gọi lại vẫn trả `204`. Lỗi hạ tầng không
được coi là logout thành công; client cần thông báo và cho phép thử lại.

Access token bị lớp xác thực từ chối tại endpoint được bảo vệ trả `401` kèm
`X-Auth-Expired: 1`. Frontend chỉ refresh và thử lại request này một lần; không
tự gửi lại lỗi mạng, `5xx`, `403` hoặc `401` từ nghiệp vụ.

### `POST /api/auth/register` (công khai)

```json
{ "tenDangNhap": "kh3", "email": "kh3@gmail.com", "matKhau": "KhachHang@123", "xacNhanMatKhau": "KhachHang@123" }
```

Trả về `{ "message": "Đăng ký thành công." }`. Ràng buộc: tên đăng nhập ≥ 6 ký tự,
mật khẩu ≥ 8 ký tự có hoa/thường/số/ký tự đặc biệt, email đúng định dạng, không trùng.

### `POST /api/auth/forgot-password` / `POST /api/auth/quen-mat-khau` (công khai)

```json
{ "email": "kh1@coachbooking.com" }
```

Sinh mã OTP 6 chữ số (hiệu lực 15 phút, lưu hash SHA-256 trong `password_reset_tokens`,
giới hạn cooldown 60 giây) và gửi qua SMTP tới email đã đăng ký. Trả về:
`{ "message": "Mã xác nhận OTP đã được gửi tới email của bạn. Vui lòng kiểm tra hộp thư." }`.

### `POST /api/auth/verify-reset-otp` (công khai)

```json
{ "email": "kh1@coachbooking.com", "otp": "123456" }
```

Xác thực mã OTP 6 chữ số (tối đa 5 lần nhập sai). Nếu hợp lệ, trả về `resetToken`
để chuyển sang bước nhập mật khẩu mới:
`{ "message": "Xác thực OTP thành công.", "resetToken": "..." }`.

### `POST /api/auth/reset-password` (công khai)

```json
{
  "email": "kh1@coachbooking.com",
  "resetToken": "...",
  "newPassword": "KhachHang@456"
}
```

Xác thực `resetToken` đã qua bước OTP, cập nhật mật khẩu mới (BCrypt)
và trả về `{ "message": "Đặt lại mật khẩu thành công. Bạn có thể đăng nhập bằng mật khẩu mới." }`.

### `GET /api/auth/me`

Trả về đúng đối tượng `user` như trên.

### `POST /api/auth/change-password`

```json
{ "matKhauCu": "KhachHang@123", "matKhauMoi": "KhachHang@456" }
```

Đổi/reset mật khẩu, admin đổi mật khẩu/quyền/trạng thái tài khoản sẽ thu hồi toàn bộ
phiên của tài khoản trong cùng transaction. Đổi mật khẩu của chính mình xóa cookie
hiện tại và frontend yêu cầu đăng nhập lại. Mở khóa không khôi phục phiên cũ.

### `GET /api/oauth2/authorization/google` (công khai)

Khởi tạo luồng Google OAuth2 (Authorization Code Flow) cho cổng Khách hàng:
- Chuyển hướng người dùng tới Google Consent Screen.
- Google callback về `/api/login/oauth2/code/google`.
- Tham số tùy chọn `rememberMe=true`; mặc định `false`. Lựa chọn được gắn với OAuth2 request trong cookie có chữ ký và hạn 180 giây.
- Thành công: tự động liên kết hoặc tạo tài khoản `KhachHang` (`auth_provider = GOOGLE`), tạo phiên, đặt refresh cookie và chuyển về `${APP_OAUTH2_REDIRECT_URI}`. Không đưa token vào URL; frontend gọi `/api/auth/refresh` để hoàn tất.
- Thất bại (tài khoản bị khóa, tài khoản `Admin`/`NhanVien`, hoặc từ chối): chuyển hướng về `/login?error=<message>`.

## 2. Danh mục (CRUD)

Mọi tài nguyên dưới đây có cùng bộ endpoint:

| Method | Path | Quyền |
|---|---|---|
| `GET` | `/api/{resource}` | Admin, NhanVien |
| `GET` | `/api/{resource}/{id}` | Admin, NhanVien |
| `POST` | `/api/{resource}` | Admin (riêng `customers`: Admin + NhanVien) |
| `PUT` | `/api/{resource}/{id}` | Admin (riêng `customers`: Admin + NhanVien) |
| `DELETE` | `/api/{resource}/{id}` | Admin |

Các nguồn (`{resource}`): `bus-companies`, `vehicle-types`, `vehicles`, `routes`, `drivers`, `ticket-prices`,
`trips`, `customers`, `employees`, `accounts`, `roles`.

`employees`, `accounts`, `roles` chỉ dành cho `Admin` ở mọi thao tác.

Riêng `GET /api/routes` và `GET /api/routes/{id}` cũng mở công khai (phục vụ ô chọn điểm đi/điểm đến khi tìm chuyến). Các trường tham chiếu trả về dạng
mã số; giao diện tự ghép tên từ danh sách tham chiếu đã tải.

Trường dữ liệu (camelCase, khớp cột trong PostgreSQL):

| Nguồn | Trường |
|---|---|
| `bus-companies` | `maNx, tenNx, sdt, email, trangThai` |
| `vehicle-types` | `maLx, tenLx, soCho, trangThai` |
| `vehicles` | `maXe, maNx, maLx, bienSoXe, trangThai` |
| `routes` | `maTuyen, diemDi, diemDen, quangDuong, thoiGianDuKien, trangThai` (`thoiGianDuKien` dạng `"HH:mm:ss"`) |
| `drivers` | `maTx, tenTx, namSinh, cccd, gioiTinh, sdt, diaChi, bangLai, ngayCap, trangThai` |
| `ticket-prices` | `maGv, maNx, maLx, maTuyen, giaVe` |
| `trips` | xem mục 3 |
| `customers` | `maKh, tenKh, namSinh, sdt, gioiTinh, maTk` |
| `employees` | `maNv, tenNv, namSinh, diaChi, sdt, gioiTinh, trangThai, maTk` |
| `accounts` | `maTk, tenDangNhap, matKhau` (chỉ ghi), `email, maQuyen, trangThai` |
| `roles` | `maQuyen, tenQuyen, moTa` |

Quy tắc xóa: chặn khi còn dữ liệu tham chiếu (nhà xe còn xe, loại xe còn xe,
tuyến còn chuyến, tài xế còn chuyến, chuyến đã có vé, khách hàng đã có hóa đơn,
nhân viên đã có hóa đơn, tài khoản đang được gắn với nhân viên/khách hàng).

## 3. Chuyến xe

`GET /api/trips` trả về danh sách:

```json
[{
  "maChuyen": 1, "maXe": 1, "bienSoXe": "29A-12345",
  "maNhaXe": 1, "tenNhaXe": "Nhà xe Phương Trang",
  "maLoaiXe": 1, "tenLoaiXe": "Ghế ngồi", "soCho": 40,
  "maTuyen": 1, "diemDi": "TP HCM", "diemDen": "Vũng Tàu",
  "maTx": 1, "tenTx": "Nguyễn Văn Hậu",
  "ngayGioKhoiHanh": "2026-11-05T07:00:00",
  "ngayGioDenDuKien": "2026-11-05T09:00:00",
  "giaVeThucTe": 150000, "soVeDaBan": 2
}]
```

`POST`/`PUT` nhận `{ maXe, maTuyen, maTx, ngayGioKhoiHanh }`. Hệ thống tự tính
`ngayGioDenDuKien` từ `thoiGianDuKien` của tuyến và tự lấy `giaVeThucTe` từ bảng
`gia-ve` theo cặp (nhà xe, loại xe, tuyến). Từ chối khi: khởi hành ở quá khứ,
cùng xe hoặc tài xế đã có chuyến chồng khoảng chạy, không tìm thấy bảng giá phù hợp.
Hai chuyến nối tiếp đúng giờ đến được phép. Các yêu cầu tạo/sửa được tuần tự hóa bằng khóa
xe rồi tài xế trong transaction. Nhà xe, xe, loại xe, tuyến và tài xế phải hoạt động.
Giá phải lớn hơn 0. Thời gian tuyến phải lớn hơn 0; khi chưa khai báo dùng mặc định 6 giờ.

### `GET /api/trips/search?diemDi=&diemDen=&ngay=YYYY-MM-DD&tuGio=&denGio=`

Dùng cho cả nhân viên và khách hàng. Chỉ trả chuyến chưa khởi hành, có giá vé > 0,
kèm `soVeDaBan`, `soCho`, `giaVeThucTe`. `tuGio`/`denGio` là số nguyên 0–23 (tùy chọn).

### `GET /api/trips/{id}/seats`

```json
{
  "maChuyen": 1, "tenLoaiXe": "Giường nằm", "soCho": 45,
  "giaVe": 180000, "gheDaDat": [1, 2, 7], "heldSeats": [5, 6],
  "diemDi": "TP HCM", "diemDen": "Vũng Tàu",
  "ngayGioKhoiHanh": "2026-11-05T07:00:00"
}
```

`heldSeats` là các ghế đang được khách khác giữ tạm thời và chưa bán. Trình duyệt có thể
theo dõi snapshot ghế theo thời gian thực qua SSE công khai.

### `GET /api/trips/{id}/seats/events`

Sự kiện `seat-status` trả `{ tripId, bookedSeats, heldSeats, occurredAt }`.

### Giữ ghế tạm thời

Khách hàng phải đăng nhập và giữ ghế trước khi xác nhận đặt vé. Hold tồn tại 10 phút,
thuộc về đúng tài khoản tạo ra và áp dụng nguyên tử cho toàn bộ danh sách ghế.

`POST /api/seat-holds` nhận:

```json
{ "tripId": 1, "seatNumbers": [3, 4] }
```

Trả về HTTP `201`:

```json
{
  "holdToken": "6ef80623-073d-4be7-a900-f9b9ac28a48a",
  "tripId": 1,
  "seatNumbers": [3, 4],
  "expiresAt": "2026-11-12T10:40:00+07:00"
}
```

- `GET /api/seat-holds/{holdToken}`: đọc hold còn hiệu lực của chính tài khoản.
- `DELETE /api/seat-holds/{holdToken}`: nhả ghế, trả HTTP `204`.
- Trả `409` nếu một ghế đã bán, đang được giữ hoặc hold đã hết hạn.

## 4. Đặt vé

### `POST /api/bookings`

Nhân viên:

```json
{
  "maChuyen": 1, "choNgois": [3, 4],
  "tenKhachHang": "Lê Văn Trung", "sdt": "0981111111",
  "namSinh": "1990-02-15", "gioiTinh": true,
  "phuongThuc": "Tiền mặt"
}
```

Khách hàng đăng nhập gửi thêm `holdToken` nhận từ API giữ ghế; phương thức trên cổng
khách hàng hiện là `Chuyển khoản` (hệ thống lấy
hồ sơ khách hàng từ token). Nếu tài khoản chưa có hồ sơ khách hàng (`maKh = null`),
lần đặt vé đầu tiên phải gửi thêm `tenKhachHang`, `sdt`, `namSinh` (và tùy chọn
`gioiTinh`) để hệ thống tạo hồ sơ gắn với tài khoản.

Trả về:

```json
{ "maHd": 21, "tongTien": 300000, "soVe": 2, "phuongThuc": "Tiền mặt", "trangThai": true }
```

Ràng buộc: chuyến tồn tại và chưa khởi hành, giá vé > 0, ghế nằm trong `1..soCho`,
ghế chưa bị đặt (kiểm tra lại trong transaction, vi phạm ràng buộc duy nhất trả `409`),
số điện thoại 10–11 ký tự, tên khách không rỗng. Hold của khách phải đúng tài khoản,
chuyến, tập ghế và chưa hết hạn. Nếu số điện thoại đã tồn tại thì dùng khách hàng cũ,
ngược lại tạo mới. Nhân viên tại quầy dùng `Tiền mặt`, `Chuyển khoản`, `Thẻ ATM` và
không được ghi đè ghế khách đang giữ. Chuyển khoản khách hàng dùng Cổng thanh toán SePay
sandbox: tạo hóa đơn chưa thanh toán và phiên giữ ghế tối đa 10 phút từ lúc đặt vé,
không vượt giờ khởi hành. Không tạo `Payment` trước khi có IPN hoặc kết quả tra cứu SePay hợp lệ.

Phản hồi khách hàng dùng tên trường tiếng Anh như DTO backend:

```json
{
  "invoiceId": 21, "totalAmount": 300000, "ticketCount": 2,
  "paymentMethod": "Chuyển khoản", "isPaid": false,
  "paymentSessionId": "6ef80623-073d-4be7-a900-f9b9ac28a48a",
  "paymentStatus": "PENDING", "expiresAt": "2026-11-16T10:10:00Z"
}
```

Gửi lại cùng `holdToken`, chủ tài khoản, chuyến, ghế và phương thức trả cùng hóa đơn/phiên,
không gia hạn. Gửi lại với dữ liệu khác trả `409` (khác chủ trả `403`).
Khi SePay chưa bật hoặc cấu hình thiếu, đặt vé khách hàng trả `503`, không tạo hóa đơn/vé.
Luồng nhân viên giữ nguyên; các trường phiên thanh toán trả `null`.

### Thanh toán SePay sandbox

- `POST /api/payments/sepay/{sessionId}/checkout`: chỉ role `KhachHang`, đúng chủ phiên.
  Trả `{ "checkoutUrl": "https://pay-sandbox.sepay.vn/v1/checkout/init", "fields": [{"name":"order_amount","value":"300000"}, ...] }`.
  Frontend tạo form HTML POST theo thứ tự `fields`; không gọi checkout bằng fetch.
  Backend tự lấy số tiền, mã tham chiếu, URL callback và ký HMAC-SHA256/Base64.
  Mở lại phiên dùng cùng mã tham chiếu/hạn. Phiên không còn `PENDING` trả `409`.
- `GET /api/payments/sepay/{sessionId}`: chỉ role `KhachHang`, đúng chủ phiên.
  Trả `{ paymentSessionId, invoiceId, paymentStatus, totalAmount, expiresAt, confirmationMode, lastCheckedAt, syncError }`.
  `confirmationMode` là `IPN` hoặc `POLLING`, được giữ cố định từ lúc tạo phiên.
  `lastCheckedAt` là ISO-8601 hoặc null; `syncError` là thông báo an toàn hoặc null.
  GET chỉ đọc database và kiểm tra hết hạn, không gọi API SePay.
  Không đăng nhập trả `401`, sai role/chủ trả `403`, không tồn tại trả `404`.
- `POST /api/payments/sepay/ipn`: không yêu cầu JWT, nhưng bắt buộc header `X-Secret-Key`
  khớp `SEPAY_IPN_SECRET` (Auth Type `SECRET_KEY`); sai/thiếu trả `401`.
  Payload theo SePay, dùng `notification_type`, `order`, `transaction` theo snake_case.
  Đối chiếu `ORDER_PAID`, `CAPTURED`, `APPROVED`, loại `PAYMENT`, phương thức
  `BANK_TRANSFER`, mã tham chiếu và số tiền/tiền tệ VND của cả order lẫn transaction.
  IPN hợp lệ cập nhật hóa đơn và tạo Payment trong cùng transaction; IPN lặp không tạo thêm Payment.
  Trả HTTP `200` với `{ "success": true }` sau khi lưu; payload sai định dạng trả `400`,
  lỗi transaction trả lỗi server để SePay gửi lại. Đơn không nhận diện được được lưu để đối soát.

Trạng thái phiên: `PENDING`, `PAID`, `EXPIRED`, `CANCELLED`, `REVIEW_REQUIRED`.
Job mỗi phút nhả ghế hết hạn; GET trạng thái/POST checkout cũng kiểm tra hạn ngay.
IPN đến muộn, thanh toán sai tiền/trạng thái hoặc thanh toán bổ sung chuyển sang
`REVIEW_REQUIRED`, lưu giao dịch để đối soát, không tự cấp lại vé/hoàn tiền.
`TRANSACTION_VOID` của phiên đã thanh toán cần đối soát; không tự đảo trạng thái hóa đơn.
Redirect thành công/lỗi/hủy chỉ điều hướng về trang trạng thái, không cập nhật thanh toán.

#### Polling dành cho local sandbox

- Bật bằng `SEPAY_CONFIRMATION_MODE=POLLING`, profile Spring `local`, SePay `sandbox`.
  Khởi động backend thất bại nếu chế độ/profile/URL local không hợp lệ.
- `POST /api/payments/sepay/{sessionId}/sync`: khách hàng đăng nhập, đúng chủ phiên.
  Trả cùng cấu trúc GET trạng thái. `401` nếu chưa đăng nhập, `403` nếu sai role/chủ,
  `404` nếu không có phiên, `409` nếu chế độ hiện tại hoặc phiên không dùng polling,
  `503` nếu thiếu cấu hình SePay. Lỗi mạng/provider được trả trong `syncError`, không giả xác nhận thanh toán.
- Worker và POST sync dùng chung lịch kiểm tra/lease đã lưu: tối thiểu 15 giây giữa các lượt
  của phiên chờ; không giữ khóa database khi gọi HTTP. `404` từ SePay trước khi mở checkout là đang chờ.
  Lỗi được backoff tới 60 giây; `429` tuân thủ `Retry-After` trong cửa sổ đối soát 24 giờ.
- Chỉ backend gọi `https://pgapi-sandbox.sepay.vn/v1/order/detail/{order_invoice_number}`
  bằng Basic Auth. Đối chiếu mã đơn, số tiền, VND và từng giao dịch PAYMENT/APPROVED;
  dùng chung transaction, khóa, chống trùng và nhật ký với IPN. Nhật ký bổ sung `source=IPN|POLLING`.
- Giữ nguyên hạn ghế. Phiên hủy/hết hạn tiếp tục đối soát mỗi phút, tối đa 24 giờ từ lúc tạo;
  giao dịch đến muộn chuyển `REVIEW_REQUIRED`, không cấp lại ghế. Phiên đã PAID dừng polling.
- Callback của phiên polling trỏ tới `{SEPAY_FRONTEND_URL}/sepay-local-return.html?sessionId={uuid}&port={3000|5173}`.
  File tĩnh chuyển về `http://localhost:{port}/thanh-toan/sepay/{uuid}`; không gọi API, không cần đăng nhập,
  không nhận URL đích tùy ý và không xác nhận thanh toán. Chế độ IPN giữ callback hiện tại.

## 5. Hóa đơn, vé

| Method | Path | Mô tả |
|---|---|---|
| `GET` | `/api/invoices?tu=&den=&trangThai=` | Danh sách (Admin, NhanVien) |
| `GET` | `/api/invoices/{id}` | Chi tiết kèm vé và thanh toán |
| `POST` | `/api/invoices/{id}/pay` | `{ "phuongThuc": "Tiền mặt" }` |
| `POST` | `/api/invoices/{id}/cancel` | Hủy toàn bộ vé của hóa đơn **chưa thanh toán** |
| `GET` | `/api/invoices/my` | Lịch sử của khách hàng đang đăng nhập |
| `POST` | `/api/tickets/{maVe}/cancel` | Khách hàng hủy 1 vé chưa thanh toán |

Danh sách hóa đơn:

```json
[{
  "maHd": 21, "tenKhachHang": "Lê Văn Trung", "tenNhanVien": "Nguyễn Văn An",
  "ngayLap": "2026-11-04T09:00:00", "tongTien": 300000,
  "trangThai": true, "soVe": 2, "phuongThuc": "Tiền mặt"
}]
```

Chi tiết hóa đơn:

```json
{
  "maHd": 21, "tenKhachHang": "Lê Văn Trung", "sdtKhachHang": "0981111111",
  "tenNhanVien": "Nguyễn Văn An", "ngayLap": "2026-11-04T09:00:00",
  "tongTien": 300000, "trangThai": true,
  "veXes": [{
    "maVe": 40, "maChuyen": 1, "diemDi": "TP HCM", "diemDen": "Vũng Tàu",
    "ngayGioKhoiHanh": "2026-11-05T07:00:00", "choNgoi": 3, "giaVe": 150000
  }],
  "thanhToan": { "maTt": 12, "tongTien": 300000, "ngayTt": "2026-11-04T09:01:00", "phuongThuc": "Tiền mặt" }
}
```

Chỉ được hủy vé khi hóa đơn chưa thanh toán và chuyến chưa khởi hành.

Với hóa đơn SePay, danh sách và chi tiết bổ sung `sepay` chứa cùng cấu trúc GET trạng thái;
hóa đơn khác trả `sepay: null`. Khách hàng không được gọi API `/pay` để tự xác nhận (`403`);
nhân viên cũng không được thanh toán thủ công hóa đơn thuộc SePay (`409`).
Chỉ hủy **toàn bộ** hóa đơn SePay qua `/invoices/{id}/cancel`; hủy từng vé trả `409`.
Hủy/hết hạn nhả ghế, giữ hóa đơn, số tiền ban đầu và phiên để đối soát/lịch sử.
Hóa đơn cũ chưa thanh toán không tự chuyển sang SePay; liên hệ quầy vé.

## 6. Hồ sơ

`GET /api/profile` và `PUT /api/profile` cho mọi tài khoản đã đăng nhập:

```json
{
  "loai": "KHACHHANG", "maKh": 1, "maNv": null,
  "hoTen": "Lê Văn Trung", "namSinh": "1990-02-15", "sdt": "0981111111",
  "gioiTinh": true, "diaChi": null, "email": "4@gmail.com", "tenDangNhap": "kh1"
}
```

`PUT` nhận các trường được sửa: `hoTen, namSinh, sdt, gioiTinh, diaChi` (nhân viên có
thêm `diaChi`). Không cho sửa email/tên đăng nhập/quyền tại đây.

## 7. Thống kê

`GET /api/statistics?tu=YYYY-MM-DD&den=YYYY-MM-DD` (Admin, NhanVien), mặc định 30 ngày gần nhất:

```json
{
  "soKhachHang": 12,
  "soChuyenXe": 28,
  "tongDoanhThu": 5400000,
  "soVe": 36,
  "doanhThuTheoNgay": [{ "ngay": "2026-11-01", "doanhThu": 450000, "soVe": 3 }],
  "tuyenPhoBien": [{ "tenTuyen": "TP HCM → Vũng Tàu", "soVe": 12 }]
}
```
