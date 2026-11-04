# QLBXMD — Frontend React + TypeScript

Giao diện quản trị và đặt vé cho hệ thống quản lý bán vé xe miền Đông, viết lại từ ứng dụng
WinForms cũ. Frontend giao tiếp với backend Spring Boot qua hợp đồng API tại `docs/API.md`.

## Công nghệ

- Vite + React 18 + TypeScript
- `react-router-dom` v6
- CSS thuần trong `src/styles.css` (không dùng thư viện UI ngoài)
- Gọi API bằng `fetch` qua client dùng chung (`src/api/client.ts`)

## Chạy ở môi trường phát triển

```bash
cd frontend
npm install
npm run dev
```

Dev server chạy tại `http://localhost:5173` và proxy mọi request `/api` sang backend
`http://localhost:8080` (cấu hình trong `vite.config.ts`). Hãy khởi động backend trước.

## Build

```bash
npm run build     # tsc + vite build, kết quả ở thư mục dist/
npm run preview   # xem thử bản build
```

## Tài khoản demo

| Vai trò | Tên đăng nhập | Mật khẩu |
|---|---|---|
| Quản trị viên (`Admin`) | `admin` | `Admin@123` |
| Nhân viên (`NhanVien`) | `nv1` | `NhanVien@123` |
| Khách hàng (`KhachHang`) | `kh1` | `KhachHang@123` |

## Sơ đồ route ↔ trang

| Route | Trang | Quyền |
|---|---|---|
| `/login` | `Login` | Công khai |
| `/register` | `Register` | Công khai |
| `/forgot-password` | `ForgotPassword` | Công khai |
| `/` | Điều hướng theo quyền | — |
| `/admin` | `AdminDashboard` (thống kê) | Admin, NhanVien |
| `/admin/ban-ve` | `AdminBooking` (dùng chung `Booking`, chế độ nhân viên) | Admin, NhanVien |
| `/admin/hoa-don` | `Invoices` (chi tiết dạng modal) | Admin, NhanVien |
| `/admin/khach-hang` | `Customers` (CrudPage) | Admin, NhanVien |
| `/admin/chuyen-xe` | `Trips` | Admin, NhanVien |
| `/admin/nha-xe` | `BusCompanies` (CrudPage) | Admin, NhanVien |
| `/admin/loai-xe` | `VehicleTypes` (CrudPage) | Admin, NhanVien |
| `/admin/xe` | `Vehicles` (CrudPage) | Admin, NhanVien |
| `/admin/tuyen-duong` | `RoutesPage` (CrudPage) | Admin, NhanVien |
| `/admin/gia-ve` | `Prices` (CrudPage) | Admin, NhanVien |
| `/admin/tai-xe` | `Drivers` (CrudPage) | Admin, NhanVien |
| `/admin/nhan-vien` | `Employees` (CrudPage) | Chỉ Admin |
| `/admin/tai-khoan` | `Accounts` (CrudPage) | Chỉ Admin |
| `/app/dat-ve` | `CustomerBooking` (dùng chung `Booking`, chế độ khách hàng) | KhachHang |
| `/app/lich-su` | `CustomerHistory` | KhachHang |
| `/app/ho-so` | `Profile` | KhachHang |

Route không khớp được đưa về trang mặc định theo quyền (`/admin` hoặc `/app/dat-ve`),
người chưa đăng nhập về `/login`.

## Cấu trúc thư mục

```
src/
├── api/            client.ts (fetch + token + xử lý 401), types.ts
├── auth/           AuthContext.tsx (login/logout/me + RequireRole)
├── components/     AppLayout, DataTable, Modal, FormField, Toast,
│                   SeatMap, Badge, CrudPage (form CRUD cấu hình được)
├── pages/          các trang theo sơ đồ route ở trên
├── utils/          format.ts (tiền, ngày giờ, boolean)
├── App.tsx         khai báo router
├── main.tsx        điểm vào
└── styles.css      toàn bộ giao diện
```

Toàn bộ 9 danh mục CRUD dùng chung một component `CrudPage`; mỗi trang chỉ khai báo
cột bảng, field form, danh sách tham chiếu và quyền ghi. Trang chuyến xe tách riêng vì
có logic giờ khởi hành/giá vé.

## Quy ước gọi API

- Token lưu ở `localStorage` với khóa `qlbxmd_token`, gửi qua `Authorization: Bearer <token>`.
- Gặp `401`, client xóa token và chuyển về `/login`.
- Lỗi hiển thị nguyên văn `message` trong response; xung đột ghế (`409`) được hiển thị
  rõ và sơ đồ ghế tự tải lại.
- Không dùng MoMo. Phương thức thanh toán: `Tiền mặt`, `Chuyển khoản`, `Thẻ ATM`
  (khách hàng có thêm `Thanh toán sau`).

## Ghi chú nghiệp vụ đặt vé

- Tìm chuyến theo điểm đi/điểm đến/ngày/khoảng giờ (`GET /api/chuyen-xe/tim`).
- Chọn chuyến → tải sơ đồ ghế (`GET /api/chuyen-xe/{id}/ghe`). Xe "Giường nằm" chia
  2 tầng (nửa đầu / nửa sau), các loại khác 1 tầng, lưới 4 ghế/hàng.
- Nhân viên nhập thông tin khách; khách hàng có hồ sơ (`maKh` khác null) thì lấy từ
  `GET /api/ho-so`. Khách vừa đăng ký chưa có hồ sơ (`maKh` null) sẽ thấy các ô họ tên,
  số điện thoại, ngày sinh, giới tính để backend tạo hồ sơ ở lần đặt đầu tiên.
- Khách hàng chọn `Thanh toán sau` thì gửi `phuongThuc: "Thanh toán sau"`, hóa đơn ở
  trạng thái chưa thanh toán và có thể thanh toán/hủy từng vé ở mục Lịch sử đặt vé.

## Docker

```bash
docker build -t qlbxmd-frontend ./frontend
```

- Stage 1: `node:20-alpine` chạy `npm install` + `npm run build`.
- Stage 2: `nginx:1.27-alpine` phục vụ `dist/`, SPA fallback về `index.html`,
  proxy `/api/` sang `http://backend:8080`.

`docker-compose.yml` ở thư mục gốc dựng cả backend, frontend và PostgreSQL.
