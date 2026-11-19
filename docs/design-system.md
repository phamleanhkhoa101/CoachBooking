# CoachBooking — Global Design System

> **Art Direction:** **THE NIGHT WINDOW — CINEMATIC TWILIGHT**  
> **Primary Visual Reference:** Approved twilight coach-window landing screen.  
> **Architecture:** Pure CSS custom properties (`frontend/src/styles/tokens.css`), zero external CSS framework dependencies.

---

## 1. Color Identity

### Core Brand Palette
| Token | Hex | Role |
| :--- | :--- | :--- |
| `--nw-midnight` | `#0B1521` | Cinematic dark canvas, Admin sidebar, primary ink on light surfaces |
| `--nw-deep-navy` | `#172638` | Elevated dark surface, glass search bar base, booking/lookup headers |
| `--nw-slate-blue` | `#30445B` | Secondary dark borders/surfaces, muted navy labels, secondary controls |
| `--nw-amber` | `#F2A34B` | Primary CTA (`Tìm chuyến`, `Đặt vé`), active seat highlight, signature warm accents |
| `--nw-amber-hover` | `#E08E32` | Hover/pressed state for Warm Amber CTAs |
| `--nw-ivory` | `#F5F2EE` | Primary text on dark surfaces; calm warm canvas for Booking, Dashboard & Admin |

### Light-Surface Neutrals (Booking, Dashboard & Admin)
| Token | Hex | Role |
| :--- | :--- | :--- |
| `--surface` | `#FFFFFF` | Primary cards, tables, modals, form containers |
| `--surface-alt` | `#EFECE6` | Subtle table headers, secondary panels, disabled inputs |
| `--border` | `#DFD9CE` | Refined warm hairline borders |
| `--border-strong` | `#C7BFB1` | Input borders, interactive outlines |
| `--text` | `#0B1521` | Primary text (Midnight Navy) |
| `--text-secondary` | `#30445B` | Secondary text (Slate Blue) |
| `--muted` | `#5E6D7E` | Captions, metadata, placeholders |

### Semantic Status Colors
| Status | Foreground | Background | Border | Usage |
| :--- | :--- | :--- | :--- | :--- |
| **Success** | `#047857` | `#ECFDF5` | `#A7F3D0` | Paid invoices, active records, available seats |
| **Warning** | `#B45309` | `#FFFBEB` | `#FDE68A` | Pending payment, active seat hold countdown, review required |
| **Danger** | `#B91C1C` | `#FEF2F2` | `#FECACA` | Errors, sold seats, cancelled invoices, schedule conflicts |
| **Info** | `#0369A1` | `#F0F9FF` | `#BAE6FD` | Informational notices, SSE live sync indicators |

---

## 2. Typography

Loaded via Google Fonts in `frontend/index.html` (full Vietnamese diacritic support):
- **Editorial Serif (`--font-editorial`):** `'Playfair Display', Georgia, serif` — used **italic** for cinematic storytelling headlines (*"Đi xa một chút."*, *"Một hành trình, nhiều hơn là di chuyển."*) and destination titles on route showcase cards.
- **UI Sans-Serif (`--font-sans`):** `'Be Vietnam Pro', 'Plus Jakarta Sans', system-ui, sans-serif` — used across navigation, forms, buttons, tables, and admin controls.
- **Data Typography:** `font-variant-numeric: tabular-nums` applied to all VND prices, departure/arrival times, seat numbers, countdown timers, and KPI figures.

---

## 3. Five-Zone Surface Strategy

> **Rule:** Never overuse cinematic photography across transactional or operational screens.

| Application Zone | Routes | Surface Treatment | Typography & Density |
| :--- | :--- | :--- | :--- |
| **1. Public Website** | `/` | Full-bleed Midnight Navy `#0B1521`, twilight coach-window hero, glass search bar, atmospheric route cards, horizon timeline. | Editorial `Playfair Display` italic + bold sans-serif; spacious rhythm. |
| **2. Authentication** | `/login`, `/register`, `/forgot-password`, `/oauth2/callback` | Subtle twilight gradient (`#0B1521` → `#172638`) with centered clean Soft Ivory/White card. | Minimal, calm, high-focus inputs; clear portal indicator. |
| **3. Customer Booking** | `/tim-chuyen`, `/dat-ve/*`, `/thanh-toan/*`, `/ve-dien-tu/*`, `/tra-cuu-ve` | Soft Ivory `#F5F2EE` canvas + `#FFFFFF` cards + `#0B1521` compact header & stepper. | Clean sans-serif + `tabular-nums`; high legibility for times, prices & seats. |
| **4. Customer Account** | `/ve-cua-toi`, `/ho-so` | Soft Ivory `#F5F2EE` canvas + structured white cards, calm spacing. | Clear status badges, scannable booking history. |
| **5. Admin & Staff** | `/admin/*` | Fixed Midnight Navy `#0B1521` sidebar + Soft Ivory `#F5F2EE` workspace + `#FFFFFF` dense data tables. | High information density, compact 8px grid, tabular KPIs. |

---

## 4. Spacing, Radii, Elevation & Accessibility

- **Spacing Scale:** Strict 8px grid (`4px`, `8px`, `12px`, `16px`, `20px`, `24px`, `32px`, `48px`).
- **Radii:** `6px` (`--radius-xs` inputs/badges), `10px` (`--radius-sm` buttons), `14px` (`--radius` cards), `20px` (`--radius-lg` hero search/modals), `9999px` (pills).
- **Focus & Motion:** High-contrast `:focus-visible` outline (`2px solid var(--nw-amber)` on dark, `2px solid var(--nw-midnight)` on light); respect `prefers-reduced-motion`.
- **UI States:** Every async view provides explicit Loading, Empty, Error, and Success states.

