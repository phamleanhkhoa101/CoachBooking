USE master;
ALTER DATABASE HeThongBanVeXeMienDong SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE HeThongBanVeXeMienDong;

CREATE DATABASE HeThongBanVeXeMienDong;
GO
USE HeThongBanVeXeMienDong;
GO
-- 1. Quyền.
CREATE TABLE Quyen (
    MaQuyen INT IDENTITY(1,1) PRIMARY KEY,
    TenQuyen NVARCHAR(50) UNIQUE NOT NULL,
    MoTa NVARCHAR(255)
);

-- 2. Tài khoản
CREATE TABLE TaiKhoan (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap VARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    MaQuyen INT NOT NULL,
    TrangThai BIT DEFAULT 1,
    FOREIGN KEY (MaQuyen) REFERENCES Quyen(MaQuyen)
);

-- 3. Nhân viên
CREATE TABLE NhanVien (
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    TenNV NVARCHAR(100) NOT NULL,
    NamSinh DATE NOT NULL CHECK (DATEDIFF(YEAR, NamSinh, GETDATE()) >= 18),
    DiaChi NVARCHAR(50) NOT NULL,
    SDT VARCHAR(11) UNIQUE NOT NULL,
    GioiTinh BIT DEFAULT 0,
    TrangThai BIT DEFAULT 1,
    MaTK INT,
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);

-- 4. Khách hàng
CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    TenKH NVARCHAR(100) NOT NULL,
    NamSinh DATE NOT NULL,
    SDT CHAR(10) UNIQUE NOT NULL,
    GioiTinh BIT DEFAULT 0,
    MaTK INT,
    FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
);

-- 5. Nhà xe
CREATE TABLE NhaXe (
    MaNX INT IDENTITY(1,1) PRIMARY KEY,
    TenNX NVARCHAR(100) NOT NULL,
    SDT CHAR(11) UNIQUE NOT NULL,
    Email CHAR(50) UNIQUE NOT NULL,
    TrangThai BIT DEFAULT 1
);

-- 6. Loại xe
CREATE TABLE LoaiXe (
    MaLX INT IDENTITY(1,1) PRIMARY KEY,
    TenLX NVARCHAR(100) NOT NULL,
    SoCho INT NOT NULL CHECK (SoCho > 0),
    TrangThai BIT DEFAULT 1
);

-- 7. Xe
CREATE TABLE Xe (
    MaXe INT IDENTITY(1,1) PRIMARY KEY,
    MaNX INT NOT NULL,
    MaLX INT NOT NULL,
    BienSoXe CHAR(20) UNIQUE NOT NULL,
    TrangThai BIT DEFAULT 1,
    FOREIGN KEY (MaNX) REFERENCES NhaXe(MaNX),
    FOREIGN KEY (MaLX) REFERENCES LoaiXe(MaLX)
);

-- 8. Tuyến đường
CREATE TABLE TuyenDuong (
    MaTuyen INT IDENTITY(1,1) PRIMARY KEY,
    DiemDi NVARCHAR(255) NOT NULL,
    DiemDen NVARCHAR(255) NOT NULL,
    QuangDuong INT,
    ThoiGianDuKien TIME,
    TrangThai BIT DEFAULT 1
);

-- 9. Giá vé
CREATE TABLE GiaVe (
    MaGV INT IDENTITY(1,1) PRIMARY KEY,
    MaNX INT NOT NULL,
    MaLX INT NOT NULL,
    MaTuyen INT NOT NULL,
    GiaVe DECIMAL(12,0) NOT NULL,
    UNIQUE(MaNX, MaLX, MaTuyen),
    FOREIGN KEY (MaNX) REFERENCES NhaXe(MaNX),
    FOREIGN KEY (MaLX) REFERENCES LoaiXe(MaLX),
    FOREIGN KEY (MaTuyen) REFERENCES TuyenDuong(MaTuyen)
);

-- 10. Tài xế
CREATE TABLE TaiXe (
    MaTX INT IDENTITY(1,1) PRIMARY KEY,
    TenTX NVARCHAR(100),
    NamSinh DATE NOT NULL CHECK (DATEDIFF(YEAR, NamSinh, GETDATE()) > 18),
    CCCD CHAR(12) UNIQUE NOT NULL,
    GioiTinh BIT DEFAULT 0,
    SDT CHAR(10) UNIQUE NOT NULL,
    DiaChi NVARCHAR(100) NOT NULL,
    BangLai CHAR(5) NOT NULL,
    NgayCap DATE NOT NULL,
    TrangThai BIT DEFAULT 1
);

