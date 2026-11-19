# CoachBooking — Redesign Progress Tracker

> **Branch:** `feat/redesign-ui`  
> **Design System:** *The Night Window — Cinematic Twilight* (`Playfair Display` + `Be Vietnam Pro` / `Plus Jakarta Sans`)

---

## Status Overview

| Phase | Scope | Status | Notes |
| :--- | :--- | :---: | :--- |
| **Phase 0** | Documentation (`architecture-audit`, `route-inventory`, `design-system`, `redesign-plan`, `progress`) | ✅ Done | All 5 docs created and verified against codebase |
| **Phase 1** | Design System Foundation (`index.html`, `styles/tokens.css`, `styles.css`, `BrandLogo`) | ✅ Done | Loaded `Playfair Display` + `Be Vietnam Pro` / `Plus Jakarta Sans`, implemented Night Window tokens, updated sidebar, buttons & auth canvas |
| **Phase 2** | Public Website (`CustomerNavbar`, `CustomerLayout`, `Home`, twilight assets) | ✅ Done | Built cinematic Night Window hero, glass search bar with passenger filter, 4 USP strip, real-DB popular route cards, 4-step horizon timeline |
| **Phase 3** | Authentication (`AuthShell`, `Login`, `Register`, `ForgotPassword`, `OAuth2Callback`) | ✅ Done | Unified AuthShell, removed legacy Royal Blue auth CSS, added password strength on Register and Twilight OAuth2Callback |
| **Phase 4** | Customer Booking & Account (`BookingStepper`, `TripSearch`, `SeatSelection`, `Checkout`, `SepayPayment`, `ETicket`, `TicketLookup`, `CustomerTickets`, `CustomerProfile`) | ✅ Done | Removed all legacy Royal Blue colors; Soft Ivory canvas, Warm Amber seat selection & CTAs, Midnight Navy boarding pass & lookup hero |
| **Phase 5** | Admin & Operations (`AppLayout`, `AdminDashboard`, `CrudPage`/`DataTable`, `AdminBooking`/`SeatMap`, `Invoices`, `Trips`, `TripSchedules`, `Profile`) | ✅ Done | Midnight Navy sidebar & avatar, Night Window dashboard charts & KPI tabular-nums, Warm Amber POS seat selection, TripSchedules toolbar & StatusBadge |
| **Phase 6** | Verification (`npm test`, `npm run build`, legacy color audit, walkthrough) | ✅ Done | 32/32 tests pass, Vite production build clean, zero Royal Sapphire rules remaining |

---

## Changelog

- **Phase 0:** Audited full repository (React 18 + TS + Vite + pure CSS, Spring Boot 3.5 + Java 25, PostgreSQL 16 + Flyway V1–V13, dual-portal 3000/3001, JWT+HttpOnly refresh sessions, SSE seat updates, SePay sandbox IPN/Polling). Created all 5 planning & architecture documents in `docs/`.
- **Phase 1:** Added `Playfair Display` italic + `Be Vietnam Pro` / `Plus Jakarta Sans` in `frontend/index.html`, created `frontend/src/styles/tokens.css` with **The Night Window — Cinematic Twilight** palette (`#0B1521`, `#172638`, `#30445B`, `#F2A34B`, `#F5F2EE`), wired tokens into `main.tsx` and `styles.css`, updated `BrandLogo.tsx` to Midnight Navy & Warm Amber.
- **Phase 2:** Added custom twilight photography (`frontend/public/images/hero-night-window.jpg`, `dest-dalat.jpg`, `dest-nhatrang.jpg`, `dest-danang.jpg`, `dest-quynhon.jpg`). Upgraded `CustomerNavbar` (`transparent` & `solid` variants), `CustomerLayout` footer, and redesigned `Home.tsx` / `Home.css` preserving real `/api/routes` & `/api/bus-companies` integration; polished search bar contrast/alignment, fixed USP/timeline icon CSS specificity, removed quick-date strip on `TripSearchPage`, and fixed `:3001/admin` redirect loop for customer sessions.
- **Phase 3:** Created reusable `AuthShell` component, purged legacy Royal Blue auth CSS from `styles.css`, and unified `Login`, `Register` (with password strength meter), `ForgotPassword` (3-step OTP), and `OAuth2Callback`.
- **Phase 4–6:** Upgraded `BookingStepper`, `SeatSelectionPage` (Warm Amber active seat), `SepayPaymentPage` (calm Soft Ivory status card), `ETicketPage` (Midnight Navy boarding pass), `TicketLookupPage` (Twilight hero), `CustomerProfilePage`, Admin Dashboard charts & KPIs (`tabular-nums`), Counter POS `SeatMap`, topbar avatar, and `TripSchedules`. Verified with `npm test` (32/32 pass) and `npm run build`.
