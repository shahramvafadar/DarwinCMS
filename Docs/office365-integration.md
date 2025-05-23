# Office 365 Integration – DarwinCMS

DarwinCMS will integrate with Microsoft 365 to provide enterprise-grade document management, user synchronization, and CRM capabilities. This document outlines the planned architecture and components for the Office 365 integration module.

---

## 🎯 Goals

- Synchronize users and clients with Azure AD / Microsoft Identity
- Use SharePoint Lists and Libraries to store structured business data and documents
- Enable automation through Power Automate and Workflows
- Provide PowerApps UI for internal users to interact with data
- Integrate with Microsoft Graph API and Dynamics CRM

---

## 🧩 Office 365 Components

### 1. Azure Active Directory (Azure AD)
- Single Sign-On (SSO) for admins and internal staff
- External login providers for Google and Microsoft accounts
- Two-Factor Authentication enabled for secure access

### 2. SharePoint Online
- Document libraries for client files, invoices, and contracts
- Lists for leads, quotes, CRM metadata
- Versioning, permissions, and metadata tagging enabled

### 3. Power Automate
- Triggers: New submission, contract signed, contact updated
- Flows: Send email, notify CRM, update SharePoint, log audit trail
- Approval flows for managers before finalizing content

### 4. PowerApps
- Custom canvas apps to manage SharePoint lists visually
- Mobile-friendly UIs for remote staff or consultants
- Interfaces for CRM workflows, lead forms, dashboards

### 5. Dynamics 365 CRM (Planned)
- Read/write contacts, companies, deals, quotes
- Use Graph API for contact sync
- Automate tasks via Logic Apps or background worker

---

## 📦 CRM Integration Project (Planned)

A separate module `DarwinCMS.Modules.Office365CRM` will be added with the following structure:

```
📦 DarwinCMS.Modules.Office365CRM
 ┣ 📁 Domain
 ┣ 📁 Application
 ┣ 📁 Infrastructure
 ┣ 📁 UI
 ┗ 📄 Office365CRM.csproj
```

**Purpose:**  
This project will connect DarwinCMS entities (clients, quotes, messages) with their Office 365 counterparts. It will include integration services, token handling, and mapping logic.

---

## 🔒 Authentication & Permissions

- OAuth 2.0 with Microsoft Identity Platform
- Token caching and refresh support
- Granular Graph API scopes for least-privilege access
- User role mapping (CMS <-> AD Groups)
- Admin UI to configure Office365 credentials and endpoints

---

## 🧠 Educational Focus

This module demonstrates how to integrate a .NET system with Office 365 enterprise tools, exposing students to:

- Graph API
- Office365 token handling
- Working with SharePoint Lists from code
- Automating real-world business processes with Power Platform
- Mapping internal domain models to external systems

---

## 🗂️ Planned SharePoint Entities

| Entity Name      | Type              | Description                          |
|------------------|-------------------|--------------------------------------|
| Clients          | SharePoint List   | Basic info + ID reference to CMS     |
| Documents        | Document Library  | Contracts, uploads, PDFs             |
| Quotes           | SharePoint List   | CRM quote data with status + logs    |
| Feedback Forms   | SharePoint List   | User or customer feedback entries    |

---

## 📊 Diagrams and Flowcharts

Included in `/docs/images`:

- CRM Integration Architecture
- Flowchart: CMS <-> SharePoint <-> PowerApps
- Authorization & Token Management Flow
- Power Automate Trigger/Action Map

---

## 🚧 Development Plan

1. Create base module and interfaces
2. Add support for Graph API auth and refresh
3. Build SharePoint wrapper service
4. Enable document uploads from CMS to SharePoint
5. Sync selected CMS data (clients, leads) to SharePoint lists
6. Add PowerApps as frontend to edit/view CRM records
7. Add trigger support via Power Automate

