# CoachBooking — Architecture Audit

> **Branch:** `feat/redesign-ui`  
> **Scope:** Full-stack audit prior to the *The Night Window — Cinematic Twilight* UI redesign.  
> **Constraint:** Preserve backend APIs, database schema, authentication/RBAC, and core business logic.

---

## 1. Technology Stack

| Layer | Technology & Verified Version | Location |
| :--- | :--- | :--- |
| **Frontend** | React `18.3.1`, TypeScript `5.6.3`, Vite `5.4.9`, React Router DOM `6.26.2` | `frontend/` |
| **Styling** | Pure CSS with CSS Custom Properties (`styles.css` + per-page CSS modules/files; no Tailwind or UI kit) | `frontend/src/` |
| **Frontend Tests** | Node native test runner (`node --test tests/*.test.mjs`) | `frontend/tests/` |
| **Backend** | Java `25`, Spring Boot `3.5.16`, Spring Data JPA, Spring Security, OAuth2 Client, Validation, Mail (SMTP), Actuator, SpringDoc OpenAPI `2.8.17` | `backend/` |
| **Database** | PostgreSQL `16` (`postgres:16-alpine`), Flyway migrations `V1` → `V13` | `backend/src/main/resources/db/migration/` |
| **Infrastructure** | Docker Compose (`db`, `backend:8080`, `frontend:3000`, `frontend-admin:3001`), Nginx `/api/**` proxy | `docker-compose.yml`, `frontend/nginx.conf` |

---

## 2. System Architecture & Request Flow

```text
Browser (Customer :3000 | Admin :3001 | Vite Dev :5173)
   └──► /api/** (Nginx or Vite proxy)
          └──► Spring Boot (:8080)
                 ├── SecurityFilterChain (AuthOriginFilter → JwtAuthenticationFilter → @PreAuthorize)
                 ├── Controllers (21 REST controllers under com.coachbooking.web)
                 ├── Services (20 domain/operational services under com.coachbooking.service)
                 └── Repositories (22 Spring Data JPA repositories → PostgreSQL :5432)
```

### Dual-Portal Detection
Defined in `frontend/src/auth/portal.ts`:
- **Admin Portal:** `window.location.port === '3001'`, hostname starting with `admin`, or `?portal=admin`. `/` redirects to `/admin` (or `/login` when unauthenticated).
- **Customer Portal:** Default (`:3000` / `:5173`). `/` renders the public `Home` page.

---

## 3. Database & Flyway Migrations (`V1`–`V13`)

Entities use **flat JPA models with numeric foreign keys** (no broad `@ManyToOne` graphs) to avoid N+1 queries and circular serialization.

| Migration | Purpose |
| :--- | :--- |
| `V1__init_schema.sql` | Core tables: `roles`, `accounts`, `employees`, `customers`, `bus_companies`, `vehicle_types`, `vehicles`, `routes`, `drivers`, `ticket_prices`, `trips`, `invoices`, `tickets`, `payments`. |
| `V2__seed_data.sql` | Seed catalogs, fare tables, and 14-day trips. |
| `V3__add_seat_holds.sql` | Atomic 10-minute `seat_holds` table preventing double booking. |
| `V4__add_password_reset_tokens.sql`, `V5__add_reset_token_verification.sql` | 6-digit OTP password reset flow via SMTP (`password_reset_tokens`). |
| `V6__add_google_oauth2_support.sql` | `auth_provider` support for Google OAuth2 customer login. |
| `V7`–`V9` | Recurring trip schedules (`trip_schedules`, `schedule_occurrences`) and retirement of mandatory stop templates. |
| `V10__natural_transport_names.sql` | Natural demo transport names. |
| `V11__authentication_sessions.sql` | `auth_sessions` & `auth_refresh_tokens` for rotating refresh cookies and replay detection. |
| `V12__sepay_payments.sql`, `V13__sepay_local_polling.sql` | `sepay_sessions` & `sepay_notifications` for SePay sandbox IPN and local polling reconciliation. |

---

## 4. Authentication & Role-Based Authorization

- **Access Token:** Short-lived JWT (≤ 15 min), stored **in memory only** on the frontend (`AuthContext.tsx` / `api/client.ts`).
- **Refresh Token:** Opaque random token in `HttpOnly`, `SameSite=Lax` cookie (`coachbooking_refresh`, path `/api/auth`), rotated on every refresh with DB hash tracking and replay revocation.
- **CSRF Defense:** `AuthOriginFilter` enforces `X-Auth-Request: 1` and allowed `Origin` on `/api/auth/*login`, `/refresh`, `/logout`.
- **Session Limits:** `KhachHang` (7 days idle / 30 days absolute); `Admin` & `NhanVien` (12 hours fixed).
- **Roles:**
  - `Admin` (`roleId = 1`): Full system access, including `employees`, `accounts`, `roles`, and `trip-schedules`.
  - `NhanVien` (`roleId = 2`): Counter ticket sales, invoices, customers, read-only master catalogs, statistics.
  - `KhachHang` (`roleId = 3`): Self-service booking, seat holds, SePay checkout, own ticket history, cancellation of eligible unpaid tickets, profile.

---

## 5. Booking, Payment & Real-Time Architecture

1. **Public Search:** `GET /api/trips/search` returns upcoming trips with `actualPrice > 0`, seat capacity, and sold count.
2. **Real-Time Seat Map:** `GET /api/trips/{id}/seats` + SSE stream `GET /api/trips/{id}/seats/events` (`seat-status` event).
3. **Atomic Seat Hold:** `POST /api/seat-holds` locks requested seats for 10 minutes per account (`holdToken`).
4. **Customer Booking + SePay Sandbox:**
   - `POST /api/bookings` (with `holdToken` and `paymentMethod = "Chuyển khoản"`) creates an unpaid invoice and a `PENDING` `SepaySession`.
   - `SepayPaymentPage` submits a signed HTML POST form (`POST /api/payments/sepay/{sessionId}/checkout`) to SePay sandbox, polls status every 3s, and redirects to `/ve-dien-tu/{invoiceId}` upon `PAID`.
   - Customers cannot self-confirm payment via `/pay`. Cancellation of SePay invoices cancels the entire invoice and releases seats.
5. **Counter Sales (`AdminBooking`):** Staff/Admin select seats (cannot override active customer holds), create/reuse customer by phone, and accept `Tiền mặt`, `Chuyển khoản`, or `Thẻ ATM`.

---

## 6. Current UI Findings & Gaps

- **Visual Drift:** Current `styles.css` (2,824 lines) uses a bright *Royal Sapphire* palette (`#1d4ed8` / `#0f2b5c` / `#f8fafc`) alongside 11 separate page CSS files with hardcoded colors, radii, and shadows.
- **Duplicate Legacy Customer Routes:** `/app/dat-ve` (`CustomerBooking.tsx`) and `/app/lich-su` (`CustomerHistory.tsx`) duplicate `/tim-chuyen` and `/ve-cua-toi` inside `AppLayout`. Kept intact for backward compatibility.
- **Not Implemented (Do Not Fabricate):** MoMo payment gateway, dedicated SePay reconciliation UI (`REVIEW_REQUIRED` is DB-audited), server-side passenger-count filter on `/api/trips/search` (handled cleanly client-side as available-seat filter).

