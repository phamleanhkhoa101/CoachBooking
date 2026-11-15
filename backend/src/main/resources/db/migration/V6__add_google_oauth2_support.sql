ALTER TABLE accounts
    ALTER COLUMN password DROP NOT NULL;

ALTER TABLE accounts
    ADD COLUMN auth_provider VARCHAR(20) NOT NULL DEFAULT 'LOCAL',
    ADD COLUMN google_sub VARCHAR(100);

CREATE UNIQUE INDEX uq_accounts_google_sub
    ON accounts (google_sub)
    WHERE google_sub IS NOT NULL;
