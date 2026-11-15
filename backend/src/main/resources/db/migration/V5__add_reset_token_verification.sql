ALTER TABLE password_reset_tokens
    ADD COLUMN verified_at TIMESTAMP,
    ADD COLUMN reset_token_hash VARCHAR(64);

CREATE INDEX idx_prt_reset_token_hash ON password_reset_tokens (reset_token_hash);
