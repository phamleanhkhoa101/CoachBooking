# Mô Tả Tính Năng Hệ Thống QLBXMD

## 1. Tổng quan

`QLBXMD` là ứng dụng desktop `WinForms` trên `.NET Framework 4.8` dùng để quản lý và bán vé xe tại bến xe miền Đông. Hệ thống được tổ chức theo các lớp `GUI -> BUS -> DAO -> EF`, có tích hợp thanh toán `MoMo`, gửi email qua `SMTP`, và quản lý dữ liệu bằng `Entity Framework 6`.

Mục tiêu chính của hệ thống là:

- Quản lý danh mục vận hành như nhà xe, xe, loại xe, tuyến đường, chuyến xe, tài xế, giá vé.
- Hỗ trợ bán vé tại quầy cho nhân viên.
- Cho phép khách hàng tự tìm chuyến và đặt vé.
- Theo dõi hóa đơn, vé xe, lịch sử mua vé và thống kê doanh thu.

## 2. Cấu trúc chức năng theo thư mục

### `GUI/DN_DK_QMK`

Nhóm màn hình xác thực đầu vào:

- Đăng nhập.
- Đăng ký tài khoản khách hàng.
- Quên mật khẩu và gửi mật khẩu mới qua email.

### `GUI/Admin`

Nhóm màn hình dành cho `Admin` và `NhânVien`:

- Trang chủ tổng quan.
- Bán vé tại quầy.
- Quản lý hóa đơn.
- Quản lý khách hàng.
- Quản lý nhân viên.
- Quản lý tài khoản.
- Quản lý nhà xe, loại xe, xe, tuyến đường, chuyến xe, giá vé, tài xế.
- Thống kê hoạt động kinh doanh.
- Cập nhật thông tin cá nhân.

### `GUI/User`

Nhóm màn hình dành cho `KhachHang`:

- Tìm chuyến và đặt vé.
- Xem lịch sử mua vé.
- Xem chi tiết hóa đơn/vé.
- Thanh toán hóa đơn chưa thanh toán.
- Hủy vé chưa thanh toán.
- Cập nhật hồ sơ cá nhân và đổi mật khẩu.

### `BUS`, `DAO`, `EF`, `Services`, `Utils`

- `BUS`: chứa kiểm tra nghiệp vụ và validate trước khi thao tác dữ liệu.
- `DAO`: truy xuất dữ liệu.
- `EF`: mô hình dữ liệu `Entity Framework`.
- `Services`: dịch vụ ngoài hệ thống, hiện có `MoMoPaymentService`.
- `Utils`: hàm băm mật khẩu, validate dữ liệu, gửi email.

## 3. Vai trò người dùng

## `Admin`

- Có toàn quyền truy cập các màn hình quản trị.
- Quản lý đầy đủ danh mục, tài khoản, nhân sự, khách hàng, hóa đơn, thống kê.

## `NhânVien`

Theo hiện trạng code, nhân viên đăng nhập chung vào giao diện admin nhưng bị giới hạn quyền. Nhân viên chỉ được dùng:

- `Trang chủ`
- `Đặt vé`
- `Hóa đơn`
- `Khách hàng`

Các màn hình quản trị sâu như danh mục hệ thống, tài khoản, thống kê, phân quyền không được mở cho nhân viên.

## `KhachHang`

- Tự đăng ký tài khoản.
- Đăng nhập giao diện người dùng.
- Tìm chuyến, chọn ghế, đặt vé, thanh toán, xem lịch sử và cập nhật thông tin cá nhân.

## 4. Các chức năng nghiệp vụ chính

### 4.1. Xác thực và tài khoản

- Đăng nhập bằng tên đăng nhập và mật khẩu đã băm `SHA-256`.
- Điều hướng giao diện theo quyền sau khi đăng nhập.
- Đăng ký tài khoản khách hàng mới.
- Kiểm tra định dạng email, độ dài tên đăng nhập, độ mạnh mật khẩu.
- Quên mật khẩu: sinh mật khẩu mới ngẫu nhiên, cập nhật vào database và gửi qua email.
- Quản trị có thể tạo, sửa, xóa, khóa/mở khóa tài khoản.
- Quản trị có thể đặt lại mật khẩu cho tài khoản.
- Hệ thống không cho xóa tài khoản nếu đang liên kết với nhân viên hoặc khách hàng.

### 4.2. Bán vé tại quầy cho nhân viên/admin

Màn hình `UCDatVe` hỗ trợ quy trình bán vé tại quầy:

