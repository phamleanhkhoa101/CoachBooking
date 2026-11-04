# 📜 Quy chuẩn Commit & Quản lý Nhánh (Git Conventions)

> **Dự án:** CoachBooking — Hệ thống bán vé xe bến xe miền Đông  
> **Tiêu chuẩn áp dụng:** [Conventional Commits v1.0.0](https://www.conventionalcommits.org/en/v1.0.0/)

---

## 1. Cấu trúc một Commit Message chuẩn

Một commit message chuẩn bao gồm 3 phần: **Header**, **Body (tùy chọn)**, và **Footer (tùy chọn)**.

```text
<type>(<scope>): <mô tả ngắn bằng thể mệnh lệnh>

[Phần thân (body) mô tả chi tiết: Tại sao thay đổi này cần thiết? Nó giải quyết vấn đề gì?]

[Phần chân (footer): Breaking changes hoặc link tới Issue, PR ví dụ: Closes #12]
```

### Quy tắc định dạng dòng Header:
* Không viết hoa chữ cái đầu tiên của `<type>` và `<description>`.
* Sử dụng thể mệnh lệnh ở thì hiện tại (ví dụ: dùng `add` thay vì `added` hoặc `adds`).
* Không đặt dấu chấm `.` ở cuối dòng mô tả.
* Dòng header không vượt quá 72 ký tự.

---

## 2. Các tiền tố (Types) được sử dụng trong dự án

| Type | Ý nghĩa | Ví dụ thực tế |
| :--- | :--- | :--- |
| **`feat`** | Thêm một tính năng mới cho người dùng | `feat(trip): add API to create a new trip` |
| **`fix`** | Sửa một lỗi (bug) trong hệ thống | `fix(auth): fix token expiration calculation error` |
| **`docs`** | Thêm hoặc sửa đổi tài liệu (README, docs...) | `docs(readme): add system architecture diagram` |
| **`style`** | Thay đổi format code, khoảng trắng, không ảnh hưởng logic | `style(common): apply Google Java Format via Spotless` |
| **`refactor`** | Tái cấu trúc code (không sửa bug cũng không thêm tính năng) | `refactor(booking): extract seat validation into a shared helper` |
| **`perf`** | Cải thiện hiệu năng xử lý hoặc truy vấn | `perf(trip): add composite index on departure time and route` |
| **`test`** | Thêm test mới hoặc chỉnh sửa test có sẵn | `test(booking): add unit tests for seat conflict handling` |
| **`build`** | Thay đổi cấu hình build tool, cập nhật dependency | `build(gradle): upgrade spring-boot plugin to 3.5.16` |
| **`ci`** | Thay đổi cấu hình CI/CD (GitHub Actions, SonarCloud) | `ci(github): add build and SonarCloud pipeline` |
| **`chore`** | Các công việc lặt vặt khác (config, .gitignore, script) | `chore(config): add .env.example template` |

---

## 3. Phạm vi áp dụng (Scopes) trong CoachBooking

Đặt trong dấu ngoặc đơn `(<scope>)` để chỉ rõ thay đổi nằm ở module nào:

* **`auth`**: Đăng nhập, đăng ký, quên/đổi mật khẩu, JWT và Spring Security.
* **`booking`**: Tìm chuyến, sơ đồ ghế, đặt vé, hủy vé.
* **`invoice`**: Hóa đơn, thanh toán và trạng thái thanh toán.
* **`trip`**: Chuyến xe, tuyến đường, lịch khởi hành, giá vé áp dụng.
* **`catalog`**: Nhà xe, loại xe, xe, tuyến đường, bảng giá, tài xế.
* **`customer`**: Khách hàng và hồ sơ khách hàng.
* **`employee`**: Nhân viên và tài khoản quản trị.
* **`statistic`**: Thống kê doanh thu và tuyến phổ biến.
* **`ui`**: Trang và component React, style giao diện.
* **`common`**: DTO, exception handler, validator dùng chung.
* **`config`**: application.yml, cấu hình Spring, dữ liệu khởi tạo.
* **`db`**: Flyway migration và seed dữ liệu.
* **`docker`**: Dockerfile, docker-compose, nginx.
* **`ci`**: GitHub Actions, SonarQube, Spotless, JaCoCo.

---

## 4. Ví dụ so sánh Đúng vs Sai

### ❌ Những commit KHÔNG ĐẠT chuẩn (Tránh dùng):
```bash
git commit -m "update code"              # Quá mơ hồ, không rõ làm gì
git commit -m "fix bug"                  # Không biết sửa bug ở đâu, bug gì
git commit -m "feat: Added new feature." # Dùng quá khứ "Added", có dấu chấm cuối dòng
git commit -m "WIP"                      # Work in progress không có ngữ cảnh
```

### ✅ Những commit ĐẠT chuẩn chuyên nghiệp:
```bash
git commit -m "feat(booking): add seat availability validation before payment"
git commit -m "fix(auth): return HTTP 403 instead of 500 when access denied"
git commit -m "test(booking): add unit tests for seat conflict handling"
git commit -m "chore(deps): add flyway-database-postgresql dependency"
```

---

## 5. Quy chuẩn phân nhánh (Git Branching Strategy)

Dự án áp dụng mô hình phân nhánh chuẩn **Git Flow**:

1. **`main` (Production):**
   * Nhánh chính, code trên nhánh này luôn ở trạng thái sẵn sàng deploy và chạy ổn định.
   * **Tuyệt đối không commit trực tiếp vào `main`**.
2. **`dev` (Development / Staging):**
   * Nhánh tích hợp code chính của các tính năng trong quá trình phát triển.
   * Tất cả các feature sau khi hoàn thành sẽ merge vào `dev`.
3. **`feature/<tên-tính-năng>` (Feature Branches):**
   * Nhánh tách ra từ `dev` để phát triển từng tính năng riêng biệt.
   * Quy ước đặt tên:
     * `feature/booking-seat-map`
     * `feature/auth-jwt`
     * `feature/trip-crud`
   * Sau khi hoàn thành và test pass, tạo **Pull Request (PR)** để merge vào `dev`.
4. **`hotfix/<tên-lỗi>` (Hotfix Branches):**
   * Tách trực tiếp từ `main` để sửa lỗi khẩn cấp trên Production.
