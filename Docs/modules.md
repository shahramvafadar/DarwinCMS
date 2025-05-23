# DarwinCMS Modules

This document describes all feature modules in DarwinCMS, including their roles, status, structure, and key dependencies. The system is designed to start with the most essential module and grow incrementally.

---

## ✅ Core Modules (Phase 1 - MVP)

### 📦 Module: Content (Pages)
**Status:** Implemented  
**Purpose:**  
Manage static or dynamic pages of the site with support for metadata, routing, layout selection, and versioning.

**Project Name:** `DarwinCMS.Modules.Content`  
**Internal Structure:**
- `Domain/` – Page entity, events, repository interface
- `Application/` – Commands/queries like CreatePage, GetPageBySlug
- `Infrastructure/` – PageRepository EF implementation
- `UI/` – Razor Pages, ViewModels, partials for managing content

**Packages Used:** `MediatR`, `FluentValidation`, `EF Core`, `AutoMapper`

---

### 📦 Module: User Management
**Status:** Implemented  
**Purpose:**  
Authentication and authorization using ASP.NET Identity. Admin user roles and claims-based access control.

**Project Name:** `DarwinCMS.Modules.Users`  
**Internal Structure:**
- `Identity/` – User, Role entities, Identity configuration
- `Services/` – Login, registration, token generation
- `UI/` – Account and user management Razor pages

**Packages Used:** `Microsoft.AspNetCore.Identity`, `Microsoft.AspNetCore.Authentication.*`, `TwoFactor`, `Google/Microsoft Login`

---

### 📦 Module: Navigation / Menus
**Status:** Implemented  
**Purpose:**  
Build and render navigation menus based on user roles, permissions, and content.

**Project Name:** `DarwinCMS.Modules.Navigation`  
**Internal Structure:**
- `Entities/` – Menu, MenuItem
- `Application/` – Menu CRUD, caching
- `UI/` – Component or ViewComponent for menu rendering

---

## 🔜 Optional Modules (Planned or In Progress)

### 📦 Module: Blog
**Status:** In Progress  
**Purpose:**  
Supports blog posts, categories, tagging, SEO-friendly URLs, and commenting.

### 📦 Module: Media Library
**Status:** Planned  
**Purpose:**  
Upload, manage and reuse media (images, videos) with metadata and folder structure.

### 📦 Module: Comments
**Status:** Planned  
**Purpose:**  
Supports threaded comments, moderation, notifications.

### 📦 Module: Notifications
**Status:** Planned  
**Purpose:**  
System-wide notifications via email, SignalR, or dashboard.

### 📦 Module: Localization
**Status:** Planned  
**Purpose:**  
Multilingual support for content and UI labels. Uses JSON files or EF-based translation provider.

### 📦 Module: Search
**Status:** Planned  
**Purpose:**  
Site-wide search index with filters, boosting, suggestions.

### 📦 Module: Analytics
**Status:** Planned  
**Purpose:**  
Track page views, user actions. Pluggable with Matomo, GA4.

---

## 🔄 Integration Module: Office 365 & CRM

### 📦 Module: CRM (Office 365)
**Status:** Placeholder project  
**Purpose:**  
Integrate DarwinCMS with Office 365 services: SharePoint, Power Automate, and Dynamics CRM. This module will evolve in the final stage of development.

**Will Include:**
- SharePoint Lists & Libraries for storing contracts and customer files
- PowerApps UI for internal workflows
- Power Automate flows for triggering email/workflows
- Dynamics 365 APIs for contact sync and quote management

**Project Name:** `DarwinCMS.Modules.Office365CRM` (Planned)

---

## 📁 Modules Folder Layout (Standardized)

Each module follows this pattern:

```
📦 DarwinCMS.Modules.[ModuleName]
 ┣ 📁 Domain
 ┣ 📁 Application
 ┣ 📁 Infrastructure
 ┣ 📁 UI
 ┗ 📄 [Module].csproj
```

This modular monolith layout ensures modules are independent yet integrated within the main system.

