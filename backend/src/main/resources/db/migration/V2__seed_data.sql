-- Dữ liệu mẫu. Tài khoản được tạo ở tầng ứng dụng (BCrypt) khi khởi động lần đầu,
-- sau đó tự gắn với nhân viên/khách hàng theo số điện thoại.

INSERT INTO quyen (ten_quyen, mo_ta) VALUES
    ('Admin', 'Toàn quyền quản trị hệ thống'),
    ('NhanVien', 'Nhân viên bán vé'),
    ('KhachHang', 'Khách hàng sử dụng hệ thống');

INSERT INTO nha_xe (ten_nx, sdt, email) VALUES
    ('Nhà xe Phương Trang', '19006067', 'info@futabus.vn'),
    ('Nhà xe Thành Bưởi', '19006079', 'cskh@thanhbuoi.com.vn'),
    ('Nhà xe Hoàng Long', '1900988920', 'info@hoanglongasia.com'),
    ('Nhà xe Mai Linh', '1055', 'cskh@mailinh.vn'),
    ('Nhà xe Kumho Samco', '02835114648', 'lienhe@kumhosamco.com.vn');

INSERT INTO loai_xe (ten_lx, so_cho) VALUES
    ('Ghế ngồi', 40),
    ('Giường nằm', 45),
    ('Limousine', 40);

INSERT INTO xe (ma_nx, ma_lx, bien_so_xe) VALUES
    (1, 1, '29A-12345'),
    (2, 1, '29B-45634'),
    (3, 2, '30A-34567'),
    (4, 1, '30B-45678'),
    (5, 3, '31A-56789'),
    (1, 2, '31B-67890'),
    (2, 2, '32A-78901');

INSERT INTO tuyen_duong (diem_di, diem_den, quang_duong, thoi_gian_du_kien) VALUES
    ('TP HCM', 'Vũng Tàu', 95, '02:00:00'),
    ('TP HCM', 'Đà Lạt', 300, '07:00:00'),
    ('TP HCM', 'Nha Trang', 430, '09:00:00'),
    ('TP HCM', 'Phan Thiết', 200, '04:00:00'),
    ('TP HCM', 'Quy Nhơn', 650, '12:00:00'),
    ('TP HCM', 'Đà Nẵng', 960, '16:00:00'),
    ('TP HCM', 'Buôn Ma Thuột', 330, '07:30:00');

INSERT INTO tuyen_duong (diem_di, diem_den, quang_duong, thoi_gian_du_kien)
SELECT diem_den, diem_di, quang_duong, thoi_gian_du_kien
FROM tuyen_duong
WHERE ma_tuyen <= 7;

INSERT INTO tai_xe (ten_tx, nam_sinh, cccd, gioi_tinh, sdt, dia_chi, bang_lai, ngay_cap) VALUES
    ('Nguyễn Văn Hậu', '1985-04-15', '123456789012', TRUE, '0909123456', 'TP.HCM', 'D', '2010-05-20'),
    ('Lê Văn Sơn', '1982-02-20', '123456789013', TRUE, '0909234567', 'TP.HCM', 'E', '2009-06-10'),
    ('Phạm Minh Tâm', '1988-08-08', '123456789014', TRUE, '0909345678', 'TP.HCM', 'D', '2011-09-12'),
    ('Trần Hữu Phát', '1990-05-10', '123456789015', TRUE, '0909456789', 'TP.HCM', 'E', '2012-12-01');

INSERT INTO nhan_vien (ten_nv, nam_sinh, gioi_tinh, dia_chi, sdt) VALUES
    ('Nguyễn Văn An', '1980-01-15', TRUE, 'TP Hồ Chí Minh', '0912345678'),
    ('Trần Thị Bình', '1982-03-20', FALSE, 'Long An', '0912345679'),
    ('Lê Văn Cường', '1978-07-10', TRUE, 'Bình Định', '0912345680'),
    ('Nguyễn Thị Dung', '1985-05-05', FALSE, 'Cần Thơ', '0912345681'),
    ('Hoàng Văn Em', '1983-11-25', TRUE, 'An Giang', '0912345682');

INSERT INTO khach_hang (ten_kh, nam_sinh, sdt, gioi_tinh) VALUES
    ('Lê Văn Trung', '1990-02-15', '0981111111', TRUE),
    ('Phạm Thị Hoa', '1988-04-20', '0981111112', FALSE),
    ('Nguyễn Văn Nam', '1992-06-25', '0981111113', TRUE),
    ('Trần Thị Linh', '1995-03-30', '0981111114', FALSE),
    ('Vũ Văn Minh', '1987-09-10', '0981111115', TRUE),
    ('Đỗ Thị Mỹ', '1991-12-05', '0981111116', FALSE),
    ('Nguyễn Thị Nga', '1993-07-07', '0981111117', FALSE),
    ('Lê Văn Phong', '1989-08-08', '0981111118', TRUE),
    ('Hoàng Thị Quyên', '1990-10-10', '0981111119', FALSE),
    ('Trần Văn Sơn', '1985-11-11', '0981111120', TRUE),
    ('Phạm Thị Uyên', '1992-01-01', '0981111121', FALSE),
    ('Nguyễn Văn Vinh', '1988-02-02', '0981111122', TRUE),
    ('Lê Thị Xuân', '1994-03-03', '0981111123', FALSE),
    ('Đỗ Văn Ý', '1996-04-04', '0981111124', TRUE),
    ('Trần Thị Diễm', '1993-05-05', '0981111125', FALSE),
    ('Vũ Văn Anh', '1991-06-06', '0981111126', TRUE),
    ('Nguyễn Thị Bảo', '1990-07-07', '0981111127', FALSE),
    ('Lê Văn Cẩn', '1987-08-08', '0981111128', TRUE),
    ('Phạm Thị Đào', '1994-09-09', '0981111129', FALSE),
    ('Trần Văn Lợi', '1992-10-10', '0981111130', TRUE);

