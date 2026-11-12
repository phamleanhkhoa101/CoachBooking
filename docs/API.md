# Hợp đồng API — CoachBooking (Spring Boot + React)

Base URL: `/api`. Mọi endpoint (trừ nhóm `auth` công khai) yêu cầu header
`Authorization: Bearer <token>`.

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

### `POST /api/auth/user/login` (công khai)

Đăng nhập dành cho cổng Khách hàng (`KhachHang`). Nếu tài khoản có quyền `Admin` hoặc `NhanVien`, hệ thống trả về HTTP 403 yêu cầu đăng nhập tại cổng Quản trị.

```json
{ "tenDangNhap": "kh1", "matKhau": "KhachHang@123" }
```

### `POST /api/auth/admin/login` (công khai)

Đăng nhập dành cho cổng Quản trị viên (`Admin`) và Nhân viên (`NhanVien`). Nếu tài khoản là `KhachHang`, hệ thống trả về HTTP 403 từ chối quyền truy cập.

```json
{ "tenDangNhap": "admin", "matKhau": "Admin@123" }
```

### `POST /api/auth/login` (công khai - kế thừa)

Đăng nhập tổng quát (hỗ trợ mọi vai trò hợp lệ).

```json
{ "tenDangNhap": "admin", "matKhau": "Admin@123" }
```

Trả về:

```json
{
  "token": "eyJhbGciOi...",
  "nguoiDung": {
    "maTk": 1, "tenDangNhap": "admin", "email": "admin@coachbooking.com",
    "quyen": "Admin", "maQuyen": 1, "maNv": 1, "maKh": null,
    "hoTen": "Nguyễn Văn An", "trangThai": true
  }
}
```

`maNv`/`maKh` là `null` nếu tài khoản chưa gắn với nhân viên/khách hàng.

### `POST /api/auth/register` (công khai)

```json
{ "tenDangNhap": "kh3", "email": "kh3@gmail.com", "matKhau": "KhachHang@123", "xacNhanMatKhau": "KhachHang@123" }
```

Trả về `{ "message": "Đăng ký thành công." }`. Ràng buộc: tên đăng nhập ≥ 6 ký tự,
mật khẩu ≥ 8 ký tự có hoa/thường/số/ký tự đặc biệt, email đúng định dạng, không trùng.

### `POST /api/auth/quen-mat-khau` (công khai)

```json
{ "tenDangNhap": "kh1", "email": "kh1@coachbooking.com" }
```

Trả về `{ "matKhauTam": "Abc123!xyz" }` (môi trường demo trả mật khẩu tạm trực tiếp
thay vì gửi email — có ghi chú rõ trong README).

### `GET /api/auth/me`

Trả về đúng đối tượng `nguoiDung` như trên.

### `POST /api/auth/doi-mat-khau`

```json
{ "matKhauCu": "KhachHang@123", "matKhauMoi": "KhachHang@456" }
```

## 2. Danh mục (CRUD)

Mọi tài nguyên dưới đây có cùng bộ endpoint:

| Method | Path | Quyền |
|---|---|---|
| `GET` | `/api/{nguon}` | Admin, NhanVien |
| `GET` | `/api/{nguon}/{id}` | Admin, NhanVien |
| `POST` | `/api/{nguon}` | Admin (riêng `khach-hang`: Admin + NhanVien) |
| `PUT` | `/api/{nguon}/{id}` | Admin (riêng `khach-hang`: Admin + NhanVien) |
| `DELETE` | `/api/{nguon}/{id}` | Admin |

Các nguồn: `nha-xe`, `loai-xe`, `xe`, `tuyen-duong`, `tai-xe`, `gia-ve`,
`chuyen-xe`, `khach-hang`, `nhan-vien`, `tai-khoan`, `quyen`.

`nhan-vien`, `tai-khoan`, `quyen` chỉ dành cho `Admin` ở mọi thao tác.

Riêng `GET /api/tuyen-duong` và `GET /api/tuyen-duong/{id}` cũng mở cho `KhachHang`
(phục vụ ô chọn điểm đi/điểm đến khi tìm chuyến). Các trường tham chiếu trả về dạng
mã số; giao diện tự ghép tên từ danh sách tham chiếu đã tải.

