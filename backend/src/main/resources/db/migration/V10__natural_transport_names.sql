-- Rename only the original seeded labels. Preserve IDs, bookings, fares and times.
-- Names/plates are illustrative fixtures, not verified operator or employee data.
UPDATE bus_companies SET company_name = replace(company_name, ' (DEMO)', '')
WHERE company_name IN ('Nhà xe Phương Trang (DEMO)', 'Nhà xe Thành Bưởi (DEMO)');

UPDATE vehicle_types SET type_name = replace(type_name, ' (DEMO)', '')
WHERE type_name IN ('Limousine 24 chỗ (DEMO)', 'Ghế ngồi 40 chỗ (DEMO)');

UPDATE vehicles v SET license_plate = n.new_plate
FROM (VALUES
  ('DEMO-PT-DL', '51B-310.01'),
  ('DEMO-PT-NT', '51B-310.02'),
  ('DEMO-PT-VT', '51B-310.03'),
  ('DEMO-TB-DL', '51B-310.04'),
  ('DEMO-TB-NT', '51B-310.05'),
  ('DEMO-TB-VT', '51B-310.06')
) AS n(old_plate, new_plate)
WHERE v.license_plate = n.old_plate;

UPDATE drivers d SET full_name = n.new_name
FROM (VALUES
  ('Tài xế DEMO PT-DL lượt đi', 'Nguyễn Văn Hùng'),
  ('Tài xế DEMO PT-DL lượt về', 'Trần Minh Tuấn'),
  ('Tài xế DEMO PT-NT lượt đi', 'Lê Quốc Bảo'),
  ('Tài xế DEMO PT-NT lượt về', 'Phạm Thanh Sơn'),
  ('Tài xế DEMO PT-VT lượt đi', 'Võ Văn Hải'),
  ('Tài xế DEMO PT-VT lượt về', 'Đặng Minh Đức'),
  ('Tài xế DEMO TB-DL lượt đi', 'Bùi Quốc Huy'),
  ('Tài xế DEMO TB-DL lượt về', 'Đỗ Anh Dũng'),
  ('Tài xế DEMO TB-NT lượt đi', 'Hồ Văn Phúc'),
  ('Tài xế DEMO TB-NT lượt về', 'Ngô Thanh Tùng'),
  ('Tài xế DEMO TB-VT lượt đi', 'Dương Văn Long'),
  ('Tài xế DEMO TB-VT lượt về', 'Phan Minh Quân')
) AS n(old_name, new_name)
WHERE d.full_name = n.old_name;

UPDATE trip_schedules s
SET name = regexp_replace(c.company_name, '^Nhà xe ', '') || ' | '
  || r.departure || ' → ' || r.destination || ' | ' || to_char(s.departure_time, 'HH24:MI')
FROM vehicles v JOIN bus_companies c ON c.company_id = v.company_id, routes r
WHERE s.vehicle_id = v.vehicle_id AND s.route_id = r.route_id AND s.name LIKE 'DEMO - %';

