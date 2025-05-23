# DarwinCMS Developer Guide

## 📁 Solution Structure

### Core Projects (under `src/`)

- **DarwinCMS.Domain**
  - Contains domain entities, enums, value objects, and interfaces.
  - Example: `User`, `Role`, `ContentItem`, `Email`, `Slug`.

- **DarwinCMS.Application**
  - Application services, abstractions (repositories), DTOs.
  - Follow `Services/{Module}` and `DTOs/{Module}` convention.

- **DarwinCMS.Infrastructure**
  - Implements repositories, services, EF Core, entity configurations.
  - Includes `DarwinDbContext`, Seeders, Service registration.

- **DarwinCMS.Web**
  - Public-facing frontend for general users (e.g. blog, e-commerce).
  - Not yet implemented.

- **DarwinCMS.WebAdmin**
  - Admin panel for managing content, users, roles, permissions.
  - Uses Bootstrap (not Telerik), with support for areas.
  - Path: `Areas/Admin/Views/...`

- **DarwinCMS.Shared**
  - Shared helpers (e.g. `PasswordHasher.cs`), common utilities.

- **DarwinCMS.Worker**, **DarwinCMS.Tools**, **DarwinCMS.WebApi**
  - Future extensions: background jobs, CLI tools, APIs for external access.

- **Modules**
  - External pluggable modules (e.g. `DarwinCMS.Module.Blog`).
  - Must follow naming: `DarwinCMS.Module.*`

---

### Test Projects (under `tests/`)

- **DarwinCMS.UnitTests**
  - Follows structure: `Application/Services/{ServiceName}Tests.cs`

- **DarwinCMS.IntegrationTests**
  - Integration tests using `WebApplicationFactory`.
  - Issue: Missing `testhost.deps.json` → Fix later.

---

## ✅ Development Rules

- **Clean Architecture** (modular + layered separation)
- **No FluentValidation** unless explicitly requested.
- **All classes, methods, and properties** must have XML or C# `///` comments.
- **English for all comments**, short Persian inline allowed only if helpful.
- Always use latest .NET SDK (`net9.0`, `sdk: 9.0.203`).
- `Directory.Build.props` ensures versioning, warnings, and paths are enforced.

## 🔌 ModuleLoader.cs

File: `DarwinCMS.WebAdmin.Infrastructure.Modules.ModuleLoader.cs`

- Dynamically loads all modules (DarwinCMS.Module.*)
- Registers their assemblies with MVC
- Optionally adds embedded views or static files
- Used in `Program.cs` like:

```csharp
ModuleLoader.RegisterModules(services, mvcBuilder, environment.WebRootPath);
```

## 🧪 Testing

- Use `FluentAssertions`, `Moq`, and `ITestOutputHelper`.
- Tests must be deterministic and cover success/failure edge cases.

## 🧱 Areas

- Admin UI uses `Areas/Admin` structure.
- Uses `_AdminLayout.cshtml`, and navigation uses Font Awesome icons.
- Pages like Dashboard, User/Role management follow Bootstrap layout.

## 🌍 Localization

- Three default cultures: `en`, `de`, `fa`.
- Stored in `Resources` folder.

## ✅ Summary

DarwinCMS is modular, developer-friendly, and future-proof. You can extend it easily by:
- Adding new services/modules without modifying core
- Keeping a consistent structure across services, UI, and repositories

For help, always consult this file or the core team.