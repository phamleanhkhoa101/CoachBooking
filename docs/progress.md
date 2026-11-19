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
| **Phase 3** | Authentication (`Login`, `Register`, `ForgotPassword`, `OAuth2Callback`) | 🔄 Next | Twilight backdrop applied; next refine form states & OTP stepper |
| **Phase 4** | Customer Booking & Account (`TripSearch`, `SeatSelection`, `Checkout`, `SepayPayment`, `ETicket`, `TicketLookup`, `CustomerTickets`, `CustomerProfile`) | ⏳ Pending | Soft Ivory trustworthy booking & calm account views |
| **Phase 5** | Admin & Operations (`AppLayout`, `AdminDashboard`, `CrudPage`/`DataTable`, `AdminBooking`, `Invoices`, `Trips`, `TripSchedules`, `Profile`) | ⏳ Pending | Midnight Navy sidebar done; next refine high-density dashboard & tables |
| **Phase 6** | Verification (`npm test`, `npm run build`, responsive & contrast audit) | ⏳ Pending | Final sign-off |

---

## Changelog

- **Phase 0:** Audited full repository (React 18 + TS + Vite + pure CSS, Spring Boot 3.5 + Java 25, PostgreSQL 16 + Flyway V1–V13, dual-portal 3000/3001, JWT+HttpOnly refresh sessions, SSE seat updates, SePay sandbox IPN/Polling). Created all 5 planning & architecture documents in `docs/`.
- **Phase 1:** Added `Playfair Display` italic + `Be Vietnam Pro` / `Plus Jakarta Sans` in `frontend/index.html`, created `frontend/src/styles/tokens.css` with **The Night Window — Cinematic Twilight** palette (`#0B1521`, `#172638`, `#30445B`, `#F2A34B`, `#F5F2EE`), wired tokens into `main.tsx` and `styles.css`, updated `BrandLogo.tsx` to Midnight Navy & Warm Amber.
- **Phase 2:** Added custom twilight photography (`frontend/public/images/hero-night-window.jpg`, `dest-dalat.jpg`, `dest-nhatrang.jpg`, `dest-danang.jpg`, `dest-quynhon.jpg`). Upgraded `CustomerNavbar` (`transparent` & `solid` variants), `CustomerLayout` footer, and redesigned `Home.tsx` / `Home.css` preserving real `/api/routes` & `/api/bus-companies` integration plus client-side passenger-seat filtering into `TripSearchPage.tsx`.