-- 11. Chuyến xe
CREATE TABLE ChuyenXe (
    MaChuyen INT IDENTITY(1,1) PRIMARY KEY,
    MaXe INT NOT NULL,
    MaTuyen INT NOT NULL,
    MaTX INT NOT NULL,
    NgayGioKhoiHanh DATETIME NOT NULL,
    NgayGioDenDuKien DATETIME,
    GiaVeThucTe DECIMAL(12,0),
    FOREIGN KEY (MaXe) REFERENCES Xe(MaXe),
    FOREIGN KEY (MaTuyen) REFERENCES TuyenDuong(MaTuyen),
    FOREIGN KEY (MaTX) REFERENCES TaiXe(MaTX),
	UNIQUE (MaXe, NgayGioKhoiHanh, MaTX)
);

-- 12. Hóa đơn
CREATE TABLE HoaDon (
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT,
    MaNV INT,
    NgayLap DATETIME NOT NULL,
    TongTien DECIMAL(12,0) DEFAULT 0,
    TrangThai BIT DEFAULT 0,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);

-- 13. Thanh toán
CREATE TABLE ThanhToan (
    MaTT INT IDENTITY(1,1) PRIMARY KEY,
    MaHD INT UNIQUE NOT NULL,
    TongTien DECIMAL(12,0) DEFAULT 0,
    NgayTT DATETIME NOT NULL,
    PhuongThuc NVARCHAR(50) NOT NULL,
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD) on delete cascade
);

-- 14. Vé xe
CREATE TABLE VeXe (
    MaVe INT IDENTITY(1,1) PRIMARY KEY,
    MaChuyen INT NOT NULL,
    MaHD INT NOT NULL,
    ChoNgoi INT NOT NULL,
    GiaVe DECIMAL(12,0) DEFAULT 0,
    UNIQUE(MaChuyen, ChoNgoi),
    FOREIGN KEY (MaChuyen) REFERENCES ChuyenXe(MaChuyen),
    FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD) on delete cascade
);
--trg
--1
go
CREATE TRIGGER trg_TinhNgayDen
ON ChuyenXe
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE cx
    SET cx.NgayGioDenDuKien =
        DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', td.ThoiGianDuKien), cx.NgayGioKhoiHanh)
    FROM ChuyenXe cx
    INNER JOIN inserted i ON cx.MaChuyen = i.MaChuyen
    INNER JOIN TuyenDuong td ON td.MaTuyen = cx.MaTuyen;
END;
GO
--2
CREATE TRIGGER trg_GiaVeChoChuyenXe
ON ChuyenXe
AFTER INSERT
AS
BEGIN
    UPDATE cx
    SET GiaVeThucTe = gv.GiaVe
    FROM ChuyenXe cx
    INNER JOIN inserted i ON cx.MaChuyen = i.MaChuyen
    INNER JOIN Xe x ON x.MaXe = cx.MaXe
    INNER JOIN GiaVe gv ON gv.MaNX = x.MaNX AND gv.MaLX = x.MaLX AND gv.MaTuyen = cx.MaTuyen;
END;
GO
--3
CREATE TRIGGER trg_GiaVeVeXe
ON VeXe
AFTER INSERT
AS
BEGIN
    UPDATE vx
    SET vx.GiaVe = cx.GiaVeThucTe
    FROM VeXe vx
    INNER JOIN inserted i ON vx.MaVe = i.MaVe
    INNER JOIN ChuyenXe cx ON cx.MaChuyen = vx.MaChuyen;
END;
GO
--4
CREATE TRIGGER trg_TongTien_Add
ON VeXe
AFTER INSERT
AS
BEGIN
    UPDATE hd
    SET TongTien = sub.TongTien
    FROM HoaDon hd
    INNER JOIN (
        SELECT MaHD, SUM(GiaVe) AS TongTien
        FROM VeXe
        GROUP BY MaHD
    ) sub ON hd.MaHD = sub.MaHD;
END;
GO
--5
CREATE TRIGGER trg_TongTien_Delete
ON VeXe
AFTER DELETE
AS
BEGIN
    UPDATE hd
    SET TongTien = ISNULL(sub.TongTien,0)
    FROM HoaDon hd
    LEFT JOIN (
        SELECT MaHD, SUM(GiaVe) AS TongTien
        FROM VeXe
        GROUP BY MaHD
    ) sub ON hd.MaHD = sub.MaHD;
