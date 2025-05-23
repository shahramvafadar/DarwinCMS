# DarwinCMS Solution & Project Structure

DarwinCMS is organized into clearly separated layers and modules that follow Clean Architecture principles. Below is a full breakdown of the solution folders, projects, their types, purposes, internal structure, and key packages used.

---

## 📁 Solution Folder: `src/`

Contains all the production (application) code.

---

### 📦 Project: `DarwinCMS.Domain` (Class Library)

**Purpose:**  
Contains the domain layer of the system – the core business logic, entities, value objects, domain events, interfaces, and custom exceptions. It has no dependencies on other projects or external libraries.

**Internal Structure:**
- `Entities/` – Domain entities and aggregates (e.g., Page, User, etc.)
- `ValueObjects/` – Immutable objects with equality by value (e.g., EmailAddress, PageSlug)
- `Interfaces/` – Repository and domain service contracts (e.g., IPageRepository)
- `Events/` – Domain event classes that signal something important has happened
- `DomainExceptions/` – Business rule validation exceptions

**Packages Used:**  
_None_ (no external dependencies to keep domain pure)

---

### 📦 Project: `DarwinCMS.Application` (Class Library)

**Purpose:**  
Encapsulates all use cases, command/query handlers (CQRS), DTOs, application services, validators, and interfaces for infrastructure concerns like email or file storage.

**Internal Structure:**
- `Interfaces/` – Contracts for external services (e.g., IEmailSender, IFileStorage)
- `Services/` – Coordinating services for higher-level logic
- `Commands/` – Write-side use cases (e.g., CreatePageCommand + Handler)
- `Queries/` – Read-side queries (e.g., GetPageByIdQuery + Handler)
- `DTOs/` – Data Transfer Objects between UI and application logic
- `Validators/` – FluentValidation-based input validators
- `Behaviors/` – MediatR pipeline behaviors (e.g., logging, performance)

**Packages Used:**
- `MediatR`
- `FluentValidation`
- `AutoMapper` (optional)

---

### 📦 Project: `DarwinCMS.Infrastructure` (Class Library)

**Purpose:**  
Implements interfaces defined in Domain and Application. Includes database access (EF Core), Identity, file storage, external integrations (e.g., Office 365), and configuration.

**Internal Structure:**
- `Persistence/`
  - `DarwinCmsDbContext.cs` – EF Core DbContext
  - `EntityConfigs/` – Fluent API configurations
  - `Migrations/` – EF Core migrations
  - `Repositories/` – Implementations of IPageRepository, etc.
- `Identity/` – ASP.NET Identity integration
- `Services/` – Implementations like SmtpEmailSender
- `Integration/` – Office 365 (Graph API), SharePoint, or CRM connectors
- `Configuration/` – Service registrations, IOptions binding

**Packages Used:**
- `Microsoft.EntityFrameworkCore` (+ SQLServer/Sqlite)
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Microsoft.Graph` (optional)
- `Serilog` or `ApplicationInsights` (optional logging)

---

### 📦 Project: `DarwinCMS.Web` (ASP.NET Core Web App)

**Purpose:**  
The entry point of the application. Handles the UI layer with Razor Pages and Telerik UI. Configures all DI, routing, and middleware.

**Internal Structure:**
- `Pages/` – Razor Pages and PageModel files
- `wwwroot/` – Static files (CSS, JS, images)
- `Controllers/` – Optional Web API Controllers
- `Models/` – ViewModels for Razor UI
- `Components/` – Partial views or TagHelpers
- `Program.cs` – App startup, DI, and middleware configuration
- `appsettings.json` – Configuration

**Packages Used:**
- `Microsoft.AspNetCore.Razor.Runtime`
- `Telerik.UI.for.AspNetCore`
- `Microsoft.AspNetCore.Authentication.*`
- `MediatR`
- `Swashbuckle.AspNetCore` (for Swagger, optional)

---

## 📁 Solution Folder: `tests/`

Contains all test projects for automated testing.

---

### 📦 Project: `DarwinCMS.Domain.Tests` (xUnit Test Project)

**Purpose:**  
Unit tests for domain logic and entities.

**Packages Used:**
- `xUnit`
- `FluentAssertions`
- `AutoFixture` (optional)

---

### 📦 Project: `DarwinCMS.Application.Tests` (xUnit Test Project)

**Purpose:**  
Tests for command/query handlers and validators.

**Packages Used:**
- `xUnit`
- `FluentAssertions`
- `AutoFixture`
- `Moq` or `NSubstitute`

---

### 📦 Project: `DarwinCMS.Infrastructure.Tests` (xUnit Test Project)

**Purpose:**  
Integration tests for EF Core repositories and services.

**Packages Used:**
- `Microsoft.EntityFrameworkCore.InMemory`
- `xUnit`

---

### 📦 Project: `DarwinCMS.Web.Tests` (xUnit Test Project)

**Purpose:**  
Web-level integration tests, possibly using TestServer or Selenium (optional).

**Packages Used:**
- `Microsoft.AspNetCore.Mvc.Testing`
- `xUnit`

---

## 📁 Solution Folder: `docs/`

Documentation files (Markdown) used in GitHub repo.

---

## 📁 Solution Folder: `tools/` (Planned)

For CLI tools or background workers:

- `DarwinCMS.Tools` – Console apps like database seeder, migration runner
- `DarwinCMS.Worker` – Background tasks processor (e.g. for outbox, cleanups)

---

This structured layout ensures that code is clean, modular, and each responsibility has a clear place. Every project and folder has a focused role, reducing complexity and making it easier to test, maintain, and scale.
