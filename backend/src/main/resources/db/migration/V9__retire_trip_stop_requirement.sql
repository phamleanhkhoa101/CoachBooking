-- Stop selection is retired. Keep historical stop snapshots and V7/V8 intact.
-- Existing schedules retain their historical template reference; new schedules
-- no longer require or use one. No trips, tickets or invoices are deleted.
ALTER TABLE trip_schedules ALTER COLUMN template_id DROP NOT NULL;
