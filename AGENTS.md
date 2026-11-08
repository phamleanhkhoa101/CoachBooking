# AGENTS.md — CoachBooking

Instructions for AI coding agents working in this repository. Follow these rules for every task unless the user explicitly overrides a non-safety preference. Always inspect the current repository; documentation can be outdated.

## 1. Project Overview

CoachBooking is a web-based coach/bus ticket booking and management system for Bến xe Miền Đông. It is a migration of an earlier C# WinForms + EF6 + SQL Server application into a Java Spring Boot + React + PostgreSQL stack. **Do not assume the legacy C# application or its SQL Server schema/scripts are still part of the active system.**

Existing functionality described in `README.md` includes:
- Authentication, registration, and role-based access (admin, employee, customer).
- Master-data management, routes/trips, schedules, and fare tables.
- Trip search, seat availability, ticket booking (counter and customer flows).
- Invoices, supported payment methods, cancellation of eligible tickets.
- Booking history, profiles, revenue statistics, and popular routes.

Keep the scope aligned with the active web application. MoMo integration and real SMTP email delivery are **not currently implemented** according to the README; never imply otherwise or introduce them without an explicit request.

## 2. Technology and Directory Map

| Area | Technology / location |
| --- | --- |
| Backend | Java 25, Spring Boot 3.5, Spring Data JPA, Spring Security + JWT — `backend/` |
| Build | Gradle 9.1 wrapper — `backend/` |
| Frontend | React 18, TypeScript, Vite, React Router — `frontend/` |
| Database | PostgreSQL 16; Flyway — `backend/src/main/resources/db/migration/` |
| Infrastructure | Docker Compose — `docker-compose.yml`; frontend nginx in container |
| API contract | `docs/API.md` |
| Configuration | `.env.example`; runtime `.env` must not be committed |

Request path: React → `/api/**` → nginx proxy (container) / Vite proxy (development) → Spring Boot → JPA → PostgreSQL.

Before editing, verify actual packages, components, build scripts, configuration, endpoint paths, and conventions. Do not invent file names or dependencies.

## 3. Required Workflow for Agents

1. Read `README.md` and the relevant parts of `docs/API.md`.
2. Inspect related backend/frontend files, their callers, and relevant Flyway migrations.
3. Check `git status` and preserve all existing user changes.
4. When a problem or feature has multiple implementation approaches or technical trade-offs, present the options clearly (pros/cons) and ask the user to decide/agree on the approach before coding.
5. For non-trivial tasks, give a brief plan and identify affected files and data flows.
6. Make the smallest coherent change; reuse existing patterns.
7. Add or update relevant tests; run checks appropriate to touched code.
8. Review the diff for unintended modifications, security issues, and API/schema drift.
9. Explain results, tests run, remaining risks, and any manual verification steps.

For bug fixes, reproduce or characterize the failure before changing the implementation when practical. Do not claim tests or builds passed without actually running them.

## 4. Architecture and Coding Rules

### Backend (Spring Boot)

- Follow the repository's existing controller/service/repository structure.
- Keep HTTP transport and validation in controllers/DTO boundaries; keep business rules in services.
- Use Spring Data JPA repositories and existing query/projection patterns.
- This project intentionally uses flat JPA entities with foreign-key ID fields rather than broad `@ManyToOne` object graphs. **Preserve that approach** unless an architectural change is explicitly requested.
- Keep database table/column naming consistent with the existing `snake_case` convention.
- Preserve API status codes, DTO shapes, authentication requirements, and exception behavior unless the task explicitly changes the contract.
- Prefer dependency injection, clear names, narrow methods, explicit errors, and minimal new dependencies.
- Existing business logic previously implemented in database triggers was moved into services (including `TripService`, `BookingService`, and `InvoiceService`). Preserve that ownership; do not silently recreate triggers.

### Frontend (React + TypeScript)

- Follow existing folder structure, naming, routing, and styling conventions.
- Keep components focused and reuse established UI and API utilities.
- Use TypeScript types for API payloads and responses; keep them aligned with `docs/API.md` and backend DTOs.
- Handle loading, empty, success, validation, unauthorized, and server-error states as relevant.
- Maintain admin/employee/customer route guards and user-visible permissions, while remembering backend authorization is authoritative.
- Do not hardcode alternate API hosts when existing `/api` proxying is available.

## 5. Critical Booking and Payment Invariants

Treat seat inventory, reservation state, fares, invoices, and payments as high-risk business logic.

- **Prevent double booking.** Inspect the existing seat-availability and booking implementation before changing it. Protect against concurrent booking attempts using appropriate database constraints and transactional/locking strategies consistent with the project. A frontend seat check alone is insufficient.
- Validate requested trip, seat, user, and booking eligibility on the server. Never trust client-supplied prices or authorization claims.
- Preserve existing fare-calculation, invoice-total, and booking-state rules. Avoid duplicated calculations in unrelated layers.
- Use transactions for multi-step state transitions that must succeed or fail together. Consider rollback and retry behavior.
- Guard against duplicate submissions and unintended duplicate invoices/tickets. Use idempotency where the existing design requires it or an approved change introduces it.
- Preserve the documented restriction on cancellation of unpaid tickets, unless the specification is intentionally updated.
- Do not invent payment confirmations or add payment gateways. Supported payment options in the current README are cash, bank transfer, ATM card, and the customer-specific pay-later option.
- For changes to booking or payment logic, test at least: success, invalid input, unavailable seat, duplicate/concurrent request, unauthorized role, and transaction failure where applicable.

