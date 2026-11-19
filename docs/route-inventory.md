# CoachBooking — Route & Feature Inventory

> Separates **verified existing functionality** from **proposed redesign work**. No existing routes, API contracts, or role guards will be removed.

---

## 1. Public & Authentication Routes

| Route | Source File(s) | User Role | Verified Existing Functionality | Backend Dependencies | Proposed Redesign (*The Night Window*) | Priority |
| :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| `/` | `pages/customer/Home.tsx`, `Home.css` | Public (Admin portal redirects to `/admin` or `/login`) | Route search with swap & date picker, live routes/companies from DB, inline quick search results. | `GET /routes`, `GET /bus-companies`, `GET /trips/search` | Full cinematic twilight hero (coach window at dusk), italic `Playfair Display` headline, translucent dark-glass search bar with client-side passenger-seat filter, 4 USP strip, atmospheric popular route cards from real DB routes, 4-step horizon journey timeline. | **P0** |
| `/login` | `pages/auth/Login.tsx` | Public | Role-aware portal login (Customer vs Admin), remember-me, Google OAuth2 (customer portal), error/notice banners. | `POST /auth/user/login`, `POST /auth/admin/login`, `GET /oauth2/authorization/google` | Minimal Twilight backdrop (`#0B1521` → `#172638`) with crisp Soft Ivory/clean card, clear portal distinction, zero visual clutter. | **P0** |
| `/register` | `pages/auth/Register.tsx` | Public | Customer self-registration with username, email, and strong password validation. | `POST /auth/register` | Shared `AuthShell` layout with live password strength/requirement cues. | **P1** |
| `/forgot-password` | `pages/auth/ForgotPassword.tsx` | Public | 3-step SMTP OTP password reset (send OTP → verify OTP → reset password). | `POST /auth/forgot-password`, `/verify-reset-otp`, `/reset-password` | Compact 3-step indicator inside `AuthShell`, clean 6-digit OTP input styling. | **P1** |
| `/oauth2/callback` | `pages/auth/OAuth2Callback.tsx` | Public | Finalizes Google OAuth2 session via `/auth/refresh` and redirects to home. | `POST /auth/refresh` | Branded Twilight loading transition screen. | **P2** |

---

## 2. Customer Booking & Account Routes (`CustomerLayout`)

| Route | Source File(s) | User Role | Verified Existing Functionality | Backend Dependencies | Proposed Redesign (*The Night Window*) | Priority |
| :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| `/tim-chuyen` | `pages/customer/TripSearchPage.tsx`, `TripSearchPage.css` | Public | Search bar, 7-day quick date strip, company/vehicle-type/time filters, sort by time/price. | `GET /routes`, `GET /trips/search` | Soft Ivory (`#F5F2EE`) workspace, Midnight Navy compact search header, boarding-pass trip cards with `tabular-nums` pricing/times, clean filter sidebar. | **P0** |
| `/dat-ve/chon-ghe/:tripId` | `pages/customer/SeatSelectionPage.tsx`, `SeatSelectionPage.css` | `KhachHang` | 1-deck & 2-deck seat map, real-time SSE seat updates, atomic 10-min `SeatHold`. | `GET /trips/{id}/seats`, `SSE /seats/events`, `POST /seat-holds` | Focused 2-column layout on Soft Ivory: high-contrast seat legend (Available / Selected Warm Amber / Held / Sold) + sticky summary panel. | **P0** |
| `/dat-ve/thanh-toan` | `pages/customer/CheckoutPage.tsx`, `CheckoutPage.css` | `KhachHang` | 10-min hold countdown, profile autofill, passenger form, booking creation + SePay redirect. | `GET /profile`, `POST /bookings`, `DELETE /seat-holds/{token}` | Trustworthy 2-column checkout with Warm Amber countdown pill, clear price breakdown, and clean SePay handoff notice. | **P0** |
| `/thanh-toan/sepay/:sessionId` | `pages/customer/SepayPaymentPage.tsx`, `SepayPaymentPage.css` | `KhachHang` | Auto HTML POST to SePay sandbox, 3s polling, manual re-check, auto-redirect on `PAID`, terminal states (`EXPIRED`, `CANCELLED`, `REVIEW_REQUIRED`). | `GET/POST /payments/sepay/{id}`, `/checkout`, `/sync` | Calm, reassuring payment status card with clear state iconography and action hierarchy. | **P0** |
| `/ve-dien-tu/:invoiceId` | `pages/customer/ETicketPage.tsx`, `ETicketPage.css` | Public / Auth | Displays invoice, ticket seats, trip details, print action. | `GET /invoices/{id}` | Boarding-pass aesthetic: Midnight Navy header band, Soft Ivory ticket body, perforated divider, print-friendly CSS. | **P1** |
| `/tra-cuu-ve` | `pages/customer/TicketLookupPage.tsx`, `TicketLookupPage.css` | Public | Guest lookup by invoice ID + phone number. | `POST /invoices/lookup` | Compact Twilight hero header + focused lookup card + clean ticket result card. | **P1** |
| `/ve-cua-toi` | `pages/customer/CustomerTicketsPage.tsx`, `CustomerTicketsPage.css` | `KhachHang` | Filter tabs (All / Paid / Unpaid), resume SePay payment, cancel eligible unpaid invoices. | `GET /invoices/my`, `POST /invoices/{id}/cancel` | Calm Soft Ivory dashboard with clear status badges and scannable ticket cards. | **P1** |
| `/ho-so` | `pages/customer/CustomerProfilePage.tsx`, `CustomerProfilePage.css` | `KhachHang` | View/edit personal profile and change password. | `GET/PUT /profile`, `POST /auth/change-password` | Structured 2-section account page (Personal Info + Security) on Soft Ivory. | **P1** |
| `/app/dat-ve`, `/app/lich-su` | `pages/customer/CustomerBooking.tsx`, `CustomerHistory.tsx` | `KhachHang` | Legacy customer routes inside `AppLayout`. | `/trips/search`, `/bookings`, `/invoices/my` | Preserved for compatibility; automatically styled via redesigned `AppLayout` & tokens. | **P3** |

