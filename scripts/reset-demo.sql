-- OPT-IN local demo reset. Never install this script as a Flyway migration.
-- Preserves accounts, roles, customers, employees, auth settings and Flyway history.
\set ON_ERROR_STOP on
\if :{?confirm_demo_reset}
SELECT :'confirm_demo_reset' = 'RESET_COACHBOOKING_DEMO' AS confirmed \gset
\else
\echo 'Missing confirm_demo_reset. No data changed.'
\quit 3
\endif
\if :confirmed
\else
\echo 'Invalid confirmation. No data changed.'
\quit 3
\endif

BEGIN;
SET LOCAL TIME ZONE 'Asia/Ho_Chi_Minh';
SET LOCAL lock_timeout = '10s';
SELECT pg_advisory_xact_lock(hashtext('coachbooking_demo_reset'));
DO $$
BEGIN
  IF current_database() NOT IN ('coachbooking', 'coachbooking_feature_test') THEN
    RAISE EXCEPTION 'Only the local CoachBooking or isolated feature-test database is supported.';
  END IF;
  IF NOT EXISTS (SELECT 1 FROM flyway_schema_history WHERE version = '9' AND success) THEN
    RAISE EXCEPTION 'Migrations through V9 must be applied before resetting demo data.';
  END IF;
END $$;

CREATE TEMP TABLE preserved_counts ON COMMIT DROP AS
SELECT (SELECT count(*) FROM accounts) accounts,
       (SELECT count(*) FROM customers) customers,
       (SELECT count(*) FROM employees) employees,
       (SELECT count(*) FROM roles) roles;

-- No CASCADE: unexpected references fail safely. IDs continue to avoid stale links
-- accidentally referring to newly created trips/tickets after a reset.
TRUNCATE TABLE seat_holds, payments, tickets, invoices, schedule_occurrences,
  trip_stops, template_stops, trip_schedules, stop_templates, stop_locations,
  trips, ticket_prices, vehicles, drivers, routes, vehicle_types, bus_companies
  CONTINUE IDENTITY;

INSERT INTO bus_companies(company_name, phone, email) VALUES
 ('Nhà xe Phương Trang', '0907000001', 'demo-pt@example.test'),
 ('Nhà xe Thành Bưởi', '0907000002', 'demo-tb@example.test');
INSERT INTO vehicle_types(type_name, seat_capacity) VALUES
 ('Limousine 24 chỗ', 24), ('Ghế ngồi 40 chỗ', 40);

CREATE TEMP TABLE demo_corridors(
 code TEXT, destination TEXT, distance INTEGER, duration TIME,
 outward TIME, return_departure TIME, base_price INTEGER
) ON COMMIT DROP;
INSERT INTO demo_corridors VALUES
 ('VT', 'Vũng Tàu', 100, '02:00', '08:00', '12:00', 150000),
 ('DL', 'Đà Lạt', 300, '07:00', '06:00', '15:00', 280000),
 ('NT', 'Nha Trang', 430, '09:00', '04:00', '15:00', 350000);
INSERT INTO routes(departure, destination, distance, estimated_duration)
SELECT 'TP HCM', destination, distance, duration FROM demo_corridors
UNION ALL SELECT destination, 'TP HCM', distance, duration FROM demo_corridors;

CREATE TEMP TABLE demo_fleet ON COMMIT DROP AS
SELECT c.company_id, c.company_name, CASE WHEN c.email = 'demo-pt@example.test' THEN 'PT' ELSE 'TB' END company_code,
       vt.type_id, d.*, row_number() OVER (ORDER BY c.company_id, d.code)::INTEGER fleet_no
FROM bus_companies c CROSS JOIN demo_corridors d
JOIN vehicle_types vt ON vt.seat_capacity =
  CASE WHEN c.email = 'demo-pt@example.test' THEN 24 ELSE 40 END;

INSERT INTO vehicles(company_id, type_id, license_plate)
SELECT company_id, type_id, '51B-310.' || lpad(fleet_no::TEXT, 2, '0') FROM demo_fleet;
-- A separate driver for each direction, with a dedicated vehicle per round trip.
INSERT INTO drivers(full_name, date_of_birth, citizen_id, gender, phone, address, license_number, issue_date)
SELECT (ARRAY['Nguyễn Văn Hùng', 'Trần Minh Tuấn', 'Lê Quốc Bảo', 'Phạm Thanh Sơn',
              'Võ Văn Hải', 'Đặng Minh Đức', 'Bùi Quốc Huy', 'Đỗ Anh Dũng',
              'Hồ Văn Phúc', 'Ngô Thanh Tùng', 'Dương Văn Long', 'Phan Minh Quân'])[f.fleet_no * 2 + direction - 1],
       DATE '1985-01-01', '900000' || lpad((f.fleet_no * 2 + direction)::TEXT, 6, '0'), TRUE,
       '0908' || lpad((f.fleet_no * 2 + direction)::TEXT, 6, '0'), 'Địa chỉ mô phỏng', 'E', DATE '2010-01-01'
FROM demo_fleet f CROSS JOIN generate_series(0, 1) direction;

