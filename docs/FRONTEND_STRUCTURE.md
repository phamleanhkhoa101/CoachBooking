# Cấu trúc Thư mục Frontend — CoachBooking

Tài liệu quy định và hướng dẫn cấu trúc mã nguồn thư mục `frontend/src` của dự án CoachBooking.
Kiến trúc phân chia rõ ràng theo **Role / Domain** nhằm tách biệt giữa **Cổng Khách hàng (Customer Portal - Port 3000)** và **Cổng Quản trị (Admin/Staff Portal - Port 3001)**.

---

## 1. Sơ đồ Cấu trúc Tổng thể

```text
frontend/src/
├── api/                     # Quản lý giao tiếp API với Backend
│   ├── client.ts            # Fetch wrapper, interceptor JWT token, error handling
│   └── types.ts             # TypeScript definitions khớp chuẩn docs/API.md
│
├── auth/                    # Logic xác thực, lưu token và phân quyền
│   ├── AuthContext.tsx      # React Context quản lý trạng thái đăng nhập
│   └── portal.ts            # Hàm nhận diện cổng truy cập (Admin port 3001 vs Customer port 3000)
│
├── components/              # Các UI component dùng chung
│   ├── common/              # Thành phần nguyên tử (Atomic / Shared UI)
│   │   ├── Badge.tsx        # Huy hiệu trạng thái
│   │   ├── FormField.tsx    # Ô nhập form chuẩn
│   │   ├── Modal.tsx        # Hộp thoại popup
│   │   └── Toast.tsx        # Thông báo Toast nổi
│   ├── layout/              # Layout khung ứng dụng
│   │   └── AppLayout.tsx    # Layout sidebar cho bảng điều khiển
│   └── table/               # Bảng biểu và thao tác CRUD quản trị
│       ├── DataTable.tsx    # Bảng hiển thị dữ liệu phân trang, sắp xếp
│       └── CrudPage.tsx     # Trang khung CRUD mẫu dùng chung
│
├── pages/                   # Các trang ứng dụng phân chia theo Role
│   ├── auth/                # Cụm trang xác thực người dùng
│   │   ├── Login.tsx        # Trang đăng nhập (hỗ trợ phân biệt cổng Admin/Khách)
│   │   ├── Register.tsx     # Trang đăng ký tài khoản khách hàng
│   │   └── ForgotPassword.tsx # Trang yêu cầu cấp lại mật khẩu
│   │
│   ├── customer/            # Phân hệ CỔNG KHÁCH HÀNG (Port 3000)
│   │   ├── Home.tsx         # Trang chủ với Hero Section đặt vé SAMCO
│   │   ├── Home.css         # Style giao diện trang chủ
│   │   ├── CustomerBooking.tsx # Trang đặt vé khách hàng (kèm sơ đồ ghế)
│   │   ├── CustomerHistory.tsx # Lịch sử vé đã mua
│   │   ├── Booking.tsx      # Logic cốt lõi đặt vé & chọn chỗ ngồi
│   │   └── components/
│   │       └── SeatMap.tsx  # Sơ đồ ghế 2 tầng (chuyên phục vụ đặt vé)
│   │
│   └── admin/               # Phân hệ CỔNG QUẢN TRỊ (Port 3001)
│       ├── AdminDashboard.tsx # Bảng điều khiển thống kê doanh thu
│       ├── AdminBooking.tsx   # Nghiệp vụ bán vé tại quầy (Nhân viên)
│       ├── Invoices.tsx       # Quản lý hóa đơn và thanh toán
│       ├── operations/        # Vận hành xe và lịch trình
│       │   ├── Trips.tsx        # Quản lý chuyến xe
│       │   ├── Routes.tsx       # Quản lý tuyến đường
│       │   ├── Vehicles.tsx     # Quản lý xe
│       │   ├── VehicleTypes.tsx # Quản lý loại xe
│       │   ├── BusCompanies.tsx # Quản lý nhà xe đối tác
│       │   ├── Prices.tsx       # Bảng giá vé niêm yết
│       │   └── Drivers.tsx      # Quản lý tài xế
│       └── system/            # Quản trị hệ thống cấp cao
│           ├── Accounts.tsx     # Quản lý tài khoản đăng nhập
│           ├── Customers.tsx    # Quản lý danh sách khách hàng
│           ├── Employees.tsx    # Quản lý nhân viên
│           └── Profile.tsx      # Hồ sơ cá nhân người dùng
│
├── utils/                   # Tiện ích dùng chung
│   └── format.ts            # Định dạng tiền tệ VND, ngày giờ, ISO
│
├── App.tsx                  # Định tuyến ứng dụng React Router
├── main.tsx                 # Điểm khởi động ứng dụng React
└── styles.css               # CSS toàn cục hệ thống
```

---

## 2. Nguyên tắc Phân chia & Quy ước Code

1. **Tách biệt Cổng người dùng**:
   - Các trang thuộc `pages/customer/` phục vụ khách hàng tìm kiếm, đặt vé, xem lịch sử giao dịch.
   - Các trang thuộc `pages/admin/` phục vụ nhân viên điều hành và quản trị viên quản lý nghiệp vụ bến xe.
   - Các trang thuộc `pages/auth/` dùng chung cho luồng đăng nhập, đăng ký và quên mật khẩu.

2. **Quy tắc Import**:
   - Tuyệt đối không import chéo các component nghiệp vụ đặc thù của `pages/admin/` sang `pages/customer/`.
   - Các component tái sử dụng nhiều nơi (như `Badge`, `Modal`, `DataTable`, `Toast`) phải đặt trong `components/`.
   - Component `SeatMap` gắn liền với nghiệp vụ chọn ghế hành khách, được đặt trong `pages/customer/components/` hoặc chia sẻ qua `components/`.

3. **Cập nhật Route trong `App.tsx`**:
   - Luôn sử dụng path import tường minh theo từng phân hệ.
   - Giữ nguyên các bọc bảo vệ quyền truy cập (`RequireRole`).
