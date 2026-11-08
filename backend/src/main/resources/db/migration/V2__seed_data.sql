-- Seed data (English naming convention).
-- Demo accounts are seeded at application startup (BCrypt via DataInitializer).

INSERT INTO roles (role_name, description) VALUES
    ('Admin', 'Administrator with full system privileges'),
    ('NhanVien', 'Ticketing and operations staff'),
    ('KhachHang', 'Passenger and customer');

INSERT INTO bus_companies (company_name, phone, email) VALUES
    ('Nhà xe Phương Trang', '19006067', 'info@futabus.vn'),
    ('Nhà xe Thành Bưởi', '19006079', 'cskh@thanhbuoi.com.vn'),
    ('Nhà xe Hoàng Long', '1900988920', 'info@hoanglongasia.com'),
    ('Nhà xe Mai Linh', '1055', 'cskh@mailinh.vn'),
    ('Nhà xe Kumho Samco', '02835114648', 'lienhe@kumhosamco.com.vn');

INSERT INTO vehicle_types (type_name, seat_capacity) VALUES
    ('Ghế ngồi', 40),
    ('Giường nằm', 45),
    ('Limousine', 40);

INSERT INTO vehicles (company_id, type_id, license_plate) VALUES
    (1, 1, '29A-12345'),
    (2, 1, '29B-45634'),
    (3, 2, '30A-34567'),
    (4, 1, '30B-45678'),
    (5, 3, '31A-56789'),
    (1, 2, '31B-67890'),
    (2, 2, '32A-78901');

INSERT INTO routes (departure, destination, distance, estimated_duration) VALUES
    ('TP HCM', 'Vũng Tàu', 95, '02:00:00'),
    ('TP HCM', 'Đà Lạt', 300, '07:00:00'),
    ('TP HCM', 'Nha Trang', 430, '09:00:00'),
    ('TP HCM', 'Phan Thiết', 200, '04:00:00'),
    ('TP HCM', 'Quy Nhơn', 650, '12:00:00'),
    ('TP HCM', 'Đà Nẵng', 960, '16:00:00'),
    ('TP HCM', 'Buôn Ma Thuột', 330, '07:30:00');

-- Return routes
INSERT INTO routes (departure, destination, distance, estimated_duration)
SELECT destination, departure, distance, estimated_duration
FROM routes
WHERE route_id <= 7;

INSERT INTO drivers (full_name, date_of_birth, citizen_id, gender, phone, address, license_number, issue_date) VALUES
    ('Nguyễn Văn Hậu', '1985-04-15', '123456789012', TRUE, '0909123456', 'TP.HCM', 'D', '2010-05-20'),
    ('Lê Văn Sơn', '1982-02-20', '123456789013', TRUE, '0909234567', 'TP.HCM', 'E', '2009-06-10'),
    ('Phạm Minh Tâm', '1988-08-08', '123456789014', TRUE, '0909345678', 'TP.HCM', 'D', '2011-09-12'),
    ('Trần Hữu Phát', '1990-05-10', '123456789015', TRUE, '0909456789', 'TP.HCM', 'E', '2012-12-01');

INSERT INTO employees (full_name, date_of_birth, gender, address, phone) VALUES
    ('Nguyễn Văn An', '1980-01-15', TRUE, 'TP Hồ Chí Minh', '0912345678'),
    ('Trần Thị Bình', '1982-03-20', FALSE, 'Long An', '0912345679'),
    ('Lê Văn Cường', '1978-07-10', TRUE, 'Bình Định', '0912345680'),
    ('Nguyễn Thị Dung', '1985-05-05', FALSE, 'Cần Thơ', '0912345681'),
    ('Hoàng Văn Em', '1983-11-25', TRUE, 'An Giang', '0912345682');

INSERT INTO customers (full_name, date_of_birth, phone, gender) VALUES
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

-- Ticket prices
INSERT INTO ticket_prices (company_id, type_id, route_id, price)
SELECT v.company_id, v.type_id, r.route_id, v.prices[r.ord::int]
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
) AS v (company_id, type_id, prices)
JOIN (
    SELECT route_id, ROW_NUMBER() OVER (ORDER BY route_id) AS ord
    FROM routes
    WHERE route_id <= 7
) r ON r.ord <= array_length(v.prices, 1);

-- Mirror prices for return routes
INSERT INTO ticket_prices (company_id, type_id, route_id, price)
SELECT tp.company_id, tp.type_id, r2.route_id, tp.price
FROM ticket_prices tp
JOIN routes r1 ON r1.route_id = tp.route_id AND r1.route_id <= 7
JOIN routes r2 ON r2.departure = r1.destination AND r2.destination = r1.departure;

-- Seed trips for the next 14 days
INSERT INTO trips (vehicle_id, route_id, driver_id, departure_time, estimated_arrival_time, actual_price)
WITH candidate AS (
    SELECT v.vehicle_id, tp.route_id, MIN(tp.price) AS price
    FROM vehicles v
    JOIN ticket_prices tp ON tp.company_id = v.company_id AND tp.type_id = v.type_id
    GROUP BY v.vehicle_id, tp.route_id
), chosen AS (
    SELECT vehicle_id, route_id, price,
           ROW_NUMBER() OVER (PARTITION BY vehicle_id ORDER BY route_id) AS rn_veh
    FROM candidate
), final AS (
    SELECT vehicle_id, route_id, price,
           ROW_NUMBER() OVER (ORDER BY vehicle_id, route_id) AS rn
    FROM chosen
    WHERE rn_veh <= 2
)
SELECT f.vehicle_id,
       f.route_id,
       (
           SELECT d.driver_id
           FROM drivers d
           ORDER BY d.driver_id
           OFFSET ((f.rn - 1) % (SELECT COUNT(*) FROM drivers))
           LIMIT 1
       ),
       dep.departure_ts,
       dep.departure_ts + (r.estimated_duration - time '00:00'),
       f.price
FROM final f
JOIN routes r ON r.route_id = f.route_id
CROSS JOIN generate_series(0, 13) AS day_offset
CROSS JOIN LATERAL (
    SELECT (CURRENT_DATE + day_offset)::timestamp + make_interval(hours => (4 + f.rn)::int) AS departure_ts
) dep
WHERE dep.departure_ts > now()
ORDER BY dep.departure_ts;