END;
GO
--6
CREATE TRIGGER trg_ThanhToan_HoaDon
ON ThanhToan
AFTER INSERT
AS
BEGIN
    UPDATE hd
    SET TrangThai = 1,
        -- cập nhật lại tiền
        TongTien = tt.TongTien
    FROM HoaDon hd
    INNER JOIN inserted i ON hd.MaHD = i.MaHD
    INNER JOIN ThanhToan tt ON tt.MaHD = hd.MaHD;
END;
GO
--data

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
INSERT INTO NhanVien (TenNV, NamSinh, GioiTinh, DiaChi, SDT, MaTK) VALUES
(N'Nguyễn Văn An', '1980-01-15',1, N'TP Hồ Chí Minh', '0912345678', 1),
(N'Trần Thị Bình', '1982-03-20',0,  N'Long An', '0912345679', 2),
(N'Lê Văn Cường', '1978-07-10',1,  N'Bình Định', '0912345680', 3),
(N'Nguyễn Thị Dung', '1985-05-05',0,  N'Cần Thơ', '0912345681', null),
(N'Hoàng Văn Em', '1983-11-25',1,  N'An Giang', '0912345682', null);

-- 2. KhachHang
INSERT INTO KhachHang (TenKH, NamSinh, SDT, GioiTinh) VALUES
(N'Lê Văn Trung', '1990-02-15', '0981111111',1),
(N'Phạm Thị Hoa', '1988-04-20', '0981111112', 0),
(N'Nguyễn Văn Nam', '1992-06-25', '0981111113',1),
(N'Trần Thị Linh', '1995-03-30', '0981111114',0),
(N'Vũ Văn Minh', '1987-09-10', '0981111115',1),
(N'Đỗ Thị Mỹ', '1991-12-05', '0981111116',0),
(N'Nguyễn Thị Nga', '1993-07-07', '0981111117',0),
(N'Lê Văn Phong', '1989-08-08', '0981111118',1),
(N'Hoàng Thị Quyên', '1990-10-10', '0981111119',0),
(N'Trần Văn Sơn', '1985-11-11', '0981111120',1),
(N'Phạm Thị Uyên', '1992-01-01', '0981111121',0),
(N'Nguyễn Văn Vinh', '1988-02-02', '0981111122',1),
(N'Lê Thị Xuân', '1994-03-03', '0981111123',0),
(N'Đỗ Văn Ý', '1996-04-04', '0981111124',0),
(N'Trần Thị Diễm', '1993-05-05', '0981111125',0),
(N'Vũ Văn Anh', '1991-06-06', '0981111126',1),
(N'Nguyễn Thị Bảo', '1990-07-07', '0981111127',0),
(N'Lê Văn Cẩn', '1987-08-08', '0981111128',1),
(N'Phạm Thị Đào', '1994-09-09', '0981111129',0),
(N'Trần Văn Lợi', '1992-10-10', '0981111130',1);
UPDATE KhachHang SET MaTK = 4 WHERE MaKH = 1; 
UPDATE KhachHang SET MaTK = 5 WHERE MaKH = 2; 
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
(N'TP HCM', N'Vũng Tàu', 95, '02:00:00'),
(N'TP HCM', N'Đà Lạt', 300, '07:00:00'),
(N'TP HCM', N'Nha Trang', 430, '09:00:00'),
(N'TP HCM', N'Phan Thiết', 200, '04:00:00'),
(N'TP HCM', N'Quy Nhơn', 650, '12:00:00'),
(N'TP HCM', N'Đà Nẵng', 960, '16:00:00'),
(N'TP HCM', N'Buôn Ma Thuột', 330, '07:30:00');

-- Tạo tuyến đường ngược chiều
INSERT INTO TuyenDuong (DiemDi, DiemDen, QuangDuong, ThoiGianDuKien)
SELECT DiemDen, DiemDi, QuangDuong, ThoiGianDuKien
FROM TuyenDuong td
WHERE NOT EXISTS (
    SELECT 1 FROM TuyenDuong t2
    WHERE t2.DiemDi = td.DiemDen AND t2.DiemDen = td.DiemDi
);


--7. TAIXE
INSERT INTO TaiXe (TenTX, NamSinh, CCCD,GioiTinh, SDT,  DiaChi, BangLai, NgayCap) VALUES
(N'Nguyễn Văn Hậu', '1985-04-15', '123456789012',1, '0909123456', N'TP.HCM', 'D', '2010-05-20'),
(N'Lê Văn Sơn', '1982-02-20', '123456789013', 1,'0909234567', N'TP.HCM', 'E', '2009-06-10'),
(N'Phạm Minh Tâm', '1988-08-08', '123456789014',1, '0909345678', N'TP.HCM', 'D', '2011-09-12'),
(N'Trần Hữu Phát', '1990-05-10', '123456789015',1, '0909456789', N'TP.HCM', 'E', '2012-12-01');

