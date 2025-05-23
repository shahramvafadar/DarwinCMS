# DarwinCMS System Architecture

## 📌 Purpose

DarwinCMS is built as both a real-world content management system and an educational tool for developers at all levels. The architecture aims to:

- Demonstrate best practices in modern .NET application design
- Serve real business needs while staying developer-friendly
- Support modular growth, domain separation, testability, and maintainability
- Provide a transition path to microservices and the cloud (Azure)

---

## 🧱 Architectural Overview

DarwinCMS uses **Clean Architecture** as the foundation, extended with:

- **Modular Monolith Design** – Independent bounded contexts via feature folders and modules
- **Domain-Driven Design (DDD)** – Entities, value objects, aggregates, domain services
- **Event-Driven Design** – Using domain events and mediator patterns
- **Dependency Inversion** – Abstractions in core, implementations in outer layers
- **Scalable & Testable Design** – Supports unit, integration, and acceptance testing

```
       +--------------------------+
       |       UI Layer (Web)    |
       +--------------------------+
                ↓ (ViewModels)
       +--------------------------+
       | Application Layer        |
       | Use Cases, CQRS, DTOs    |
       +--------------------------+
                ↓ (Interfaces)
       +--------------------------+
       | Domain Layer             |
       | Entities, VOs, Events    |
       +--------------------------+
                ↑ (implements)
       +--------------------------+
       | Infrastructure Layer     |
       | EF Core, Identity, Email |
       +--------------------------+
```

---

## 🧩 Layers in Detail

### 1. Domain Layer

- Contains only business logic, no external dependencies.
- Defines core rules, constraints, and the "heart" of the application.
- Includes:
  - Entities
  - Value Objects
  - Domain Events
  - Repository Interfaces

### 2. Application Layer

- Contains CQRS commands/queries and business use cases.
- Coordinates between domain logic and infrastructure implementations.
- Includes:
  - Application Services
  - DTOs and Mappers
  - Validators (FluentValidation)
  - MediatR Handlers
  - Interfaces to infrastructure services

### 3. Infrastructure Layer

- Implements everything external to the application logic.
- Includes:
  - EF Core and Identity
  - File Storage
  - Email Sender
  - External APIs (e.g., SharePoint, CRM)
  - Configuration and startup wiring

### 4. Presentation Layer (Web)

- ASP.NET Core project using Razor Pages and Telerik UI
- Handles UI, routing, controllers (optional), layout, and user interaction
- Integrates authentication and global error handling

---

## ⚙️ Patterns and Practices

- **MediatR** for CQRS and event handling
- **FluentValidation** for input validation
- **AutoMapper** (optional) for object transformation
- **EF Core** with Fluent API for database mapping
- **Modular Folder Structure** per module
- **Event Bus** for internal decoupled communication
- **Partial Views / ViewComponents** for reusable UI
- **Middleware for Exception Handling, Logging, Localization**

---

## 🧠 Design Principles

- **Open/Closed Principle** – Easily extend modules with minimal change
- **Single Responsibility** – Every layer/module has one clear purpose
- **Separation of Concerns** – Each concern handled in the correct layer
- **Testability** – Core logic is mockable and testable without web/db

---

## ☁️ Cloud & Future-Ready

The system is prepared for:

- Docker-based deployment
- Azure App Services, AKS, or Kubernetes
- Cloud Identity providers (Azure AD, Google, etc.)
- Event Grid for domain events (planned)
- Azure Monitor / Application Insights
- Office 365 API integration

---

## 🔄 Diagram Reference

DarwinCMS includes diagrams for:

- Full layered architecture
- Event Bus interaction between modules
- Deployment topologies (shared hosting vs. cloud-native)
- API Gateway and future service mesh integration

These are included in `/docs/images/` and referenced in the GitHub Wiki or exported documentation.

---

## 🎓 Educational Focus

This project supports a full educational video course. The course starts from basics (HTML, C#, Razor) and evolves toward:

- SOLID + Clean Architecture
- Modular CMS development
- Office 365 and Graph API integration
- Identity + Security + Multi-tenancy (optional)
- Event-driven + cloud-native patterns

The goal: learners should be able to build a similar CMS from scratch by the end.