Trường dữ liệu (camelCase, khớp cột trong PostgreSQL):

| Nguồn | Trường |
|---|---|
| `nha-xe` | `maNx, tenNx, sdt, email, trangThai` |
| `loai-xe` | `maLx, tenLx, soCho, trangThai` |
| `xe` | `maXe, maNx, maLx, bienSoXe, trangThai` |
| `tuyen-duong` | `maTuyen, diemDi, diemDen, quangDuong, thoiGianDuKien, trangThai` (`thoiGianDuKien` dạng `"HH:mm:ss"`) |
| `tai-xe` | `maTx, tenTx, namSinh, cccd, gioiTinh, sdt, diaChi, bangLai, ngayCap, trangThai` |
| `gia-ve` | `maGv, maNx, maLx, maTuyen, giaVe` |
| `chuyen-xe` | xem mục 3 |
| `khach-hang` | `maKh, tenKh, namSinh, sdt, gioiTinh, maTk` |
| `nhan-vien` | `maNv, tenNv, namSinh, diaChi, sdt, gioiTinh, trangThai, maTk` |
| `tai-khoan` | `maTk, tenDangNhap, matKhau` (chỉ ghi), `email, maQuyen, trangThai` |
| `quyen` | `maQuyen, tenQuyen, moTa` |

Quy tắc xóa: chặn khi còn dữ liệu tham chiếu (nhà xe còn xe, loại xe còn xe,
tuyến còn chuyến, tài xế còn chuyến, chuyến đã có vé, khách hàng đã có hóa đơn,
nhân viên đã có hóa đơn, tài khoản đang được gắn với nhân viên/khách hàng).

## 3. Chuyến xe

`GET /api/chuyen-xe` trả về danh sách:

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
cùng xe đã có chuyến trong cùng phút, không tìm thấy bảng giá phù hợp.

### `GET /api/chuyen-xe/tim?diemDi=&diemDen=&ngay=YYYY-MM-DD&tuGio=&denGio=`

Dùng cho cả nhân viên và khách hàng. Chỉ trả chuyến chưa khởi hành, có giá vé > 0,
kèm `soVeDaBan`, `soCho`, `giaVeThucTe`. `tuGio`/`denGio` là số nguyên 0–23 (tùy chọn).

### `GET /api/chuyen-xe/{id}/ghe`

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
  "expiresAt": "2026-11-12T10:40:00"
}
```

- `GET /api/seat-holds/{holdToken}`: đọc hold còn hiệu lực của chính tài khoản.
- `DELETE /api/seat-holds/{holdToken}`: nhả ghế, trả HTTP `204`.
- Trả `409` nếu một ghế đã bán, đang được giữ hoặc hold đã hết hạn.

## 4. Đặt vé

### `POST /api/dat-ve`

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
không được ghi đè ghế khách đang giữ. Chuyển khoản khách hàng hiện là luồng mô phỏng
tự xác nhận, chưa có callback đối soát ngân hàng.

## 5. Hóa đơn, vé

| Method | Path | Mô tả |
|---|---|---|
| `GET` | `/api/hoa-don?tu=&den=&trangThai=` | Danh sách (Admin, NhanVien) |
| `GET` | `/api/hoa-don/{id}` | Chi tiết kèm vé và thanh toán |
| `POST` | `/api/hoa-don/{id}/thanh-toan` | `{ "phuongThuc": "Tiền mặt" }` |
| `POST` | `/api/hoa-don/{id}/huy` | Hủy toàn bộ vé của hóa đơn **chưa thanh toán** |
| `GET` | `/api/hoa-don/cua-toi` | Lịch sử của khách hàng đang đăng nhập |
| `POST` | `/api/ve/{maVe}/huy` | Khách hàng hủy 1 vé chưa thanh toán |

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

## 6. Hồ sơ

`GET /api/ho-so` và `PUT /api/ho-so` cho mọi tài khoản đã đăng nhập:

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

`GET /api/thong-ke?tu=YYYY-MM-DD&den=YYYY-MM-DD` (Admin, NhanVien), mặc định 30 ngày gần nhất:

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