-- 8. GiaVe 
-- Nhà xe 1 (Ghế ngồi + Giường nằm)
INSERT INTO GiaVe VALUES
(1,1,1,150000),(1,2,1,180000),
(1,1,2,200000),(1,2,2,240000),
(1,1,3,240000),(1,2,3,288000),
(1,1,4,180000),(1,2,4,216000),
(1,1,5,300000),(1,2,5,360000),
(1,1,6,350000),(1,2,6,420000),
(1,1,7,220000),(1,2,7,264000);
-- Nhà xe 2
INSERT INTO GiaVe VALUES
(2,1,1,160000),(2,2,1,190000),
(2,1,2,210000),(2,2,2,250000),
(2,1,3,250000),(2,2,3,298000),
(2,1,4,190000),(2,2,4,226000),
(2,1,5,310000),(2,2,5,370000),
(2,1,6,360000),(2,2,6,430000),
(2,1,7,230000),(2,2,7,274000);
-- Nhà xe 3 (không có ghế ngồi)
INSERT INTO GiaVe VALUES
(3,2,1,200000),(3,3,1,245000),
(3,2,2,260000),(3,3,2,320000),
(3,2,3,308000),(3,3,3,380000),
(3,2,4,236000),(3,3,4,290000),
(3,2,5,380000),(3,3,5,470000),
(3,2,6,440000),(3,3,6,550000),
(3,2,7,284000),(3,3,7,350000);
-- Nhà xe 4
INSERT INTO GiaVe VALUES
(4,1,1,150000),(4,2,1,180000),
(4,1,2,200000),(4,2,2,240000),
(4,1,3,240000),(4,2,3,288000),
(4,1,4,180000),(4,2,4,216000),
(4,1,5,300000),(4,2,5,360000),
(4,1,6,350000),(4,2,6,420000),
(4,1,7,220000),(4,2,7,264000);
-- Nhà xe 5 (chỉ có limousine)
INSERT INTO GiaVe VALUES
(5,3,1,255000),
(5,3,2,330000),
(5,3,3,390000),
(5,3,4,300000),
(5,3,5,480000),
(5,3,6,560000),
(5,3,7,370000);

-- Sinh giá vé cho tuyến đường ngược
INSERT INTO GiaVe (MaNX, MaLX, MaTuyen, GiaVe)
SELECT 
    gv.MaNX,
    gv.MaLX,
    td2.MaTuyen,   -- tuyến mới
    gv.GiaVe
FROM GiaVe gv
JOIN TuyenDuong td1 ON td1.MaTuyen = gv.MaTuyen
JOIN TuyenDuong td2 ON td2.DiemDi = td1.DiemDen AND td2.DiemDen = td1.DiemDi
WHERE NOT EXISTS (
    SELECT 1
    FROM GiaVe g2
    WHERE g2.MaNX = gv.MaNX
      AND g2.MaLX = gv.MaLX
      AND g2.MaTuyen = td2.MaTuyen
);
-- 7. ChuyenXe
INSERT INTO ChuyenXe (MaXe, MaTuyen, MaTX, NgayGioKhoiHanh) VALUES
(1,1,1,'2026-05-08 12:00:00'),
(2,2,2,'2026-05-08 12:00:00'),
(3,3,3,'2026-05-08 12:00:00'),
(4,4,4,'2026-05-08 12:30:00'),
(5,5,1,'2026-05-09 12:00:00'),
(6,5,2,'2026-05-09 12:00:00'),
(1,2,3,'2026-05-09 12:00:00');

-- Sinh dữ liệu chuyến xe tự động cho tháng 5/2026
DECLARE @StartDate DATE = '2026-05-01';
DECLARE @EndDate DATE   = '2026-05-31';

DECLARE @XeCount INT = (SELECT COUNT(*) FROM Xe);
DECLARE @TXCount INT = (SELECT COUNT(*) FROM TaiXe);

DECLARE @XeIndex INT = 1;
DECLARE @TXIndex INT = 1;

;WITH AllDates AS (
    SELECT @StartDate AS Ngay
    UNION ALL
    SELECT DATEADD(DAY, 1, Ngay)
    FROM AllDates
    WHERE Ngay < @EndDate
)
SELECT *
INTO #Dates
FROM AllDates;

SELECT MaTuyen
INTO #Tuyen
FROM TuyenDuong
ORDER BY MaTuyen;

