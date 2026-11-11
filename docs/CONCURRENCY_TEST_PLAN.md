# Kế hoạch & Hướng dẫn Kiểm thử Tranh chấp Đặt vé Đồng thời (Concurrency Testing with Apache JMeter)

Tài liệu này cung cấp kết quả phân tích hiện trạng xử lý đồng thời (concurrency) trong luồng đặt vé và hướng dẫn từng bước cấu hình Apache JMeter để tự kiểm thử kịch bản **2 người cùng đặt 1 ghế tại cùng một thời điểm**.

---

## 1. Phân tích Hiện trạng (System Analysis)

### 1.1. Hệ thống có bị đặt trùng ghế (Double Booking) không?
- **KHÔNG BỊ TRÙNG VÉ.**
- Hệ thống được bảo vệ bởi ràng buộc duy nhất (Unique Constraint) ở tầng Database PostgreSQL:
  ```sql
  -- File: V1__init_schema.sql (dòng 128)
  CREATE TABLE tickets (
      ...
      trip_id     INTEGER NOT NULL REFERENCES trips (trip_id),
      seat_number INTEGER NOT NULL CHECK (seat_number > 0),
      UNIQUE (trip_id, seat_number)
  );
  ```
  Ràng buộc này tự động tạo unique index `tickets_trip_id_seat_number_key`. Bất kỳ hành động insert bản ghi thứ 2 có cùng `(trip_id, seat_number)` sẽ bị PostgreSQL chặn ngay lập tức.

### 1.2. Cơ chế xử lý khi có 2 request đồng thời
1. **Application Level (`BookingService.java`):**
   - Kiểm tra ghế trống thông qua:
     ```java
     List<Integer> bookedSeats = ticketRepository.findSeatNumbersByTripId(trip.getTripId());
     ```
   - Khi 2 request đến cùng mili-giây, cả hai đều đọc database trước khi request kia commit. Do đó cả 2 đều thấy ghế còn trống (Race Condition ở tầng app).
2. **Database Level (Chốt chặn cuối):**
   - Request 1 insert vé thành công và commit transaction -> Trả về `HTTP 200 OK`.
   - Request 2 cố gắng insert vé trùng ghế -> PostgreSQL ném lỗi `duplicate key value violates unique constraint "tickets_trip_id_seat_number_key"`.
   - Spring Boot bắt ngoại lệ và kích hoạt **Rollback toàn bộ Transaction của Request 2** (hóa đơn `invoices` được tạo trước đó cũng bị xóa theo).
   - GlobalExceptionHandler bắt `DataIntegrityViolationException` và trả về `HTTP 409 Conflict`.

> [!NOTE]
> **Điểm cần lưu ý về thông điệp lỗi:**
> Trong `GlobalExceptionHandler.java`, chuỗi so sánh đang là `ve_xe_ma_chuyen_cho_ngoi_key` (tên cũ) thay vì `tickets_trip_id_seat_number_key`. Do đó, người đặt sau hiện tại sẽ nhận thông báo `HTTP 409 Conflict: "Dữ liệu bị trùng hoặc vi phạm ràng buộc."` thay vì thông báo thân thiện `"Ghế vừa được người khác đặt. Vui lòng chọn ghế khác."`.

---

## 2. Hướng dẫn Tự Kiểm thử bằng Apache JMeter

### Chuẩn bị Môi trường & Tài khoản
- Backend đang chạy tại: `http://localhost:8080` (hoặc cổng Docker tương ứng).
- Tài khoản khách hàng mẫu:
  - `kh1` / mật khẩu `Abc@123`
  - `kh2` / mật khẩu `Abc@123`

---

### Bước 1: Lấy JWT Token cho 2 User
Trước khi cấu hình bắn đồng thời, bạn cần lấy JWT Token cho mỗi tài khoản:
Gửi request đăng nhập bằng Postman / cURL hoặc HTTP Request trên JMeter:

```bash
# Đăng nhập cho kh1
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"kh1","password":"Abc@123"}'

# Đăng nhập cho kh2
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"kh2","password":"Abc@123"}'
```
Lưu lại `token` của `kh1` và `token` của `kh2`.

---

### Bước 2: Thiết lập Test Plan trong Apache JMeter

1. **Khởi động JMeter:** Chạy file `bin/jmeter.bat` (trên Windows).
2. **Tạo Test Plan mới:** Đặt tên `CoachBooking_Seat_Concurrency_Test`.

#### 2.1. Thêm Thread Group
- Chuột phải vào `Test Plan` -> **Add** -> **Threads (Users)** -> **Thread Group**.
- Cấu hình thông số:
  - **Number of Threads (users):** `2` (hoặc `5`, `10` nếu muốn test nhiều request hơn).
  - **Ramp-up period (seconds):** `0` (rất quan trọng: bắn đồng loạt, không giãn cách thời gian).
  - **Loop Count:** `1`.

#### 2.2. Thêm Synchronizing Timer (Điểm hẹn Rendezvous)
> [!IMPORTANT]
> **Synchronizing Timer** giúp giữ tất cả các thread lại cho đến khi đủ số lượng thread rồi mới thả ra cùng đúng 1 mili-giây, giả lập hoàn hảo trạng thái tranh chấp đồng thời (concurrency race condition).

