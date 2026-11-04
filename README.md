# QLBXMD — Hệ thống bán vé xe bến xe miền Đông

Bản viết lại của ứng dụng desktop C# (`QLBXMD/`, WinForms + EF6 + SQL Server) sang
**Spring Boot 3 + React + PostgreSQL**. Toàn bộ mã C# cũ vẫn được giữ nguyên trong
repo để đối chiếu; hệ thống mới chạy độc lập trong `backend/` và `frontend/`.

## Kiến trúc

| Thành phần | Công nghệ | Thư mục |
|---|---|---|
| Backend | Java 25, Spring Boot 3.5, Spring Data JPA, Spring Security (JWT), Gradle 9.1 (wrapper) | `backend/` |
| Frontend | React 18, TypeScript, Vite, React Router | `frontend/` |
| Cơ sở dữ liệu | PostgreSQL 16, khởi tạo bằng script SQL | `db/init/` |
| Hạ tầng | Docker Compose (db + backend + frontend) | `docker-compose.yml` |

Luồng dữ liệu: React gọi `/api/**` → nginx (container frontend) chuyển tiếp tới
`backend:8080` → JPA → PostgreSQL. Khi chạy dev, Vite proxy `/api` sang
`http://localhost:8080`.

## Chạy nhanh bằng Docker

```bash
cp .env.example .env      # Windows: copy .env.example .env
docker compose up --build
```

| Dịch vụ | Địa chỉ |
|---|---|
| Giao diện | http://localhost:3000 |
| API | http://localhost:8080/api |
| Health check | http://localhost:8080/actuator/health |
| PostgreSQL | localhost:5432 |

Dữ liệu mẫu (danh mục, bảng giá, chuyến xe 14 ngày kể từ ngày khởi tạo) được nạp tự
động ở lần đầu container `db` khởi động. Muốn nạp lại từ đầu:

```bash
docker compose down -v
docker compose up --build
```

## Tài khoản demo

| Tên đăng nhập | Mật khẩu | Vai trò |
|---|---|---|
| `admin` | `Admin@123` | Admin |
| `nv1` | `NhanVien@123` | Nhân viên |
| `kh1` | `KhachHang@123` | Khách hàng (đã gắn hồ sơ khách hàng) |
| `kh2` | `KhachHang@123` | Khách hàng |

Tài khoản được tạo bằng BCrypt ở lần khởi động backend đầu tiên
(`backend/src/main/java/vn/qlbxmd/config/DataInitializer.java`).

## Chạy khi phát triển (không dùng container cho backend/frontend)

Yêu cầu: JDK 25 cho backend (`java -version` phải là 25). Gradle 9.1.0 được wrapper
tự tải về, không cần cài Gradle.

Repo không chứa `backend/gradle/wrapper/gradle-wrapper.jar` (file nhị phân). Nếu thiếu,
sinh một lần bằng Docker trước khi dùng `gradlew`:

```bash
docker run --rm -v "${PWD}/backend:/home/gradle/project" -w /home/gradle/project \
  gradle:9.1.0-jdk25 gradle wrapper --gradle-version 9.1.0
```

```bash
# 1. Chỉ dựng PostgreSQL
docker compose up -d db

# 2. Backend (http://localhost:8080)
cd backend
./gradlew bootRun        # Windows: gradlew.bat bootRun

# 3. Frontend (http://localhost:5173)
cd frontend
npm install
npm run dev
```

Đóng gói backend:

```bash
cd backend
./gradlew clean bootJar   # build/libs/qlbxmd-backend-1.0.0.jar
```

Build Docker không cần `gradlew`/wrapper jar: image `gradle:9.1.0-jdk25` được dùng
trực tiếp trong `backend/Dockerfile`.

Biến môi trường backend có thể ghi đè trực tiếp:

```text
SPRING_DATASOURCE_URL=jdbc:postgresql://localhost:5432/qlbxmd
SPRING_DATASOURCE_USERNAME=qlbxmd
SPRING_DATASOURCE_PASSWORD=qlbxmd
APP_JWT_SECRET=<chuỗi tối thiểu 32 ký tự>
APP_CORS_ALLOWED_ORIGINS=http://localhost:5173,http://localhost:3000
```

## API

Hợp đồng đầy đủ ở [`docs/API.md`](docs/API.md): xác thực JWT, danh mục, tìm chuyến,
sơ đồ ghế, đặt vé, hóa đơn, hủy vé, hồ sơ và thống kê.

## Khác biệt so với bản C#

- **Database**: SQL Server → PostgreSQL; đặt tên bảng/cột `snake_case`, giữ nguyên
  quan hệ và ràng buộc. Script `HeThongBanVeBenXeMienDong.sql` (có `DROP DATABASE`)
  không còn được dùng.
- **Trigger → service**: các trigger `trg_TinhNgayDen`, `trg_GiaVeChoChuyenXe`,
  `trg_GiaVeVeXe`, `trg_TongTien_*`, `trg_ThanhToan_HoaDon` được chuyển thành logic
  trong `TripService`, `BookingService`, `InvoiceService` để dễ kiểm thử và không phụ
  thuộc dialect.
- **Mật khẩu**: SHA256 + Base64 (không salt) → BCrypt.
- **Thanh toán MoMo**: đã gỡ khỏi phạm vi chuyển đổi. Phương thức còn lại: Tiền mặt,
  Chuyển khoản, Thẻ ATM; khách hàng có thêm "Thanh toán sau".
- **Email**: môi trường demo không cấu hình SMTP, endpoint quên mật khẩu trả trực tiếp
  mật khẩu tạm trong response thay vì gửi mail.
- **Tầng dữ liệu**: EF6 EDMX → entity JPA phẳng (khóa ngoại là mã số, không dùng
  `@ManyToOne`) để tránh N+1 và lỗi tuần tự hóa vòng; các màn hình cần ghép tên dùng
  truy vấn chiếu riêng.
- **UI**: WinForms → web; phân quyền theo vai trò ở cả backend (`@PreAuthorize`) và
  frontend (route guard).

## Phạm vi đã chuyển

Đăng nhập/đăng ký/quên mật khẩu, phân quyền 3 vai trò, 11 danh mục, lập chuyến và
bảng giá, tìm chuyến, sơ đồ ghế chống trùng, bán vé tại quầy, khách tự đặt vé, hóa
đơn, thanh toán, hủy vé chưa thanh toán, lịch sử mua vé, hồ sơ cá nhân, thống kê
doanh thu và tuyến phổ biến.

## Chưa có trong bản chuyển đổi

- Thanh toán MoMo và callback (theo yêu cầu tạm bỏ).
- Gửi email thật qua SMTP.
- Bộ kiểm thử tự động cho nghiệp vụ (bản C# cũng chỉ có test rỗng).
