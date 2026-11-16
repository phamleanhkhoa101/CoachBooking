ALTER TABLE sepay_sessions
    ADD COLUMN confirmation_mode VARCHAR(10) NOT NULL DEFAULT 'IPN'
        CHECK (confirmation_mode IN ('IPN', 'POLLING')),
    ADD COLUMN created_at TIMESTAMP WITH TIME ZONE,
    ADD COLUMN next_check_at TIMESTAMP WITH TIME ZONE,
    ADD COLUMN last_checked_at TIMESTAMP WITH TIME ZONE,
    ADD COLUMN last_check_error VARCHAR(200),
    ADD COLUMN sync_failures INTEGER NOT NULL DEFAULT 0,
    ADD COLUMN sync_lease_token UUID;

UPDATE sepay_sessions s SET created_at = i.created_at AT TIME ZONE 'Asia/Ho_Chi_Minh'
FROM invoices i WHERE i.invoice_id = s.invoice_id;
ALTER TABLE sepay_sessions ALTER COLUMN created_at SET NOT NULL;
CREATE INDEX idx_sepay_polling_due ON sepay_sessions(next_check_at, created_at)
    WHERE confirmation_mode = 'POLLING' AND status <> 'PAID';
ALTER TABLE sepay_notifications ADD COLUMN source VARCHAR(10) NOT NULL DEFAULT 'IPN'
    CHECK (source IN ('IPN', 'POLLING'));