- Chọn điểm đi, điểm đến, ngày đi, khung giờ.
- Tìm danh sách chuyến xe phù hợp.
- Hiển thị loại xe, giờ khởi hành, giờ đến dự kiến, nhà xe, số chỗ, giá vé.
- Chọn ghế theo sơ đồ ghế.
- Tự nhận biết xe ghế ngồi và xe giường nằm để vẽ sơ đồ ghế 1 tầng hoặc 2 tầng.
- Tìm khách hàng theo số điện thoại.
- Nếu chưa có khách hàng, cho phép tạo nhanh khách hàng mới tại thời điểm bán vé.
- Xác nhận đặt vé và chọn phương thức thanh toán.
- Hỗ trợ thanh toán `Tiền mặt` hoặc `MoMo`.
- Tạo `HóaDon`, `VeXe`, `ThanhToan` và gắn `MANV` là nhân viên đang thao tác.

### 4.3. Khách hàng tự đặt vé

Màn hình `UCDatVeUser` hỗ trợ khách hàng tự mua vé:

- Tự động tải danh sách điểm đi, điểm đến.
- Tự động lọc chuyến theo ngày và khung giờ.
- Hiển thị danh sách chuyến bằng thẻ thông tin.
- Chọn ghế trực quan trên sơ đồ ghế.
- Tính tổng tiền theo số ghế đã chọn.
- Hiển thị hộp thoại xác nhận trước khi tạo đơn.
- Tạo hóa đơn và vé trước, sau đó xử lý thanh toán `MoMo`.
- Có cơ chế polling để kiểm tra trạng thái giao dịch `MoMo`.
- Nếu giao dịch lỗi hoặc hủy, hệ thống rollback hóa đơn/vé đã tạo.

### 4.4. Quản lý lịch sử mua vé của khách hàng

Màn hình `UCLichSuMuaVe` cho phép:

- Xem danh sách hóa đơn đã từng tạo.
- Hiển thị trạng thái thanh toán.
- Hiển thị tuyến đường, nhà xe, ngày đi, ghế đã đặt, tổng tiền.
- Mở chi tiết từng hóa đơn để xử lý thêm.

### 4.5. Xem chi tiết hóa đơn của khách hàng

Màn hình `XemChiTietDialog` hỗ trợ:

- Xem chi tiết chuyến đi, loại xe, ghế, số lượng vé, tổng tiền.
- Nếu hóa đơn chưa thanh toán:
  - thanh toán lại bằng `Tiền mặt` hoặc `MoMo`
  - hủy vé
- Nếu hóa đơn đã thanh toán:
  - chỉ xem thông tin, không cho hủy

### 4.6. Quản lý thông tin cá nhân

#### Cho khách hàng

- Cập nhật tên, số điện thoại, ngày sinh, giới tính, email.
- Nếu tài khoản chưa gắn bản ghi khách hàng, hệ thống tự tạo hồ sơ khách hàng mặc định.
- Đổi mật khẩu.

#### Cho admin/nhân viên

- Xem và sửa thông tin nhân viên của chính mình.
- Đổi mật khẩu.
- Đổi email tài khoản.

### 4.7. Quản lý khách hàng

Màn hình `UCKhachHang` cho phép:

- Xem danh sách khách hàng.
- Tìm kiếm và lọc theo nhiều tiêu chí.
- Thêm, xem, sửa, xóa khách hàng.
- Liên kết khách hàng với tài khoản có sẵn.
- Cấp tài khoản mới cho khách hàng chưa có tài khoản.

### 4.8. Quản lý nhân viên

Màn hình `UCNhanVien` cho phép:

- Xem danh sách nhân viên.
- Tìm kiếm và lọc.
- Thêm, xem, sửa, xóa nhân viên.
- Gắn tài khoản có sẵn cho nhân viên.
- Cấp tài khoản mới cho nhân viên chưa có tài khoản.

### 4.9. Quản lý tài khoản

Màn hình `UCTaiKhoan` cho phép:

- Quản lý danh sách tài khoản hệ thống.
- Hiển thị tài khoản đang liên kết với nhân viên hay khách hàng.
- Thêm tài khoản mới với quyền và trạng thái.
- Xem/sửa thông tin tài khoản.
- Xóa tài khoản nếu không bị ràng buộc.
- Đổi hoặc đặt lại mật khẩu.

### 4.10. Quản lý hóa đơn

Màn hình `UCHoaDon` cho phép:

- Xem danh sách hóa đơn.
- Tìm kiếm, lọc theo mã, khách hàng, nhân viên, ngày lập, trạng thái.
- Xem các vé thuộc từng hóa đơn.
- Thêm, xem, sửa, xóa hóa đơn.

### 4.11. Quản lý danh mục vận hành

Nhóm màn hình danh mục trong `GUI/Admin` cho phép quản lý các đối tượng cốt lõi:

