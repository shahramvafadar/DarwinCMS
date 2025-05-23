# Cloud Migration and Scalability Strategy

DarwinCMS is designed to run on shared hosting environments but supports a smooth migration path to the cloud. This file describes the patterns, principles, and technologies adopted to ensure scalability, resilience, and cloud compatibility.

---

## 🌐 Shared Hosting (Current Deployment Target)

- Deployment via FTP or ZIP to standard Linux or Windows hosting
- Single Web App instance (.NET Core)
- SQLite or SQL Server Express for DB
- No message queue or distributed cache
- No container orchestration
- Limited scaling (vertical only)

This allows DarwinCMS to be used by small organizations and for educational purposes without requiring cloud infrastructure.

---

## ☁️ Cloud-Ready Design (Planned Enhancements)

The application is prepared for the following:

### ✅ Containerization

- **Docker Support** planned via `Dockerfile` and `docker-compose.override.yml`
- Each project can be containerized as its own microservice later
- Volume mounting, secrets, environment-based config supported

### ✅ CI/CD Pipelines

- GitHub Actions or Azure DevOps pipeline YAML files (to be added)
- Supports unit tests, linting, build, publish, deploy stages
- Secrets management via GitHub/Azure Key Vault

### ✅ Horizontal Scaling

- Designed for horizontal scaling using Azure App Services
- Stateless architecture (no in-memory state per user)
- Session handling via distributed cache (Redis planned)
- Health Check endpoint (`/health`) via `AspNetCore.Diagnostics.HealthChecks`

### ✅ Kubernetes Migration

- DarwinCMS follows the **Strangler Fig Pattern** to slowly extract modules as microservices
- Future: migrate modules like Notifications, CRM Sync to separate services
- Use **API Gateway** or **BFF** pattern to split frontend/backend
- Helm charts (planned), AKS-ready manifests for staging/production

### ✅ Observability

- Logging via `Serilog` or `Azure Application Insights`
- Health and readiness probes
- Metrics export planned for Prometheus/Grafana

---

## 🛠️ Resilience Patterns

DarwinCMS will adopt the following resilience and cloud patterns:

| Pattern            | Purpose                                       |
|--------------------|-----------------------------------------------|
| Retry + Timeout     | Fault handling when calling external APIs    |
| Circuit Breaker     | Prevent service overload in failure cases     |
| Bulkhead Isolation  | Isolate heavy dependencies (e.g., SMTP)       |
| Rate Limiting       | Protect public endpoints and APIs             |
| Eventual Consistency| Async syncing for Office365, CRM              |
| Outbox Pattern      | Reliable messaging using background jobs      |
| Cache Aside         | Improve performance and scalability           |

---

## 🧩 Optional Cloud Services (Future Integration)

| Service                | Purpose                                      |
|------------------------|----------------------------------------------|
| Azure Blob Storage     | Media uploads, PDFs, etc.                    |
| Azure Event Grid       | Event-based async comms (e.g., CRM Updated)  |
| Azure Functions        | Scheduled jobs, external sync                |
| Azure Key Vault        | Secure secret storage                        |
| Azure API Management   | Central API Gateway                          |
| Azure Redis Cache      | Performance boost for menu, pages, sessions  |
| Azure Cosmos DB        | For globally-distributed read access         |

---

## 🔄 Migration Plan: Phased Approach

1. **Phase 1: Local Hosting**
   - SQLite or SQL Server LocalDB
   - Razor Pages, Single Web Project
   - No Docker, works on any shared host

2. **Phase 2: Dockerized Monolith**
   - Single container
   - Use Docker Compose for local dev
   - Secrets in `env` files or user-secrets

3. **Phase 3: Cloud Dev/Test**
   - Push to Azure Web App (Linux)
   - Enable Application Insights and logging

4. **Phase 4: Distributed Services**
   - Extract CRM/Notifications into services
   - Introduce RabbitMQ or Azure Service Bus

5. **Phase 5: Kubernetes Deployment**
   - AKS + CI/CD pipelines
   - Helm + Secrets + Observability stack

---

## 📊 Diagrams and Topology

This strategy is supported by multiple diagrams in the documentation:

- Deployment Topology (Shared vs. Cloud)
- Messaging Pattern via Event Bus
- Service Mesh Gateway (planned)
- BFF (Backend-for-Frontend) with React

These diagrams are stored in `/docs/images` and version-controlled.

