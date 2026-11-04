# CoachBooking — Hệ thống bán vé xe bến xe miền Đông

Bản viết lại của ứng dụng desktop C# (WinForms + EF6 + SQL Server) sang
**Spring Boot 3 + React + PostgreSQL**. Mã nguồn C# cũ đã được gỡ khỏi repo;
hệ thống hiện tại nằm trong `backend/` và `frontend/`.

## Kiến trúc

| Thành phần | Công nghệ | Thư mục |
|---|---|---|
| Backend | Java 25, Spring Boot 3.5, Spring Data JPA, Spring Security (JWT), Gradle 9.1 (wrapper) | `backend/` |
| Frontend | React 18, TypeScript, Vite, React Router | `frontend/` |
| Cơ sở dữ liệu | PostgreSQL 16, migration bằng Flyway | `backend/src/main/resources/db/migration/` |
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

Backend tự chạy Flyway khi khởi động: `V1__init_schema.sql` tạo schema,
`V2__seed_data.sql` nạp danh mục, bảng giá và chuyến xe 14 ngày kể từ ngày chạy.
Muốn nạp lại từ đầu:

```bash
docker compose down -v
docker compose up --build
```

Nếu volume `db-data` được tạo bởi bộ script `db/init` cũ (không có bảng
`flyway_schema_history`), Flyway sẽ đánh dấu schema đang có là phiên bản 2 thay vì
chạy lại migration; dữ liệu cũ được giữ nguyên.

## Tài khoản demo

| Tên đăng nhập | Mật khẩu | Vai trò |
|---|---|---|
| `admin` | `Admin@123` | Admin |
| `nv1` | `NhanVien@123` | Nhân viên |
| `kh1` | `KhachHang@123` | Khách hàng (đã gắn hồ sơ khách hàng) |
| `kh2` | `KhachHang@123` | Khách hàng |

Tài khoản được tạo bằng BCrypt ở lần khởi động backend đầu tiên
(`backend/src/main/java/com/coachbooking/config/DataInitializer.java`).

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
./gradlew clean bootJar   # build/libs/coachbooking-backend-1.0.0.jar
```

Build Docker không cần `gradlew`/wrapper jar: image `gradle:9.1.0-jdk25` được dùng
trực tiếp trong `backend/Dockerfile`.

Biến môi trường backend có thể ghi đè trực tiếp:

```text
SPRING_DATASOURCE_URL=jdbc:postgresql://localhost:5432/coachbooking
SPRING_DATASOURCE_USERNAME=coachbooking
SPRING_DATASOURCE_PASSWORD=coachbooking
APP_JWT_SECRET=<chuỗi tối thiểu 32 ký tự>
APP_CORS_ALLOWED_ORIGINS=http://localhost:5173,http://localhost:3000
```

## CI/CD và SonarQube

GitHub Actions chạy khi push vào `main`, `dev`, `feat/**`, `fix/**` và mọi pull request vào `main`/`dev`:

| Job | Nội dung |
|---|---|
| `backend` | JDK 25 + Gradle 9.1.0: `spotlessCheck`, `build`, `jacocoTestReport`, sau đó `sonar` nếu có token |
| `frontend` | Node 20, `npm ci`, `npm run build` |
| `docker` | kiểm tra `docker compose config`, build thử cả hai image |

### Cấu hình SonarQube

Project key, organization và host nằm trong `backend/build.gradle` (block `sonarqube`), đang trỏ tới
`phamleanhkhoa101_QLBXMD` / `phamleanhkhoa101`.

1. Tạo token ở **My Account → Security** trên SonarCloud.
2. Thêm secret `SONAR_TOKEN` trong GitHub repo (Settings → Secrets and variables → Actions).
3. Push code; job backend tự chạy `gradle sonar`.

Không cần biến `SONAR_HOST_URL` vì host đã khai báo trong `build.gradle`. Khi chưa có token, bước Sonar tự bỏ qua.

Định dạng code: `./gradlew spotlessApply` để tự sửa, `./gradlew spotlessCheck` để kiểm tra. Quality gate chưa bật `wait` vì dự án chưa có test, coverage 0% sẽ làm gate fail.

### Phát hành image

Push tag dạng `v1.2.3`, hoặc chạy tay workflow **CD**, để build và đẩy image lên GHCR:

- `ghcr.io/phamleanhkhoa101/coachbooking-backend`
- `ghcr.io/phamleanhkhoa101/coachbooking-frontend`

Bước triển khai thực tế chưa được tự động hóa; thêm job deploy sau bước publish nếu cần.

> Đã bổ sung `backend/gradle/wrapper/gradle-wrapper.jar` và `frontend/package-lock.json`, nên chạy local bằng `./gradlew` và cài dependency bằng `npm ci` được. CI vẫn cài Gradle 9.1.0 trực tiếp để không phụ thuộc wrapper jar.

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
- Bộ kiểm thử tự động cho nghiệp vụ.
