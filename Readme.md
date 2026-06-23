# AI Knowledge Base - Backend

## Tech Stack

* ASP.NET Core 10
* C#
* Entity Framework Core
* PostgreSQL + pgvector
* MediatR
* FluentValidation

---

## Architecture: Modular Monolith

Backend dibagi menjadi modul independen dalam satu solution.

```text id="bearch001"
src/
├── Api/
├── Modules/
│   ├── Identity/
│   ├── Workspace/
│   ├── Document/
│   ├── Chat/
│   ├── AI/
├── Infrastructure/
└── Worker/
```

---

## Modules

### Identity Module

* Login
* Register
* JWT Auth

### Workspace Module

* Multi workspace
* User isolation

### Document Module

* File upload
* Text extraction
* Chunking

### Chat Module

* Chat session
* Message history

### AI Module

* Embedding generation
* RAG pipeline
* OpenClaw integration

---

## RAG Flow

```text id="berag001"
User Query
  ↓
Embedding
  ↓
Vector Search (pgvector)
  ↓
Top-K Chunks
  ↓
LLM (OpenClaw)
  ↓
Response
```

---

## API Endpoints

```text id="beapi001"
POST /api/auth/login
POST /api/documents/upload
GET  /api/workspaces
POST /api/chat/message
```

---

## Background Jobs

* Document parsing
* Embedding generation
* Chunk processing

---

## Run Locally

```bash id="berun001"
dotnet restore
dotnet run
```

---

## Docker

```bash id="bedocker001"
docker compose up -d
```