If a business rule is unclear, inspect current implementation and `docs/API.md`; clearly disclose uncertainty instead of silently inventing policy.

## 6. Database and Flyway

- Use new versioned Flyway migrations for schema/data changes; do not edit already-applied `V1__init_schema.sql`, `V2__seed_data.sql`, or any previously shipped migration.
- Preserve primary keys, foreign keys, unique constraints, and relationships.
- Review migration impact on seeded demo data and existing deployments.
- Do not use `ddl-auto` schema mutation as a replacement for Flyway.
- Never run destructive DB commands, `docker compose down -v`, `DROP DATABASE`, bulk deletes, or data resets without explicit permission.
- Do not reuse legacy SQL Server reset scripts; they are not part of the active PostgreSQL workflow.
- Backward-incompatible changes require an explicit migration/rollout explanation.

## 7. Authentication and Security

- Keep Spring Security, JWT validation, role checks, and `@PreAuthorize` protections intact.
- Passwords must be stored using the existing BCrypt approach; never log plaintext passwords or JWT secrets.
- Frontend route guards are UX controls, **not** a substitute for backend authorization.
- Never hardcode or commit production credentials, signing keys, database passwords, or `.env` contents.
- Validate request inputs and avoid exposing private customer data in API responses, logs, or error messages.
- The README documents a **demo-only** password-reset behavior that returns a temporary password in the response. Treat it as a security limitation, not as a production-safe design. Do not expand or expose it further; flag the risk when touching authentication.
- Treat demo accounts/credentials as local testing fixtures only, not production credentials.

## 8. API Contracts and Documentation

- `docs/API.md` is the documented API contract. Read it before changing endpoints.
- When changing API paths, methods, request/response fields, validation, or error semantics, update backend code, frontend consumers, and `docs/API.md` together.
- Avoid breaking existing frontend-backend integration without a clear migration plan.
- Keep README run instructions current when setup commands, environment variables, or ports change.

## 9. Build, Run, and Verification

From repository root, Docker development setup (consult `README.md` first):

```bash
# Create .env locally based on .env.example before running.
docker compose up --build
```

Expected documented local ports:
- Docker frontend: `http://localhost:3000`
- Backend API: `http://localhost:8080/api`
- Health: `http://localhost:8080/actuator/health`
- PostgreSQL: `localhost:5432`
- Vite development frontend: `http://localhost:5173`

Local backend (JDK 25 required):

```bash
cd backend
./gradlew bootRun      # Windows PowerShell/CMD: .\gradlew.bat bootRun
./gradlew test
./gradlew clean bootJar
```

Local frontend (confirm scripts in `frontend/package.json`):

```bash
cd frontend
npm install
npm run dev
npm run build
```

The README warns that `backend/gradle/wrapper/gradle-wrapper.jar` may be absent. If wrapper execution fails for this reason, follow the documented wrapper-generation procedure or use the documented Docker build; do not assume the wrapper is available. Prefer existing lockfile-consistent installation commands when possible.

For every change:
- Run focused checks first, then affected module build/tests if the environment permits.
- If business tests are absent, add focused regression tests when practical; the README identifies automatic business tests as an outstanding gap.
- Report exact commands executed and their outcomes, including environment-related failures. Never fake verification.

## 10. Git and Change Safety

- Do not commit, push, force-push, merge, rebase, or switch branches unless specifically requested.
- Never run `git reset --hard`, `git clean -fd`, or other destructive cleanup without explicit approval.
- Do not overwrite uncommitted work or mass-format unrelated files.
- Do not alter production infrastructure or secrets without authorization.
- Do not add libraries, redesign modules, or restructure the repository for a narrow task without a demonstrated need.
- Avoid unrelated refactoring and do not delete tests merely to make a build pass.

## 11. Communication and Learning Support

- Write explanations and completion summaries in **Vietnamese**, unless the user requests another language.
- For meaningful Java/Spring Boot changes, briefly explain the request flow (controller → service → repository), relevant transaction/security concepts, and the reasoning behind the solution.
- Prefer clear, maintainable code rather than overly clever abstractions.
- Highlight one or two important methods/classes the developer should review manually, especially for concurrency or business-rule changes.
- **Hỏi và chốt phương án trước khi code (Consult before implementing):** Khi một bài toán có nhiều hướng tiếp cận (về kiến trúc, giải thuật, thư viện, hoặc luồng UX/giao diện), agent phải phân tích ngắn gọn ưu/nhược điểm của từng cách và hỏi người dùng để thống nhất hướng xử lý trước khi thay đổi mã nguồn.
- For substantial changes, explain technical trade-offs before implementing them; for small fixes, stay concise.

## 12. Definition of Done / Final Report

A task is ready to report when:
- The requested behavior is implemented without unexplained scope changes.
- Relevant booking, auth, database, and API invariants remain intact.
- Tests/build checks were executed where possible; gaps are disclosed.
- Related docs/API contracts are updated if necessary.
- The diff contains no unrelated changes or exposed secrets.

Use this final response format:
1. **Đã thay đổi:** concise summary and files touched.
2. **Giải thích:** key implementation and important Java/Spring concepts.
3. **Kiểm thử:** exact commands, results, and tests not run.
4. **Lưu ý:** known limitations, risks, or required manual checks.
