# CoachBooking — Redesign Implementation Plan

> **Branch:** `feat/redesign-ui`  
> **Guiding Principle:** Redesign the entire frontend UI to **The Night Window — Cinematic Twilight** while preserving 100% of backend APIs, database schema, RBAC, SeatHold SSE, SePay sandbox flow, and counter sales workflows.

---

## Phase 0 — Documentation & Baseline (Completed First)
- Create `docs/architecture-audit.md`, `docs/route-inventory.md`, `docs/design-system.md`, `docs/redesign-plan.md`, and `docs/progress.md`.

## Phase 1 — Design System Foundation
1. Update `frontend/index.html` to load `Playfair Display` (italic 400/500/600) alongside `Be Vietnam Pro` & `Plus Jakarta Sans`, and update `theme-color` to `#0B1521`.
2. Create `frontend/src/styles/tokens.css` implementing the full **Night Window** token system and mapping legacy CSS variables (`--primary`, `--primary-strong`, `--accent`, `--bg`, `--surface`, `--border`, `--text`) so existing modules transition cleanly without breakage.
3. Update `frontend/src/styles.css` to import `styles/tokens.css` and modernize core UI primitives (buttons, badges, forms, modals, tables, alerts, toast, sidebar, topbar).
4. Refine `BrandLogo.tsx` to match the Warm Amber coach emblem on Midnight Navy.

## Phase 2 — Public Website & Customer Navigation
1. **Navigation (`CustomerNavbar.tsx` / `CustomerNavbar.css`):**
   - Support `variant="transparent"` (floating glass header over the Home hero) and `variant="solid"` (Midnight Navy header for inner booking/account pages).
   - Keep all real navigation targets (`Đặt vé` → `/tim-chuyen`, `Tra cứu vé` → `/tra-cuu-ve`, `Vé của tôi` → `/ve-cua-toi`), hotline support indicator, and Login/Account pill.
2. **Footer (`CustomerLayout.tsx` / `CustomerLayout.css`):**
   - Midnight Navy (`#0B1521`) editorial footer with Soft Ivory typography and Warm Amber accents.
3. **Home Page (`pages/customer/Home.tsx` / `Home.css`):**
   - **Hero ("The Night Window"):** Full-bleed twilight coach-window composition (CSS/SVG/art-directed atmospheric window frame + twilight mountain horizon), editorial headline *"Đi xa một chút."* (`Playfair Display` italic) + **"GẦN NHAU HƠN."** with Warm Amber brushstroke underline.
   - **Floating Glass Search Bar:** `Nơi đi`, swap button, `Nơi đến`, `Ngày đi`, `Số hành khách` (filters trips client-side by available seats), and Warm Amber `Tìm chuyến` button.
   - **4-Column USP Strip:** `Đặt vé tức thì`, `Đa dạng nhà xe`, `Vé điện tử`, `Hỗ trợ 24/7`.
   - **Popular Routes Showcase:** Real routes from `GET /api/routes` rendered as atmospheric destination photo cards (Đà Lạt, Nha Trang, Đà Nẵng, Quy Nhơn, Vũng Tàu, etc.) with italic serif destination titles, duration, and quick-search action.
   - **Storytelling Journey Timeline:** *"Một hành trình, nhiều hơn là di chuyển."* + 4-step horizon timeline (`Đặt vé` → `Lên xe` → `Thư giãn` → `Đến nơi`) + real bus-company partners strip.

## Phase 3 — Authentication Suite
1. Introduce reusable `AuthShell` wrapper (`components/layout/AuthShell.tsx`) with subtle Twilight gradient canvas and focused ivory/white card.
2. Redesign `Login.tsx`, `Register.tsx`, `ForgotPassword.tsx`, and `OAuth2Callback.tsx` while preserving dual-portal detection (`isAdminPortal()`), Google OAuth2 button, remember-me, and 3-step SMTP OTP reset.

## Phase 4 — Customer Booking Journey & Account
1. **BookingStepper:** Refined horizontal progress bar on Soft Ivory with Midnight Navy completed steps and Warm Amber active step.
2. **TripSearchPage:** Compact Midnight Navy search header, 7-day quick-date ribbon, clean filter sidebar, boarding-pass trip cards with `tabular-nums` prices & times, client-side passenger count filter when passed from Home.
3. **SeatSelectionPage:** Clean 2-column layout preserving SSE `seat-status` updates and atomic `POST /api/seat-holds`; unmistakable seat states (Available, Selected Amber, Held, Sold).
4. **CheckoutPage:** Focused 2-column checkout with Warm Amber hold-expiry countdown bar, profile autofill, and SePay booking confirmation.
5. **SepayPaymentPage:** Calm transition & status card for all phases (`opening`, `confirming`, `paid`, `delayed`, `error`, `terminal`).
6. **ETicketPage:** High-contrast boarding-pass ticket layout with print styles.
7. **TicketLookupPage, CustomerTicketsPage, CustomerProfilePage:** Consistent Soft Ivory surfaces, clear tabs/badges, and view/edit separation.

## Phase 5 — Admin Dashboard & Management Modules
1. **AppLayout:** Midnight Navy `#0B1521` sidebar with Warm Amber active indicator, clean Soft Ivory topbar and workspace canvas.
2. **AdminDashboard:** Crisp KPI stat cards (`tabular-nums`), Navy/Amber daily revenue & popular route visualizations, quick date presets.
3. **DataTable & CrudPage:** High-density tabular layout, refined search/filter toolbar, clean modals — automatically upgrading all 9 catalog & system screens (`BusCompanies`, `VehicleTypes`, `Vehicles`, `Routes`, `Prices`, `Drivers`, `Customers`, `Employees`, `Accounts`).
4. **AdminBooking + SeatMap, Invoices, Trips, TripSchedules, Profile:** Align specialized operational screens to the same high-density Admin tokens while preserving all business rules (conflict warnings, SePay payment lockouts, counter payment methods).

## Phase 6 — Verification & Polish
- Run `cd frontend && npm test` and `cd frontend && npm run build`.
- Audit responsive breakpoints (desktop, tablet, mobile), keyboard focus, color contrast, and zero API/RBAC regressions.
- Update `docs/progress.md` with exact verification outputs.

