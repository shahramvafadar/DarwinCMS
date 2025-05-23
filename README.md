# DarwinCMS – Modular, Cloud-Ready CMS for Modern .NET

Welcome to **DarwinCMS**, a future-ready, modular content management system built with .NET 9.0. It is designed as both a **real-world enterprise CMS** and an **educational platform** to demonstrate Clean Architecture, cloud-native principles, and scalable modular design.

> **Note:** This documentation is in active development. Some documents may still include draft notes or reminders intended for internal planning.

---

## 🚀 Vision
DarwinCMS is designed to power modern websites and applications with full support for modular features, multilingual content, Office 365 integration, and scalable cloud deployment. It also serves as a teaching tool for advanced software architecture, demonstrating:

- Clean Architecture
- Domain-Driven Design (DDD)
- Event-Driven Patterns
- Modular Monolith evolving toward Microservices
- Cloud-native best practices


## 🧱 Core Architecture
DarwinCMS follows a **Clean Architecture** structure:

- **Domain Layer**: Business entities, value objects, domain rules
- **Application Layer**: Services, use cases, DTOs, CQRS, validation
- **Infrastructure Layer**: EF Core, Identity, integrations, email, file storage
- **Presentation Layer**: Razor Pages (Admin & Web), Web API

Other principles and patterns include:

- CQRS with MediatR
- AutoMapper for DTO transformations
- Exception handling middleware
- Modular folder structure per feature
- Pluggable module discovery at runtime

> See: [`architecture.md`](docs/architecture.md)


## 🔌 Modules
Modules are implemented as independent .NET class libraries with subfolders for Domain, Application, Infrastructure, UI, and optionally API.

### ✅ Core Modules (MVP)
- **Content**: Pages, metadata, layouts
- **User Management**: Roles, claims, permissions, login
- **Navigation**: Dynamic menu rendering based on permissions

### 🔜 In Progress / Planned
- **Blog**: Posts, categories, tags, comments
- **Comments**: Threaded comments, moderation, notifications
- **Media Library**: Image/video upload & reuse
- **Search**: Site-wide filtering and boosting
- **Localization**: JSON-based multilingual UI
- **Analytics**: Page views, user actions
- **Notifications**: Email/SignalR/dashboard notifications
- **CRM / Office365 Integration** (see below)

### 📌 Additional Modules (Planned for Future)
> The list below is evolving and subject to refinement:

- **Form Builder**: Drag-and-drop UI for forms, surveys, lead capture
- **Workflow Engine**: Custom content workflows with approval logic
- **Audit Trail**: Track changes to content and user actions
- **API Tokens**: Issue & manage tokens for external integrations
- **Webhooks**: Trigger external systems on CMS events
- **AI-Based Translation**: Auto-translate content via LLM APIs
- **Sitemap & SEO**: SEO tools, dynamic sitemap, robots.txt support
- **A/B Testing**: Run experiments on content variations
- **E-Commerce**: Product catalog, cart, checkout, orders

> See: [`modules.md`](docs/modules.md)


## 🤝 Office 365 + CRM Integration
A separate module and project (`DarwinCMS.Modules.Office365CRM`) enables deep integration with Microsoft 365 tools such as:

- SharePoint Lists & Libraries (clients, quotes, documents)
- PowerApps UIs for internal workflows
- Power Automate flows for business automation
- Dynamics 365 CRM for quotes, companies, contacts

This module also serves as a CRM bridge to manage customers outside of Office 365 via DarwinCMS’s public-facing UI.

> See: [`office365-integration.md`](docs/office365-integration.md)


## ☁️ Cloud-Native Design
While DarwinCMS works in traditional hosting environments, it is fully **cloud-ready**:

- ✅ **Docker**: Containerization support via `Dockerfile` & `docker-compose`
- ✅ **CI/CD**: GitHub Actions & Azure Pipelines (in progress)
- ✅ **Horizontal Scaling**: Stateless services, Redis (planned)
- ✅ **Observability**: Serilog, Health Checks, Azure Insights
- ✅ **Kubernetes**: AKS-ready design, Helm chart support (planned)

Future infrastructure includes Azure Functions, Event Grid, and Cosmos DB.

> See: [`cloud-strategy.md`](docs/cloud-strategy.md)


## 🧪 Testing & Quality
- Full support for **unit tests**, **integration tests**, and mocking
- Tools: `xUnit`, `FluentAssertions`, `AutoFixture`, `Moq`
- Code coverage for application & domain layers

> See: [`CONTRIBUTING.md`](CONTRIBUTING.md)


## 🧰 Developer Tools
- CLI Tooling via `DarwinCMS.Tools` for seeding/migration
- `DarwinCMS.Worker` for background jobs (Outbox pattern, sync)
- Admin and Public APIs via `DarwinCMS.AdminApi` and `DarwinCMS.WebApi`


## 💾 Database Strategy
- Default: SQL Server with EF Core Fluent Configurations
- Future Support: PostgreSQL, MySQL
- Designed to decouple from RDBMS (repo + service abstraction)
- Redis planned for distributed caching (menus, sessions)


## 🧠 Educational Focus
This project is designed to teach:

- Real-world modular CMS development
- Architecture principles (SOLID, Clean Architecture)
- Microsoft ecosystem (Graph API, SharePoint, Power Platform)
- GitHub workflows, CI/CD pipelines
- Event-Driven and Microservices migration strategy

Each feature is implemented with full XML/C# documentation for future maintainers and learners.


## 📂 Folder Structure
See [`solution-structure.md`](docs/solution-structure.md) and [`DarwinCMS_DevGuide.md`](docs/DarwinCMS_DevGuide.md) for full breakdown of projects, module layout, and structure.


## 📝 Documentation Status
All documents are currently **draft** and subject to change.
Some notes may include personal reminders or planning thoughts.
We welcome contributions to help improve the documentation!


## 📜 License
MIT License – see [`LICENSE`](LICENSE)

---

> Project Lead: [@shahramvafadar](https://github.com/shahramvafadar)
>
> Contributions welcome – see [`CONTRIBUTING.md`](CONTRIBUTING.md)