-- Bảng giá theo (nhà xe, loại xe, tuyến). Giá 7 tuyến chiều đi, sau đó nhân bản
-- sang 7 tuyến chiều về với cùng mức giá.
INSERT INTO gia_ve (ma_nx, ma_lx, ma_tuyen, gia_ve)
SELECT v.ma_nx, v.ma_lx, t.ma_tuyen, v.gia[t.ord::int]
FROM (VALUES
    (1, 1, ARRAY[150000, 200000, 240000, 180000, 300000, 350000, 220000]),
    (1, 2, ARRAY[180000, 240000, 288000, 216000, 360000, 420000, 264000]),
    (2, 1, ARRAY[160000, 210000, 250000, 190000, 310000, 360000, 230000]),
    (2, 2, ARRAY[190000, 250000, 298000, 226000, 370000, 430000, 274000]),
    (3, 2, ARRAY[200000, 260000, 308000, 236000, 380000, 440000, 284000]),
    (3, 3, ARRAY[245000, 320000, 380000, 290000, 470000, 550000, 350000]),
    (4, 1, ARRAY[150000, 200000, 240000, 180000, 300000, 350000, 220000]),
    (4, 2, ARRAY[180000, 240000, 288000, 216000, 360000, 420000, 264000]),
    (5, 3, ARRAY[255000, 330000, 390000, 300000, 480000, 560000, 370000])
) AS v (ma_nx, ma_lx, gia)
JOIN (
    SELECT ma_tuyen, ROW_NUMBER() OVER (ORDER BY ma_tuyen) AS ord
    FROM tuyen_duong
    WHERE ma_tuyen <= 7
) t ON t.ord <= array_length(v.gia, 1);

INSERT INTO gia_ve (ma_nx, ma_lx, ma_tuyen, gia_ve)
SELECT gv.ma_nx, gv.ma_lx, t2.ma_tuyen, gv.gia_ve
FROM gia_ve gv
JOIN tuyen_duong t1 ON t1.ma_tuyen = gv.ma_tuyen AND t1.ma_tuyen <= 7
JOIN tuyen_duong t2 ON t2.diem_di = t1.diem_den AND t2.diem_den = t1.diem_di;

-- Chuyến xe cho 14 ngày kể từ hôm nay: mỗi xe chạy 2 tuyến, mỗi tuyến 1 chuyến/ngày,
-- giờ khởi hành trải đều 05:00–18:00 nên không xe nào trùng giờ.
INSERT INTO chuyen_xe (ma_xe, ma_tuyen, ma_tx, ngay_gio_khoi_hanh, ngay_gio_den_du_kien, gia_ve_thuc_te)
WITH candidate AS (
    SELECT x.ma_xe, gv.ma_tuyen, MIN(gv.gia_ve) AS gia_ve
    FROM xe x
    JOIN gia_ve gv ON gv.ma_nx = x.ma_nx AND gv.ma_lx = x.ma_lx
    GROUP BY x.ma_xe, gv.ma_tuyen
), chosen AS (
    SELECT ma_xe, ma_tuyen, gia_ve,
           ROW_NUMBER() OVER (PARTITION BY ma_xe ORDER BY ma_tuyen) AS rn_xe
    FROM candidate
), final AS (
    SELECT ma_xe, ma_tuyen, gia_ve,
           ROW_NUMBER() OVER (ORDER BY ma_xe, ma_tuyen) AS rn
    FROM chosen
    WHERE rn_xe <= 2
)
SELECT f.ma_xe,
       f.ma_tuyen,
       (
           SELECT tx.ma_tx
           FROM tai_xe tx
           ORDER BY tx.ma_tx
           OFFSET ((f.rn - 1) % (SELECT COUNT(*) FROM tai_xe))
           LIMIT 1
       ),
       dep.bat_dau,
       dep.bat_dau + (td.thoi_gian_du_kien - time '00:00'),
       f.gia_ve
FROM final f
JOIN tuyen_duong td ON td.ma_tuyen = f.ma_tuyen
CROSS JOIN generate_series(0, 13) AS d
CROSS JOIN LATERAL (
    SELECT (CURRENT_DATE + d)::timestamp + make_interval(hours => (4 + f.rn)::int) AS bat_dau
) dep
WHERE dep.bat_dau > now()
ORDER BY dep.bat_dau;
