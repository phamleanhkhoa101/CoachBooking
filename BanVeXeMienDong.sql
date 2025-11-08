
-- CREATE DATABASE HeThongBanVeXeMienDong;
USE HeThongBanVeXeMienDong;

--
-- XÓA BẢNG
--

DROP TABLE IF EXISTS VeXe;
DROP TABLE IF EXISTS GiaVe;
DROP TABLE IF EXISTS ThanhToan;
DROP TABLE IF EXISTS ChuyenXe;
DROP TABLE IF EXISTS TaiXe;
DROP TABLE IF EXISTS TuyenDuong;
DROP TABLE IF EXISTS Xe;
DROP TABLE IF EXISTS LoaiXe;
DROP TABLE IF EXISTS NhaXe;
DROP TABLE IF EXISTS HoaDon;
DROP TABLE IF EXISTS PhanQuyen;
DROP TABLE IF EXISTS ChucNang;
DROP TABLE IF EXISTS KhachHang;
DROP TABLE IF EXISTS NhanVien;
DROP TABLE IF EXISTS TaiKhoan;
DROP TABLE IF EXISTS NhomQuyen;
--
-- TẠO BẢNG
--

-- 2.1 Bảng nhóm quyền
CREATE TABLE Quyen (
    MaQuyen INT IDENTITY(1,1) PRIMARY KEY,
    TenQuyen NVARCHAR(50) UNIQUE NOT NULL,
    MoTa NVARCHAR(255)
);



-- 2.4 Bảng tài khoản
CREATE TABLE TaiKhoan (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
    MatKhau VARCHAR(100) NOT NULL,
	Email VARCHAR(100) NOT NULL UNIQUE,
    MaQuyen INT NOT NULL,
    TrangThai BIT DEFAULT 1,
    FOREIGN KEY (MaQuyen) REFERENCES Quyen(MaQuyen)
);

--1. Bảng NhanVien
CREATE TABLE NhanVien(
	MaNV INT IDENTITY(1,1) PRIMARY KEY,
	TenNV NVARCHAR(100) NOT NULL,
	NamSinh DATE NOT NULL CHECK(DATEDIFF(YEAR, NamSinh, GETDATE()) >=18),
	DiaChi NVARCHAR(50) NOT NULL,
	SDT VARCHAR(11) NOT NULL UNIQUE,
	GioiTinh BIT DEFAULT 0,
	TrangThai BIT DEFAULT 1,
	MaTK INT,
	FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);

--2. Bảng KhachHang
CREATE TABLE KhachHang(
	MaKH INT IDENTITY(1,1) PRIMARY KEY,
	TenKH NVARCHAR(100) NOT NULL,
	NamSinh DATE NOT NULL CHECK(NamSinh < GETDATE()),
	SDT CHAR(10) NOT NULL UNIQUE,
	GioiTinh BIT DEFAULT 0,
	MaTK INT,
	FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);


--3. Bảng NhaXe
CREATE TABLE NhaXe(
	MaNX INT IDENTITY(1,1) PRIMARY KEY,
	TenNX NVARCHAR(100) NOT NULL,
	SDT CHAR(11) NOT NULL UNIQUE,
	Email CHAR(50) NOT NULL UNIQUE,
	TrangThai BIT DEFAULT 1
);

--4. Bảng LoaiXe
CREATE TABLE LoaiXe(
	MaLX INT IDENTITY(1,1) PRIMARY KEY,
	TenLX NVARCHAR(100) NOT NULL,
	SoCho INT NOT NULL CHECK(SoCho>0),
	TrangThai BIT DEFAULT 1
);

--5. Bảng Xe
CREATE TABLE Xe(
	MaXe INT IDENTITY(1,1) PRIMARY KEY,
	MaNX INT,
	MaLX INT NOT NULL,
	BienSoXe CHAR(20) NOT NULL UNIQUE,
	TrangThai BIT DEFAULT 1,
	FOREIGN KEY (MaNX) REFERENCES NhaXe(MaNX),
	FOREIGN KEY (MaLX) REFERENCES LoaiXe(MaLX)
);

--6. Bảng TuyenDuong
CREATE TABLE TuyenDuong(
	MaTuyen INT IDENTITY(1,1) PRIMARY KEY,
	DiemDi NVARCHAR(255) NOT NULL,
	DiemDen NVARCHAR(255) NOT NULL,
	QuangDuong INT,
	ThoiGianDuKien TIME,
	TrangThai BIT DEFAULT 1
)