- `UCNhaXe`: quản lý nhà xe.
- `UCLoaiXe`: quản lý loại xe và số chỗ.
- `UCXe`: quản lý xe, biển số, loại xe, nhà xe.
- `UCTuyenDuong`: quản lý điểm đi, điểm đến, quãng đường, thời gian dự kiến.
- `UCChuyenXe`: quản lý lịch chạy và chuyến xe thực tế.
- `UCGiaVe`: quản lý bảng giá theo nhà xe, loại xe, tuyến đường.
- `UCTaiXe`: quản lý tài xế.

Các màn hình này đều đi theo hướng CRUD và có lớp `BUS` kiểm tra ràng buộc dữ liệu trước khi ghi xuống database.

### 4.12. Trang chủ và thống kê

#### Trang chủ `UCTrangChu`

- Hiển thị nhanh số khách hàng, số chuyến xe, tổng doanh thu, số vé bán.
- Có bộ lọc ngày từ - đến.
- Liệt kê danh sách vé đã bán trong khoảng thời gian chọn.

#### Trang thống kê `UCThongKe`

- Thống kê tổng doanh thu.
- Thống kê số chuyến xe.
- Thống kê số khách hàng mua vé.
- Thống kê số vé đã bán.
- Biểu đồ doanh thu theo ngày.
- Biểu đồ top 5 tuyến đường phổ biến.
- Biểu đồ doanh thu theo nhà xe.

## 5. Ràng buộc nghiệp vụ và tự động hóa trong database

Script SQL khởi tạo hệ thống đã cài nhiều ràng buộc và trigger để hỗ trợ nghiệp vụ:

- Mỗi tài khoản có email và tên đăng nhập duy nhất.
- Số điện thoại của khách hàng, nhân viên, tài xế là duy nhất theo từng bảng.
- Biển số xe, CCCD, bằng lái được kiểm soát theo ràng buộc dữ liệu.
- Một ghế trên cùng một chuyến chỉ được bán một lần.
- Trigger tự tính `NgayGioDenDuKien` của chuyến xe từ thời gian dự kiến của tuyến đường.
- Trigger tự gán `GiaVeThucTe` cho chuyến xe từ bảng giá.
- Trigger tự gán `GiaVe` cho vé xe từ giá chuyến.
- Trigger tự cập nhật `TongTien` của hóa đơn khi thêm/xóa vé.
- Trigger tự cập nhật trạng thái hóa đơn khi đã có thanh toán.

## 6. Tích hợp ngoài hệ thống

### MoMo

- Có service tạo yêu cầu thanh toán `MoMo` chuẩn sandbox.
- Có logic kiểm tra chữ ký, gọi API tạo giao dịch và query trạng thái giao dịch.
- Phù hợp với luồng thanh toán thử nghiệm trong môi trường test.

### Email

- Hệ thống dùng `SMTP Gmail` để:
  - gửi mật khẩu mới khi quên mật khẩu
  - hỗ trợ gửi thông tin tài khoản khi cấp tài khoản mới

## 7. Hiện trạng triển khai cần lưu ý

Tài liệu này mô tả theo hiện trạng code hiện có, và có một vài điểm đáng chú ý:

- `NhânVien` không có giao diện riêng, mà dùng chung `FrmMainAdmin` với giới hạn quyền.
- `UCThanhToan` có tồn tại trong code nhưng nút mở module này trên menu admin hiện đang chưa được nối hoàn chỉnh.
- `MoMoPaymentService` đang dùng endpoint sandbox và `ReturnUrl/NotifyUrl` dạng `localhost`.
- Thư mục `API` đã được tạo trong project nhưng hiện chưa có triển khai backend/web API hoàn chỉnh cho callback thanh toán.
- Repo có thêm các tài liệu hướng dẫn test/thống kê trong `QLBXMD.Tests` và `Docs`.

## 8. Kết luận

Hệ thống `QLBXMD` hiện là một ứng dụng bán vé xe khá đầy đủ cho mô hình bến xe:

- Có đủ luồng cho khách hàng tự mua vé.
- Có luồng bán vé tại quầy cho nhân viên.
- Có lớp quản trị dữ liệu vận hành.
- Có theo dõi hóa đơn, vé và thống kê kinh doanh.
- Đã bắt đầu tích hợp thanh toán điện tử và email để hỗ trợ vận hành thực tế.

Nếu cần mở rộng, các hướng phát triển tự nhiên nhất là:

- hoàn thiện callback thanh toán online
- xuất báo cáo/in hóa đơn
- phân quyền chi tiết hơn theo chức năng
- bổ sung API/web hoặc mobile cho khách hàng