- Chuột phải vào `Thread Group` -> **Add** -> **Timer** -> **Synchronizing Timer**.
- Cấu hình:
  - **Number of Simulated Users to Group by:** `2` (bằng với số Number of Threads).
  - **Timeout in milliseconds:** `5000`.

#### 2.3. Thêm CSV Data Set Config (hoặc cấu hình Token động)
Để 2 thread đại diện cho 2 người dùng khác nhau (`kh1` và `kh2`):
- Tạo 1 file text `users.csv` đặt cùng thư mục file JMeter:
  ```csv
  token,customerName,phone
  <TOKEN_CUA_KH1>,Khach Hang 1,0981111111
  <TOKEN_CUA_KH2>,Khach Hang 2,0981111112
  ```
- Chuột phải `Thread Group` -> **Add** -> **Config Element** -> **CSV Data Set Config**:
  - **Filename:** Đường dẫn tới file `users.csv`.
  - **Variable Names:** `token,customerName,phone`.
  - **Delimiter:** `,`.

*(Nếu muốn test đơn giản bằng 1 tài khoản nhân viên `admin` cùng đặt vé: bạn có thể bỏ qua CSV và dùng trực tiếp 1 token).*

#### 2.4. Thêm HTTP Header Manager
- Chuột phải `Thread Group` -> **Add** -> **Config Element** -> **HTTP Header Manager**.
- Thêm 2 headers:
  - `Content-Type`: `application/json`
  - `Authorization`: `Bearer ${token}` *(hoặc dán trực tiếp token nếu không dùng CSV)*

#### 2.5. Thêm HTTP Request (API Đặt vé)
- Chuột phải `Thread Group` -> **Add** -> **Sampler** -> **HTTP Request**.
- Cấu hình:
  - **Protocol:** `http`
  - **Server Name or IP:** `localhost`
  - **Port Number:** `8080`
  - **HTTP Method:** `POST`
  - **Path:** `/api/bookings`
  - **Body Data:** Chọn tab **Body Data** và dán JSON (chọn 1 ghế cụ thể, ví dụ ghế số `5` trên chuyến `tripId: 1`):
    ```json
    {
      "tripId": 1,
      "seatNumbers": [5],
      "customerName": "${customerName}",
      "phone": "${phone}",
      "paymentMethod": "Tiền mặt"
    }
    ```
    *(Lưu ý: Đảm bảo chuyến `tripId = 1` có thời gian khởi hành trong tương lai và ghế `5` chưa bị ai đặt trước đó).*

#### 2.6. Thêm Listeners để xem kết quả
- Chuột phải `Thread Group` -> **Add** -> **Listener** -> **View Results Tree** (xem chi tiết từng request).
- Chuột phải `Thread Group` -> **Add** -> **Listener** -> **Summary Report** (xem tỷ lệ thành công / thất bại).

---

### Bước 3: Chạy Test và Đánh giá Kết quả

1. Nhấn nút **Start (mũi tên xanh)** trên thanh công cụ JMeter.
2. Mở **View Results Tree**:
   - **Request 1:** Icon màu **Xanh lá cây**
     - Response code: `200`
     - Response data: Trả về thông tin hóa đơn đã tạo thành công (`invoiceId`, `totalAmount`, `ticketCount: 1`, ...).
   - **Request 2:** Icon màu **Đỏ**
     - Response code: `409 Conflict`
     - Response data: Thông báo lỗi do trùng ghế vi phạm constraint.

3. **Kiểm tra trực tiếp trong Database PostgreSQL:**
   Chạy truy vấn SQL:
   ```sql
   -- Kiểm tra số lượng vé được tạo cho chuyến 1, ghế 5:
   SELECT ticket_id, trip_id, invoice_id, seat_number, price 
   FROM tickets 
   WHERE trip_id = 1 AND seat_number = 5;
   ```
   **Kết quả:** Luôn luôn chỉ có **duy nhất 1 dòng bản ghi**! Tuyệt đối không thể có dòng thứ 2.

4. **Kiểm tra bảng Invoices:**
   ```sql
   -- Hóa đơn của request thất bại đã được tự động ROLLBACK hoàn toàn:
   SELECT * FROM invoices ORDER BY invoice_id DESC LIMIT 5;
   ```
   Không có hóa đơn "rác" nào của request thất bại còn sót lại trong cơ sở dữ liệu.

---

## 3. Khuyến nghị Tối ưu Mã nguồn

Để hệ thống hoàn thiện hơn khi chạy thực tế:
1. **Sửa tên Constraint trong `GlobalExceptionHandler.java`:**
   Cập nhật điều kiện kiểm tra chuỗi `tickets_trip_id_seat_number_key` để client nhận thông báo thân thiện:
   ```java
   if (detail != null && (detail.contains("tickets_trip_id_seat_number_key") || detail.contains("ve_xe_ma_chuyen_cho_ngoi_key"))) {
     message = "Ghế vừa được người khác đặt. Vui lòng chọn ghế khác.";
   }
   ```
2. **Frontend UX:** Khi nhận mã `409 Conflict`, giao diện React hiển thị thông báo ghế đã có người nhanh tay đặt trước và tự động tải lại sơ đồ ghế (`seatMap`) để người dùng chọn ghế khác.