DECLARE @Ngay DATE;
DECLARE @Tuyen INT;
DECLARE @Gio TIME;
DECLARE @MaXe INT;
DECLARE @MaTX INT;

DECLARE cur CURSOR FOR
    SELECT d.Ngay, t.MaTuyen, g.Gio
    FROM #Dates d
    CROSS JOIN #Tuyen t
    CROSS JOIN (VALUES ('07:00'), ('15:00')) g(Gio)
    ORDER BY d.Ngay, t.MaTuyen, g.Gio;

OPEN cur;
FETCH NEXT FROM cur INTO @Ngay, @Tuyen, @Gio;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Lấy xe theo thứ tự tuần tự
    SELECT @MaXe = MaXe
    FROM (
        SELECT MaXe, ROW_NUMBER() OVER (ORDER BY MaXe) AS rn
        FROM Xe
    ) x
    WHERE x.rn = @XeIndex;

    -- Kiểm tra xe này có chạy được tuyến này không (dựa theo GiaVe)
    IF EXISTS (
        SELECT 1 
        FROM Xe x
        JOIN GiaVe gv ON gv.MaNX = x.MaNX AND gv.MaLX = x.MaLX
        WHERE x.MaXe = @MaXe AND gv.MaTuyen = @Tuyen
    )
    BEGIN
        -- Chọn tài xế theo thứ tự tuần tự
        SELECT @MaTX = MaTX
        FROM (
            SELECT MaTX, ROW_NUMBER() OVER (ORDER BY MaTX) AS rn
            FROM TaiXe
        ) tx
        WHERE tx.rn = @TXIndex;

        -- Tạo chuyến xe
        INSERT INTO ChuyenXe (MaXe, MaTuyen, MaTX, NgayGioKhoiHanh)
        VALUES (
            @MaXe,
            @Tuyen,
            @MaTX,
            DATEADD(HOUR, DATEPART(HOUR, @Gio), CAST(@Ngay AS DATETIME))
        );

        -- Tăng index tài xế
        SET @TXIndex = @TXIndex + 1;
        IF @TXIndex > @TXCount SET @TXIndex = 1;
    END

    -- Tăng index xe (luôn tăng dù tuyến không hợp lệ)
    SET @XeIndex = @XeIndex + 1;
    IF @XeIndex > @XeCount SET @XeIndex = 1;

    FETCH NEXT FROM cur INTO @Ngay, @Tuyen, @Gio;
END;

CLOSE cur;
DEALLOCATE cur;

DROP TABLE #Dates;
DROP TABLE #Tuyen;


-- 9. HoaDon (Ngày lập hóa đơn được dời về gần đây)
INSERT INTO HoaDon (MaKH, MaNV, NgayLap) VALUES
(1,1,'2026-05-06 06:30:00'),
(2,2,'2026-05-07 06:00:00'),
(3,3,'2026-05-07 06:30:00'),
(4,4,'2026-05-08 06:00:00'),
(5,5,'2026-05-08 06:30:00'),
(6,1,'2026-05-07 06:00:00'),
(7,2,'2026-05-08 06:00:00'),
(8,3,'2026-05-08 06:00:00'),
(9,1,'2026-05-08 06:00:00'),
(10,2,'2026-05-09 06:00:00'),
(11,3,'2026-05-09 06:00:00'),
(12,4,'2026-05-09 06:00:00'),
(13,5,'2026-05-09 06:00:00');


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


-- 11. ThanhToan (Ngày thanh toán được dời về gần đây)
INSERT INTO ThanhToan (MaHD, NgayTT, PhuongThuc) VALUES
(1, '2026-05-06 07:30:00', N'Tiền mặt'),
(2, '2026-05-06 08:00:00', N'Thẻ ATM'),
(3, '2026-05-06 08:30:00', N'Tiền mặt'),
(4, '2026-05-07 09:30:00', N'Tiền mặt'),
(5, '2026-05-07 09:30:00', N'Thẻ ATM'),
(6, '2026-05-08 10:00:00', N'Tiền mặt'),
(7, '2026-05-08 10:30:00', N'Tiền mặt'),
(8, '2026-05-08 11:00:00', N'Tiền mặt'),
(9, '2026-05-09 07:30:00', N'Tiền mặt'),
(10, '2026-05-09 08:00:00', N'Thẻ ATM'),
(11, '2026-05-09 09:30:00', N'Tiền mặt'),
(12, '2026-05-09 11:00:00', N'Tiền mặt');

-- Truy vấn test lại thông tin nhân viên
SELECT * FROM NhanVien WHERE MaTk=1;