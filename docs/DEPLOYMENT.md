# Triển khai và cập nhật CoachBooking trên VPS

Tài liệu này áp dụng cho môi trường demo hiện tại:

- Source: `/opt/coachbooking` (clone nhánh `main` từ GitHub).
- Cấu hình chạy production: `/opt/coachbooking-deploy`.
- Docker Compose production: `/opt/coachbooking-deploy/docker-compose.yml`.
- Biến môi trường bí mật: `/opt/coachbooking-deploy/.env`.
- Website: `https://coachbooking.io.vn`.
- Cổng quản trị: `https://admin.coachbooking.io.vn`.

## Cập nhật khi có code mới trên `main`

1. SSH vào VPS.

   ```bash
   ssh root@<IP_VPS>
   ```

2. Lấy source mới nhất. Lệnh `--ff-only` dừng lại thay vì tự tạo merge commit nếu source trên VPS có thay đổi cục bộ.

   ```bash
   cd /opt/coachbooking
   git pull --ff-only origin main
   ```

3. Build image mới và cập nhật container.

   ```bash
   cd /opt/coachbooking-deploy
   docker compose up -d --build
   ```

4. Kiểm tra trạng thái sau deploy.

   ```bash
   docker compose ps
   docker compose logs --tail=100 backend
   ```

Backend tự chạy Flyway khi khởi động. Nếu bản cập nhật chứa migration mới, migration sẽ được áp dụng trước khi backend sẵn sàng phục vụ.

## Theo dõi và xử lý sự cố cơ bản

Xem log liên tục:

```bash
cd /opt/coachbooking-deploy
docker compose logs -f backend
```

Các container và log tương ứng:

```bash
docker compose logs --tail=100 caddy
docker compose logs --tail=100 frontend
docker compose logs --tail=100 db
```

Khởi động lại toàn bộ dịch vụ, không build lại image:

```bash
docker compose restart
```

Kiểm tra mức dùng tài nguyên VPS:

```bash
free -h
df -h
docker stats --no-stream
```

## Sao lưu database

Tạo backup PostgreSQL trước khi cập nhật quan trọng hoặc trước khi demo:

```bash
cd /opt/coachbooking-deploy
mkdir -p /root/coachbooking-backups
docker compose exec -T db pg_dump -U coachbooking coachbooking > /root/coachbooking-backups/coachbooking-$(date +%F-%H%M).sql
```

Kiểm tra file backup:

```bash
ls -lh /root/coachbooking-backups
```

Nên tải backup về máy cá nhân hoặc một nơi lưu trữ khác; backup chỉ nằm trên cùng VPS không bảo vệ được khi VPS gặp sự cố.

## Lệnh không được dùng trên VPS demo

Không chạy lệnh sau trừ khi đã chủ động sao lưu và muốn xóa toàn bộ dữ liệu:

```bash
docker compose down -v
```

Tùy chọn `-v` xóa Docker volume chứa PostgreSQL, dẫn đến mất dữ liệu đặt vé, hóa đơn và dữ liệu đã nhập.

Không đưa file `/opt/coachbooking-deploy/.env` lên GitHub hoặc gửi file này cho người khác. File chứa mật khẩu PostgreSQL và khóa ký JWT.

## Mô hình cổng mạng

Chỉ Caddy công khai cổng `80` và `443`. PostgreSQL và Spring Boot chỉ giao tiếp trong mạng Docker nội bộ. Caddy tự cấp và gia hạn chứng chỉ HTTPS cho hai domain đã cấu hình.