--12 tai xe
CREATE TABLE TaiXe(
	MaTX INT IDENTITY(1,1) PRIMARY KEY,
	TenTX NVARCHAR(100),
	NamSinh DATE NOT NULL CHECK (DATEDIFF(YEAR, NamSinh, GETDATE()) > 18),
	CCCD CHAR(12) NOT NULL UNIQUE,
	GioiTinh BIT DEFAULT 0,
	SDT CHAR(10) NOT NULL UNIQUE,
	DiaChi NVARCHAR(100) NOT NULL,
	BangLai CHAR(5) NOT NULL,
	NgayCap DATE NOT NULL,
	TrangThai BIT DEFAULT 1
);

--10. Bảng GiaVe
CREATE TABLE GiaVe(
	MaGV INT IDENTITY(1,1) PRIMARY KEY,
	MaNX INT NOT NULL,
	MaLX INT NOT NULL,
	MaTuyen INT NOT NULL,
    GiaVe DECIMAL(10,0) NOT NULL,
	UNIQUE (MaNX, MaLX, MaTuyen),
    FOREIGN KEY (MaNX) REFERENCES NhaXe(MaNX),
    FOREIGN KEY (MaLX) REFERENCES LoaiXe(MaLX),
    FOREIGN KEY (MaTuyen) REFERENCES TuyenDuong(MaTuyen)
); 

--7. Bảng ChuyenXe
CREATE TABLE ChuyenXe(
	MaChuyen INT IDENTITY(1,1) PRIMARY KEY,
	MaXe INT,
	MaTuyen INT,
	MaTX INT,
	NgayGioKhoiHanh DATETIME NOT NULL,
	NgayGioDenDuKien DATETIME,
	GiaVeThucTe DECIMAL(10,2),
	FOREIGN KEY (MaXe) REFERENCES Xe(MaXe),
	FOREIGN KEY (MaTuyen) REFERENCES TuyenDuong(MaTuyen),
	FOREIGN KEY (MaTX) REFERENCES TaiXe(MaTX),
	UNIQUE (MaXe, NgayGioKhoiHanh, MaTX)
);

--8. Bảng HoaDon
CREATE TABLE HoaDon(
	MaHD INT IDENTITY(1,1) PRIMARY KEY,
	MaKH INT,
	MANV INT,
	NgayLap DATETIME NOT NULL,
	TongTien DECIMAL(10,0) DEFAULT 0,
	TrangThai BIT DEFAULT 0,
	FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
	FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

--9. Bảng ThanhToan
CREATE TABLE ThanhToan(
	MaTT INT IDENTITY(1,1) PRIMARY KEY ,
	MaHD INT UNIQUE,
	TongTien DECIMAL(10,0) DEFAULT 0,
	NgayTT DATETIME NOT NULL,
	PhuongThuc NVARCHAR(50) NOT NULL,
	FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD)
);

--11. Bảng VeXe
CREATE TABLE VeXe(
	MaVe INT IDENTITY(1,1) PRIMARY KEY ,
	MaChuyen INT,
	MaHD INT,
	ChoNgoi INT NOT NULL,
	GiaVe DECIMAL(10,0) DEFAULT 0,
	FOREIGN KEY (MaChuyen) REFERENCES ChuyenXe(MaChuyen),
	FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD),
	UNIQUE (MaChuyen, ChoNgoi),
);

--
-- Trigger
--

-- 1 Cập nhật trạng thái hóa đơn khi thêm thanh toán
GO
CREATE TRIGGER trg_UpdateHoaDon_AfterThanhToanInsert 
ON ThanhToan
AFTER INSERT AS
BEGIN
	--1. Cập nhật TrangThai HoaDon
	UPDATE HoaDon
	SET TrangThai=1
	FROM HoaDon hd
	INNER JOIN inserted i ON hd.MaHD = i.MaHD;

	--2. Cập nhật TongTien cua ThanhToan
	UPDATE tt
	SET tt.TongTien = hd.TongTien
	FROM ThanhToan tt 
	INNER JOIN inserted i ON i.MaTT= tt.MaTT
	INNER JOIN HoaDon hd ON hd.MaHD = tt.MaHD;
End;


