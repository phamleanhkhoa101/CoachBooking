CREATE TABLE sepay_sessions (
    session_id UUID PRIMARY KEY,
    invoice_id INTEGER NOT NULL UNIQUE REFERENCES invoices(invoice_id),
    account_id INTEGER NOT NULL REFERENCES accounts(account_id),
    trip_id INTEGER NOT NULL REFERENCES trips(trip_id),
    hold_token UUID NOT NULL UNIQUE,
    seat_numbers VARCHAR(2000) NOT NULL,
    ticket_count INTEGER NOT NULL CHECK (ticket_count > 0),
    amount NUMERIC(18,2) NOT NULL CHECK (amount > 0),
    reference VARCHAR(100) NOT NULL UNIQUE,
    status VARCHAR(30) NOT NULL CHECK (status IN ('PENDING','PAID','EXPIRED','CANCELLED','REVIEW_REQUIRED')),
    provider_transaction_id VARCHAR(128) UNIQUE,
    expires_at TIMESTAMP WITH TIME ZONE NOT NULL
);
CREATE INDEX idx_sepay_pending_expiry ON sepay_sessions(status, expires_at);

-- Selected reconciliation fields only; never persist keys or customer/card payloads.
CREATE TABLE sepay_notifications (
    event_key VARCHAR(64) PRIMARY KEY,
    session_id UUID REFERENCES sepay_sessions(session_id),
    reference VARCHAR(100) NOT NULL,
    provider_transaction_id VARCHAR(128) NOT NULL,
    notification_type VARCHAR(50) NOT NULL,
    amount NUMERIC(18,2),
    currency VARCHAR(10),
    outcome VARCHAR(50) NOT NULL,
    received_at TIMESTAMP WITH TIME ZONE NOT NULL
);
