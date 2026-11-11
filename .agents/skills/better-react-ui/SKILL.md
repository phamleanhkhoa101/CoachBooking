---
name: better-react-ui
description: Best practices for implementing clean, accessible, robust React components, design token integration, and comprehensive UI states. Use when creating or refactoring React components, styling layouts, or adding interactive UI states in CoachBooking.
---

# Better React Web UI (CoachBooking Edition)

Guidelines for building professional, production-grade React components and layouts tailored to the CoachBooking codebase architecture.

## 1. Project Technology Context
- **Core Stack:** React 18, TypeScript, Vite, React Router v6.
- **Styling Architecture:** Pure modular CSS utilizing design tokens (CSS Variables) defined in `styles.css`.
- **CRITICAL RULE:** Do NOT use Tailwind CSS utility classes (Tailwind is not installed). Always leverage existing design tokens and clean semantic CSS class naming.

## 2. Design Token Integration
When styling or writing CSS, always utilize the established design system tokens in `frontend/src/styles.css`:
- **Colors:**
  - Background & Surface: `var(--bg)` (`#f5f7fb`), `var(--surface)` (`#ffffff`)
  - Text & Muted: `var(--text)` (`#101828`), `var(--muted)` (`#667085`)
  - Borders: `var(--border)` (`#e4e7ec`)
  - Brand Primary: `var(--primary)` (`#0f766e`), `var(--primary-strong)` (`#115e59`), `var(--primary-soft)` (`#ccfbf1`)
  - Semantic Status:
    - Success: `#067647`, bg `#ecfdf3`, border `#abefc6`
    - Warning: `var(--warning)` (`#d97706`), bg `#fffaeb`
    - Danger: `var(--danger)` (`#dc2626`), bg `#fef3f2`
    - Accent: `var(--indigo)` (`#4f46e5`)
- **Radii:** `var(--radius)` (`12px`), `var(--radius-sm)` (`9px`), `999px` for pill badges/chips.
- **Elevation:** `var(--shadow-sm)` (`0 1px 2px rgba(16, 24, 40, 0.05)`), `var(--shadow-md)`.

## 3. Comprehensive UI State Checklist
Every interactive component or data-driven page MUST gracefully handle all 4 core states:
1. **Loading State:**
   - Display a spinner or skeleton placeholder during asynchronous actions.
   - Disable submit buttons and show loading labels (e.g. `{loading ? 'Đang tải…' : 'Lưu thay đổi'}`).
2. **Empty State:**
   - When lists or tables have 0 items, never render blank white space.
   - Provide a helpful message and a call-to-action (CTA) to guide the user (e.g. `<div className="empty-state">...</div>`).
3. **Error State:**
   - Surface specific, actionable error messages (using `useToast` or inline error banners `.form-error-banner`).
   - Highlight invalid form fields and prevent broken state submissions.
4. **Success State:**
   - Provide immediate visual confirmation (Toast notifications, badges, or view mode transitions).

## 4. Component Structure & Architecture
- **Single Responsibility:** Keep components focused. Extract reusable pieces (e.g. `DataTable`, `FormField`, `Modal`, `Badge`) into `components/common/` or `components/table/`.
- **View vs. Edit Separation:** For account, customer, or profile details, default to a polished View Mode (displaying clean formatted labels and values) and only toggle into Edit Mode when the user explicitly triggers it (e.g., clicking "Thay đổi thông tin").
- **Uncontrolled vs. Controlled Inputs:** Keep form state clean and synchronized. Reset state cleanly on cancel.
- **Shared Utilities:** Always use `frontend/src/utils/format.ts` for consistent formatting:
  - Dates: `formatDate` (`dd/MM/yyyy`), `formatDateTime`
  - Currency: `formatVnd` (e.g. `1.540.000 đ`)
  - Numbers: `formatNumber`

## 5. Spacing, Rhythm & Typography
- **Spacing Grid:** Adhere to an 8px grid (4px, 8px, 12px, 16px, 20px, 24px, 32px).
- **Flexbox & Grid Layouts:** Always ensure flex items or grid children have `min-width: 0` to prevent overflow or breaking layout expansion.
- **Form Layouts:** Group related inputs with clear labels, explicit required indicators (`*`), and helper text where constraints exist.
- **Accessible Focus:** Ensure all interactive elements (buttons, inputs, links) support clear `:focus-visible` outlines.

## 6. Micro-Interactions & Transitions
- Add subtle transitions on interactive elements (`transition: background 0.15s, color 0.15s, border-color 0.15s`).
- Always disable buttons during background network requests (`disabled={busy || loading}`) to prevent duplicate clicks and race conditions.
- For buttons with icons, align them nicely with `display: inline-flex; align-items: center; gap: 6px;`.