--2. Cập nhật trạng thái hóa đơn khi xóa thanh toán
GO
CREATE TRIGGER trg_UpdateHoaDon_AfterThanhToanDelete
ON ThanhToan
AFTER DELETE AS
BEGIN
	UPDATE HoaDon
	SET TrangThai =0
	FROM HoaDon hd
	INNER JOIN deleted i ON i.MaHD= hd.MaHD;
END;



--3. Tính giá vé khi thêm vé xe
GO
CREATE TRIGGER trg_UPDATEGiaVe
ON VeXe
AFTER INSERT, UPDATE AS
BEGIN
	SET NOCOUNT ON;
	UPDATE vx 
	SET vx.GiaVe = gv.GiaVe
	FROM VeXe vx
	INNER JOIN inserted i ON i.MaVe = vx.MaVe
	INNER JOIN ChuyenXe cx ON cx.MaChuyen = vx.MaChuyen
	INNER JOIN Xe x ON x.MaXe = cx.MaXe
	INNER JOIN GiaVe gv ON gv.MaNX = x.MaNX
		AND gv.MaLX = x.MaLX
		AND gv.MaTuyen = cx.MaTuyen;
END;

-- 4.Tính tổng tiền hóa đơn khi thêm vé
GO
CREATE TRIGGER trg_TinhGiaVeVaTongTienHoaDon_ThemSua
ON VeXe
AFTER INSERT, UPDATE AS
BEGIN
	SET NOCOUNT ON; 

	UPDATE hd
	SET hd.TongTien = sub.TongTien
	FROM HoaDon	hd
	INNER JOIN (
		SELECT vx.MaHD, SUM(vx.GiaVe) AS TongTien
		FROM VeXe vx
		WHERE vx.MaHD IN (SELECT DISTINCT MaHD FROM inserted)
		GROUP BY vx.MaHD
	) sub ON sub.MaHD = hd.MaHD;
END;

--5. tính tổng tiền hóa đơn khi xóa vé
GO
CREATE TRIGGER trg_TinhTongTienHoaDon_XoaVe
ON VeXe
AFTER DELETE AS
BEGIN
	SET NOCOUNT ON;

	UPDATE hd
	SET hd.TongTien= ISNULL(sub.TongTien,0)
	FROM HoaDon hd
	INNER JOIN (
		SELECT d.MaHD, SUM(vx.GiaVe) AS TongTien
		FROM deleted d
		LEFT JOIN VeXe vx ON vx.MaHD = d.MaHD
		GROUP BY d.MaHD
	) sub ON hd.MaHD= sub.MaHD;
END;

--6. Kiểm tra ngày đặt vé hợp lệ
--drop trigger trg_KiemTraNgayDatVe
--GO
--CREATE TRIGGER trg_KiemTraNgayDatVe
--ON VeXE
--AFTER INSERT AS
--BEGIN
--	SET NOCOUNT ON
--	IF EXISTS (
--		SELECT 1
--		FROM VeXe vx
--		JOIN HoaDon hd ON hd.MaHD= vx.MaHD
--		JOIN ChuyenXe cx ON cx.MaChuyen= vx.MaChuyen
--		WHERE hd.NgayLap > cx.NgayGioKhoiHanh -- ngay lap hd phai nho hon ngaygiokhoihanh cua chuyen xe
--	)
--	BEGIN
--		RAISERROR (N'Ngày lập hóa đơn phải nhỏ hơn ngày giờ khởi hành của chuyến xe',16,1)
--		ROLLBACK TRANSACTION
--		RETURN;
--	END
--END;
--7. TÍNH NGAYGIODENDUKIEN
GO
CREATE TRIGGER trg_TinhNgayGioDenDuKien
ON ChuyenXe
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cx
    SET cx.NgayGioDenDuKien = DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', td.ThoiGianDuKien), cx.NgayGioKhoiHanh)
    FROM ChuyenXe cx
    INNER JOIN inserted i ON i.MaChuyen = cx.MaChuyen
    INNER JOIN TuyenDuong td ON td.MaTuyen = cx.MaTuyen;
END;
--
-- Thêm dữ liệu
--
GO

INSERT INTO Quyen (TenQuyen, MoTa) VALUES
(N'Admin', N'Toàn quyền quản trị hệ thống'),
(N'NhanVien', N'Nhân viên bán vé'),
(N'KhachHang', N'Khách hàng sử dụng hệ thống');

INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Email, MaQuyen) VALUES
('admin', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','1@gmail.com', 1),
('nv1', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','2@gmail.com', 2),
('nv2', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','3@gmail.com', 2),
('kh1', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','4@gmail.com', 3),
('kh2', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=','5@gmail.com', 3);
-- 1. NhanVien
INSERT INTO NhanVien (TenNV, NamSinh, DiaChi, SDT, MaTK) VALUES
(N'Nguyễn Văn An', '1980-01-15', N'TP Hồ Chí Minh', '0912345678', 1),
(N'Trần Thị Bình', '1982-03-20', N'Long An', '0912345679', 2),
(N'Lê Văn Cường', '1978-07-10', N'Bình Định', '0912345680', 3),
(N'Nguyễn Thị Dung', '1985-05-05', N'Cần Thơ', '0912345681', null),
(N'Hoàng Văn Em', '1983-11-25', N'An Giang', '0912345682', null);

-- 2. KhachHang
INSERT INTO KhachHang (TenKH, NamSinh, SDT) VALUES
(N'Lê Văn Trung', '1990-02-15', '0981111111'),
(N'Phạm Thị Hoa', '1988-04-20', '0981111112'),
(N'Nguyễn Văn Nam', '1992-06-25', '0981111113'),
(N'Trần Thị Linh', '1995-03-30', '0981111114'),
(N'Vũ Văn Minh', '1987-09-10', '0981111115'),
(N'Đỗ Thị Mỹ', '1991-12-05', '0981111116'),
(N'Nguyễn Thị Nga', '1993-07-07', '0981111117'),
(N'Lê Văn Phong', '1989-08-08', '0981111118'),
(N'Hoàng Thị Quyên', '1990-10-10', '0981111119'),
(N'Trần Văn Sơn', '1985-11-11', '0981111120'),
(N'Phạm Thị Uyên', '1992-01-01', '0981111121'),
(N'Nguyễn Văn Vinh', '1988-02-02', '0981111122'),
(N'Lê Thị Xuân', '1994-03-03', '0981111123'),
(N'Đỗ Văn Ý', '1996-04-04', '0981111124'),
(N'Trần Thị Diễm', '1993-05-05', '0981111125'),
(N'Vũ Văn Anh', '1991-06-06', '0981111126'),
(N'Nguyễn Thị Bảo', '1990-07-07', '0981111127'),
(N'Lê Văn Cẩn', '1987-08-08', '0981111128'),
(N'Phạm Thị Đào', '1994-09-09', '0981111129'),
(N'Trần Văn Lợi', '1992-10-10', '0981111130');
UPDATE KhachHang SET MaTK = 1 WHERE MaKH = 1; 
UPDATE KhachHang SET MaTK = 1 WHERE MaKH = 2; 
-- 3. NhaXe
INSERT INTO NhaXe (TenNX, SDT, Email, TrangThai) VALUES
(N'Nhà xe Phương Trang', '19006067', 'info@futabus.vn', 1),
(N'Nhà xe Thành Bưởi', '19006079', 'cskh@thanhbuoi.com.vn', 1),
(N'Nhà xe Hoàng Long', '1900988920', 'info@hoanglongasia.com', 1),
(N'Nhà xe Mai Linh', '1055', 'cskh@mailinh.vn', 1),
(N'Nhà xe Kumho Samco', '02835114648', 'lienhe@kumhosamco.com.vn', 1);

-- 4. LoaiXe
INSERT INTO LoaiXe (TenLX, SoCho) VALUES
(N'Ghế ngồi', 40), (N'Giường nằm',45), (N'Limousine',40);


-- 5. Xe
INSERT INTO Xe (MaNX, MaLX, BienSoXe ) VALUES
(1,1,'29A-12345'),
(2,1,'29B-45634'),
(3,2,'30A-34567'),
(4,1,'30B-45678'),
(5,3,'31A-56789'),
(1,2,'31B-67890'),
(2,2,'32A-78901'),
(3,3,'32B-89012'),
(4,2,'33A-90123');


-- 6. TuyenDuong
INSERT INTO TuyenDuong (DiemDi, DiemDen, QuangDuong, ThoiGianDuKien) VALUES
(N'Bến xe Miền Đông', N'Vũng Tàu', 95, '02:00:00'),
(N'Bến xe Miền Đông', N'Đà Lạt', 300, '07:00:00'),
(N'Bến xe Miền Đông', N'Nha Trang', 430, '09:00:00'),
(N'Bến xe Miền Đông', N'Phan Thiết', 200, '04:00:00'),
(N'Bến xe Miền Đông', N'Quy Nhơn', 650, '12:00:00'),
(N'Bến xe Miền Đông', N'Đà Nẵng', 960, '16:00:00'),
(N'Bến xe Miền Đông', N'Buôn Ma Thuột', 330, '07:30:00');


--7. TAIXE
INSERT INTO TaiXe (TenTX, NamSinh, CCCD, SDT, DiaChi, BangLai, NgayCap) VALUES
(N'Nguyễn Văn Hậu', '1985-04-15', '123456789012', '0909123456', N'TP.HCM', 'D', '2010-05-20'),
(N'Lê Văn Sơn', '1982-02-20', '123456789013', '0909234567', N'TP.HCM', 'E', '2009-06-10'),
(N'Phạm Minh Tâm', '1988-08-08', '123456789014', '0909345678', N'TP.HCM', 'D', '2011-09-12'),
(N'Trần Hữu Phát', '1990-05-10', '123456789015', '0909456789', N'TP.HCM', 'E', '2012-12-01');

-- 8. GiaVe 
INSERT INTO GiaVe (MaNX, MaLX, MaTuyen, GiaVe) VALUES
(1,1,1,150000),
(2,1,2,120000),
(3,2,3,200000),
(4,1,4,130000),
(5,3,5,250000),
(1,1,2,150000),
(1,2,5,200000);


-- 7. ChuyenXe
INSERT INTO ChuyenXe (MaXe, MaTuyen, MaTX, NgayGioKhoiHanh) VALUES
(1,1,1,'2025-04-22 12:00:00'),
(2,2,2,'2025-04-22 12:00:00'),
(3,3,3,'2025-04-22 12:00:00'),
(4,4,4,'2025-04-22 12:30:00'),
(5,5,1,'2025-04-23 12:00:00'),
(6,5,2,'2025-04-23 12:00:00'),
(1,2,3,'2025-04-23 12:00:00');

-- 9. HoaDon 
INSERT INTO HoaDon (MaKH, MaNV, NgayLap) VALUES
(1,1,'2025-04-20 06:30:00'),
(2,2,'2025-04-21 06:00:00'),
(3,3,'2025-04-21 06:30:00'),
(4,4,'2025-04-22 06:00:00'),
(5,5,'2025-04-22 06:30:00'),
(6,1,'2025-04-21 06:00:00'),
(7,2,'2025-04-22 06:00:00'),
(8,3,'2025-04-22 06:00:00'),
(9,1,'2025-04-22 06:00:00'),
(10,2,'2025-04-23 06:00:00'),
(11,3,'2025-04-23 06:00:00'),
(12,4,'2025-04-23 06:00:00'),
(13,5,'2025-04-23 06:00:00');


-- 10. VeXe 
INSERT INTO VeXe (MaChuyen, MaHD, ChoNgoi) VALUES
(1, 1, 1),
(1, 1, 2),
(2, 2, 1),
(3, 3, 1),
(4, 4, 5),
(5, 5, 10),
(5, 5, 11),
(3, 6, 3),
(6, 7, 2),
(4, 8, 6),
(4, 8, 7),
(5, 9, 7),
(6, 9, 10),
(6, 10, 3),
(7, 11, 4),
(7, 12, 8),
(7, 13, 6);


-- 11. ThanhToan 
INSERT INTO ThanhToan (MaHD, NgayTT, PhuongThuc) VALUES
(1, '2025-04-20 07:30:00', N'Tiền mặt'),
(2, '2025-04-20 08:00:00', N'Thẻ ATM'),
(3, '2025-04-20 08:30:00', N'Tiền mặt'),
(4, '2025-04-21 09:30:00', N'Tiền mặt'),
(5, '2025-04-21 09:30:00', N'Thẻ ATM'),
(6, '2025-04-22 10:00:00', N'Tiền mặt'),
(7, '2025-04-22 10:30:00', N'Tiền mặt'),
(8, '2025-04-22 11:00:00', N'Tiền mặt'),
(9, '2025-04-23 07:30:00', N'Tiền mặt'),
(10, '2025-04-23 08:00:00', N'Thẻ ATM'),
(11, '2025-04-23 09:30:00', N'Tiền mặt'),
(12, '2025-04-23 11:00:00', N'Tiền mặt');



