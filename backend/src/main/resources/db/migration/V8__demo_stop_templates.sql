-- Fictional demonstration points; NOT published operating stops of these companies.
-- No existing trips/tickets are changed and no recurring schedule is enabled.
INSERT INTO stop_locations(name, address, area)
SELECT 'DEMO - ' || city || ' - đầu mối', 'Địa chỉ mô phỏng tại ' || city || '; cần xác nhận với nhà xe trước khi sử dụng thực tế.', city
FROM (VALUES ('TP HCM'), ('Vũng Tàu'), ('Đà Lạt'), ('Nha Trang')) AS cities(city);

INSERT INTO stop_templates(name, company_id, route_id)
SELECT 'DEMO - ' || c.company_name || ' - ' || r.departure || ' → ' || r.destination,
       c.company_id, r.route_id
FROM bus_companies c CROSS JOIN routes r
WHERE c.company_name IN ('Nhà xe Phương Trang', 'Nhà xe Thành Bưởi')
AND ((r.departure = 'TP HCM' AND r.destination IN ('Vũng Tàu', 'Đà Lạt', 'Nha Trang'))
 OR (r.destination = 'TP HCM' AND r.departure IN ('Vũng Tàu', 'Đà Lạt', 'Nha Trang')));

INSERT INTO template_stops(template_id, location_id, stop_order, boarding, dropoff, offset_minutes)
SELECT t.template_id, l.location_id, 0, TRUE, FALSE, 0
FROM stop_templates t JOIN routes r ON r.route_id = t.route_id
JOIN stop_locations l ON l.area = r.departure AND l.name = 'DEMO - ' || r.departure || ' - đầu mối'
WHERE t.name LIKE 'DEMO - %';

INSERT INTO template_stops(template_id, location_id, stop_order, boarding, dropoff, offset_minutes)
SELECT t.template_id, l.location_id, 1, FALSE, TRUE,
       (EXTRACT(EPOCH FROM r.estimated_duration) / 60)::INTEGER
FROM stop_templates t JOIN routes r ON r.route_id = t.route_id
JOIN stop_locations l ON l.area = r.destination AND l.name = 'DEMO - ' || r.destination || ' - đầu mối'
WHERE t.name LIKE 'DEMO - %';