---

## 3. Admin & Staff Routes (`AppLayout` — `/admin/*`)

| Route | Source File(s) | User Role | Verified Existing Functionality | Backend Dependencies | Proposed Redesign (*The Night Window*) | Priority |
| :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| `/admin` | `pages/admin/AdminDashboard.tsx` | `Admin`, `NhanVien` | KPI summary cards, date range + quick presets, daily revenue chart, popular routes chart. | `GET /statistics` | High-density analytics dashboard: crisp KPI cards (`tabular-nums`), Navy/Amber bar visualizations, compact filter bar. | **P1** |
| `/admin/ban-ve` | `pages/admin/AdminBooking.tsx`, `pages/customer/Booking.tsx`, `SeatMap.tsx` | `Admin`, `NhanVien` | Counter ticket sales: search trips, pick seats, customer lookup/create by phone, pay via Cash / Bank Transfer / ATM. | `/routes`, `/trips/search`, `/trips/{id}/seats`, `POST /bookings` | High-efficiency split-view POS layout with clear seat status and quick payment controls. | **P1** |
| `/admin/hoa-don` | `pages/admin/Invoices.tsx` | `Admin`, `NhanVien` | Filter invoices by date/status, inspect tickets, collect counter payment (blocks SePay invoices), cancel unpaid invoices. | `GET /invoices`, `GET /invoices/{id}`, `/pay`, `/cancel` | High-density table + clean detail modal with explicit SePay status badges. | **P1** |
| `/admin/chuyen-xe` | `pages/admin/operations/Trips.tsx` | `Admin` (CRUD), `NhanVien` (read) | Trip CRUD with schedule conflict alerts (`/trip-conflicts`) and seat occupancy. | `/trips`, `/vehicles`, `/routes`, `/drivers`, `/trip-conflicts` | Clear conflict alert banner, occupancy progress indicators, dense tabular layout. | **P2** |
| `/admin/lich-chay` | `pages/admin/operations/TripSchedules.tsx` | `Admin` | Recurring schedule CRUD, date-range preview & generation. | `/trip-schedules`, `/preview`, `/generate` | Pill weekday selector, status-badged generation result table. | **P2** |
| `/admin/nha-xe`, `/loai-xe`, `/xe`, `/tuyen-duong`, `/gia-ve`, `/tai-xe` | `pages/admin/operations/*.tsx` via `CrudPage.tsx` | `Admin` (CRUD), `NhanVien` (read) | Master-data catalog CRUD (6 modules). | Respective `/api/{resource}` endpoints | Upgraded uniformly via redesigned `CrudPage`, `DataTable`, `Modal`, and `FormField`. | **P2** |
| `/admin/khach-hang` | `pages/admin/system/Customers.tsx` via `CrudPage.tsx` | `Admin`, `NhanVien` | Customer CRUD. | `/customers` | Upgraded uniformly via redesigned `CrudPage`. | **P2** |
| `/admin/nhan-vien`, `/admin/tai-khoan` | `pages/admin/system/Employees.tsx`, `Accounts.tsx` via `CrudPage.tsx` | `Admin` | Employee and Account management (including role assignment & active status). | `/employees`, `/accounts`, `/roles` | Upgraded uniformly via redesigned `CrudPage`. | **P2** |
| `/admin/ho-so` | `pages/admin/Profile.tsx` | `Admin`, `NhanVien` | Staff profile update & password change. | `GET/PUT /profile`, `POST /auth/change-password` | Structured 2-card profile & security layout matching Admin design tokens. | **P2** |

