# AI Knowledge Base - Backend Skill Map (Modular Monolith .NET 8)

## 1. Core Stack

* ASP.NET Core 8 Web API
* C#
* Entity Framework Core
* PostgreSQL
* MediatR (CQRS pattern)
* FluentValidation
* AutoMapper (optional)

---

## 2. Architecture: Modular Monolith

### Concept

Satu aplikasi backend, tetapi dipisahkan menjadi modul independen secara logical.

```text
src/
├── Modules/
│   ├── Identity/
│   ├── Workspace/
│   ├── Document/
│   ├── Chat/
│   ├── AI/
│   └── SharedKernel/
├── Infrastructure/
├── Api/
└── Worker/
```

---

## 3. Modular Design Principles

### 3.1 Isolation

* Setiap module punya:

  * Domain
  * Application layer
  * Infrastructure (optional)
* Tidak boleh direct dependency antar module domain

### 3.2 Communication

* Via MediatR (events / commands)
* Or domain events

### 3.3 Shared Kernel

* Common types (UserId, BaseEntity, Result)

---

## 4. Core Modules

### 4.1 Identity Module

* User registration
* Login
* JWT token
* Refresh token

### 4.2 Workspace Module

* Multi workspace per user
* Access control

### 4.3 Document Module

* Upload file
* Extract text
* Store metadata
* Chunking pipeline

### 4.4 Chat Module

* Chat session management
* Message storage
* Conversation history

### 4.5 AI Module

* Embedding generation
* RAG pipeline
* OpenClaw integration
* Prompt orchestration

---

## 5. Data Flow (RAG System)

```text
User Question
    ↓
Chat Module
    ↓
AI Module
    ↓
Vector Search (PostgreSQL pgvector)
    ↓
Top-K Chunks
    ↓
LLM / OpenClaw
    ↓
Response
```

---

## 6. Background Processing

Gunakan Worker Service:

* Document parsing
* Chunking
* Embedding generation
* Queue processing

Tools:

* Hangfire / Quartz.NET / BackgroundService

---

## 7. Database Design

### Core Tables

* Users
* Workspaces
* Documents
* Chunks
* Chats
* Messages

### AI Tables

* Embeddings
* Vector indexes (pgvector)

---

## 8. API Design

### REST Endpoints

```text
POST   /api/auth/login
POST   /api/workspaces
GET    /api/documents
POST   /api/documents/upload
POST   /api/chat/message
```

---

## 9. Cross-Cutting Concerns

* Logging (Serilog)
* Exception handling middleware
* Validation pipeline
* Rate limiting
* Caching (Redis optional)

---

## 10. Security

* JWT Authentication
* Refresh token rotation
* Workspace isolation
* Input sanitization

---

## 11. AI Integration Layer

### Responsibilities

* Embedding service
* Prompt builder
* Context assembler
* LLM communication (OpenClaw)

---

## 12. Performance Considerations

* Async everywhere
* Background processing for heavy tasks
* Batch embedding generation
* Indexing vector search

---

## 13. Testing Strategy

* Unit test (Domain logic)
* Integration test (API)
* Module isolation test

---

## 14. Deployment

### Docker Compose

* API service
* Worker service
* PostgreSQL
* OpenClaw
* Nginx

---

## 15. Backend Goal

Membangun backend yang:

* Modular
* Scalable
* AI-ready
* Maintainable dalam jangka panjang
