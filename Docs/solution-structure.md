# DarwinCMS Solution & Project Structure

DarwinCMS is a modular, cleanly architected CMS designed for extensibility, scalability, and educational value. The architecture below reflects the latest design decisions.

---

## ✅ Key Architectural Principles

- Clean Architecture + Modular Monolith
- Only free, open-source, and maintained packages
- Modular system: feature modules are added incrementally
- SQL Server in Docker; must support secrets and user auth
- Swagger/OpenAPI, versioning, CORS config must be included
- Webhooks supported; modules can communicate internally
- Final structure includes separate front-facing and admin-facing apps

---

## 📁 Solution Folder: `src/`

### 📦 `DarwinCMS.Domain` (Class Library)
Core entities, value objects, interfaces, and events (pure domain logic)

### 📦 `DarwinCMS.Application` (Class Library)
CQRS, services, DTOs, validation logic, interfaces to infrastructure

### 📦 `DarwinCMS.Infrastructure` (Class Library)
Implements application and domain interfaces (EF Core, Identity, Graph API, etc.)
- Includes `Persistence/`, `Repositories/`, `Migrations/`, `Services/`

---

### 📦 `DarwinCMS.Admin` (ASP.NET Core Web App – Razor Pages + Web API)
Admin web UI and site management panel

### 📦 `DarwinCMS.AdminApi` (ASP.NET Core Web API)
Admin API for UI and external integrations

---

### 📦 `DarwinCMS.Web` (ASP.NET Core Web App – Razor Pages)
Public-facing UI for visitors and customers

### 📦 `DarwinCMS.WebApi` (ASP.NET Core Web API)
Public-facing API for mobile apps, third-party integrations, etc.

All these use Web API Controllers (not Minimal APIs) and are fully CORS/versioning-enabled.

---

## 📁 Solution Folder: `modules/`

Each feature lives in its own project with the following layout:

```
📦 DarwinCMS.Modules.[ModuleName]
 ┣ 📁 Domain
 ┣ 📁 Application
 ┣ 📁 Infrastructure
 ┣ 📁 UI
 ┗ 📁 Api (Web API Controllers only)
```

Examples:
- `DarwinCMS.Modules.Content`
- `DarwinCMS.Modules.Users`
- `DarwinCMS.Modules.Navigation`
- `DarwinCMS.Modules.Comments` (planned)
- `DarwinCMS.Modules.Office365CRM` (stub)

---

## 📁 Solution Folder: `tests/`

All tests are separated in two projects:

- `DarwinCMS.UnitTests` – Tests for domain and application layers
- `DarwinCMS.IntegrationTests` – Infrastructure, persistence, and API

Use `xUnit`, `FluentAssertions`, `AutoFixture`, `Moq`, and `ITestOutputHelper`.

---

## 📁 Solution Folder: `tools/`

- `DarwinCMS.Tools` – CLI utilities (e.g., Seeder, Migrator)
- `DarwinCMS.Worker` – Background jobs, scheduled sync, outbox pattern

---

## 📁 Solution Folder: `docs/`

Markdown documentation for GitHub:
- `README.md`, `architecture.md`, `modules.md`, `PROJECT_NOTES.md`, etc.

---

## 🧭 Summary

- Final structure includes: Admin UI/API, Public UI/API
- Razor Pages and Web API Controllers used throughout
- No Minimal APIs
- Open-source tools only
- Fully modular and extensible