INSERT INTO ticket_prices(company_id, type_id, route_id, price)
SELECT f.company_id, f.type_id, r.route_id,
       f.base_price + CASE WHEN f.company_code = 'PT' THEN 50000 ELSE 0 END
FROM demo_fleet f JOIN routes r ON
 (r.departure = 'TP HCM' AND r.destination = f.destination)
 OR (r.destination = 'TP HCM' AND r.departure = f.destination);

INSERT INTO trip_schedules(name, vehicle_id, route_id, driver_id, departure_time,
 weekdays, valid_from, valid_to, is_active, excluded_dates)
SELECT regexp_replace(f.company_name, '^Nhà xe ', '') || ' | ' || r.departure || ' → ' || r.destination
       || ' | ' || to_char(CASE WHEN direction = 0 THEN f.outward ELSE f.return_departure END, 'HH24:MI'),
       v.vehicle_id, r.route_id, d.driver_id,
       CASE WHEN direction = 0 THEN f.outward ELSE f.return_departure END,
       '1,2,3,4,5,6,7', CURRENT_DATE, CURRENT_DATE + 180, TRUE, ''
FROM demo_fleet f CROSS JOIN generate_series(0, 1) direction
JOIN vehicles v ON v.license_plate = '51B-310.' || lpad(f.fleet_no::TEXT, 2, '0')
JOIN routes r ON r.departure = CASE WHEN direction = 0 THEN 'TP HCM' ELSE f.destination END
 AND r.destination = CASE WHEN direction = 0 THEN f.destination ELSE 'TP HCM' END
JOIN drivers d ON d.citizen_id = '900000' || lpad((f.fleet_no * 2 + direction)::TEXT, 6, '0');

-- Materialize the initial horizon atomically; subsequent replenishment uses the
-- existing application service (including locks and price validation).
CREATE TEMP TABLE demo_departures ON COMMIT DROP AS
SELECT s.schedule_id, s.vehicle_id, s.route_id, s.driver_id,
       CURRENT_DATE + day_offset AS service_date,
       (CURRENT_DATE + day_offset) + s.departure_time AS departure_time,
       r.estimated_duration, tp.price
FROM trip_schedules s JOIN routes r ON r.route_id = s.route_id
JOIN vehicles v ON v.vehicle_id = s.vehicle_id
JOIN ticket_prices tp ON tp.company_id = v.company_id AND tp.type_id = v.type_id AND tp.route_id = s.route_id
CROSS JOIN generate_series(0, 13) day_offset
WHERE (CURRENT_DATE + day_offset) + s.departure_time > LOCALTIMESTAMP;
INSERT INTO trips(vehicle_id, route_id, driver_id, departure_time, estimated_arrival_time, actual_price)
SELECT vehicle_id, route_id, driver_id, departure_time,
       departure_time + (estimated_duration - TIME '00:00'), price FROM demo_departures;
INSERT INTO schedule_occurrences(schedule_id, service_date, trip_id, status)
SELECT d.schedule_id, d.service_date, t.trip_id, 'CREATED'
FROM demo_departures d JOIN trips t ON t.vehicle_id = d.vehicle_id AND t.departure_time = d.departure_time
 AND t.route_id = d.route_id AND t.driver_id = d.driver_id;

DO $$
BEGIN
  IF EXISTS (SELECT 1 FROM trips a JOIN trips b ON a.trip_id < b.trip_id
    AND (a.vehicle_id = b.vehicle_id OR a.driver_id = b.driver_id)
    AND a.departure_time < b.estimated_arrival_time AND b.departure_time < a.estimated_arrival_time) THEN
    RAISE EXCEPTION 'Demo trips overlap; reset rolled back.';
  END IF;
  IF NOT EXISTS (SELECT 1 FROM trips) OR (SELECT count(*) FROM trip_schedules) <> 12
     OR (SELECT count(*) FROM trips) <> (SELECT count(*) FROM schedule_occurrences)
     OR EXISTS (SELECT 1 FROM trips WHERE actual_price IS NULL OR actual_price <= 0) THEN
    RAISE EXCEPTION 'Demo configuration is incomplete; reset rolled back.';
  END IF;
  IF EXISTS (SELECT 1 FROM preserved_counts p WHERE
       p.accounts <> (SELECT count(*) FROM accounts) OR p.customers <> (SELECT count(*) FROM customers)
       OR p.employees <> (SELECT count(*) FROM employees) OR p.roles <> (SELECT count(*) FROM roles)) THEN
    RAISE EXCEPTION 'User profiles changed; reset rolled back.';
  END IF;
END $$;

SELECT 'accounts_preserved' resource, count(*) FROM accounts
UNION ALL SELECT 'customers_preserved', count(*) FROM customers
UNION ALL SELECT 'companies', count(*) FROM bus_companies
UNION ALL SELECT 'vehicles', count(*) FROM vehicles
UNION ALL SELECT 'drivers', count(*) FROM drivers
UNION ALL SELECT 'routes', count(*) FROM routes
UNION ALL SELECT 'schedules', count(*) FROM trip_schedules
UNION ALL SELECT 'trips', count(*) FROM trips;
COMMIT;
