# 📜 Quy chuẩn Commit & Quản lý Nhánh (Git Conventions)

> **Dự án:** EduShare (AcademicHub)  
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
| **`feat`** | Thêm một tính năng mới cho người dùng | `feat(category): add API to create new category` |
| **`fix`** | Sửa một lỗi (bug) trong hệ thống | `fix(auth): fix token expiration calculation error` |
| **`docs`** | Thêm hoặc sửa đổi tài liệu (README, docs...) | `docs(readme): add system architecture diagram` |
| **`style`** | Thay đổi format code, khoảng trắng, không ảnh hưởng logic | `style(common): apply Google Java Format via Spotless` |
| **`refactor`** | Tái cấu trúc code (không sửa bug cũng không thêm tính năng) | `refactor(storage): extract MinIO client into separate service` |
| **`perf`** | Cải thiện hiệu năng xử lý hoặc truy vấn | `perf(search): add composite index on document title and category` |
| **`test`** | Thêm test mới hoặc chỉnh sửa test có sẵn | `test(category): add unit tests for CategoryServiceImpl` |
| **`build`** | Thay đổi cấu hình build tool, cập nhật dependency | `build(gradle): upgrade spring-boot plugin to 3.4.3` |
| **`ci`** | Thay đổi cấu hình CI/CD (GitHub Actions, SonarCloud) | `ci(github): add test and linting pipeline in ci.yml` |
| **`chore`** | Các công việc lặt vặt khác (config, .gitignore, script) | `chore(config): add .env.example template` |

---

## 3. Phạm vi áp dụng (Scopes) trong EduShare

Đặt trong dấu ngoặc đơn `(<scope>)` để chỉ rõ thay đổi nằm ở module nào:

* **`category`**: Quản lý thể loại/danh mục tài liệu.
* **`document`**: Quản lý upload, tải, xem tài liệu PDF.
* **`auth`**: Đăng ký, đăng nhập, JWT, Spring Security.
* **`moderation`**: Pipeline kiểm duyệt AI, Decision Engine, Admin review.
* **`recommendation`**: Thuật toán tính điểm và gợi ý tài liệu.
* **`storage`**: Thao tác với MinIO S3 Object Storage.
* **`queue`**: RabbitMQ broker, message producer/consumer.
* **`ai`**: Gemini API adapter, structured output.
* **`common`**: DTO dùng chung, Exception Handler, Response format.
* **`config`**: Cấu hình hệ thống, application.yaml, .env.
* **`docker`**: Docker Compose, Dockerfile.

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
git commit -m "feat(category): add unique slug validation when creating category"
git commit -m "fix(security): return HTTP 403 instead of 500 when access denied"
git commit -m "test(moderation): add unit tests for 3 decision engine branches"
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
     * `feature/category-crud`
     * `feature/auth-jwt`
     * `feature/minio-storage`
   * Sau khi hoàn thành và test pass, tạo **Pull Request (PR)** để merge vào `dev`.
4. **`hotfix/<tên-lỗi>` (Hotfix Branches):**
   * Tách trực tiếp từ `main` để sửa lỗi khẩn cấp trên Production.
