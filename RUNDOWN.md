# Backend Rundown — AI Knowledge Base

## ✅ Phase 1: Project Setup (COMPLETED)
- [x] Inisialisasi ASP.NET Core 10 Web API solution
- [x] Modular Monolith structure (9 projects: Api, Modules/*, Infrastructure, Worker)
- [x] Project references antar modul
- [x] NuGet packages (EF Core, MediatR, FluentValidation, Serilog, JWT, PostgreSQL)
- [x] Docker Compose setup (API + Worker + PostgreSQL pgvector)
- [x] App settings & configuration
- [x] Shared Kernel (BaseEntity, ValueObject, Result pattern, IRepository, IUnitOfWork)
- [x] Infrastructure (AppDbContext, JwtSettings)
- [x] API (HealthController, ExceptionMiddleware)
- [x] Worker template (DocumentWorker)

## ✅ Phase 2: Shared Kernel (COMPLETED)
- [x] BaseEntity + domain events support
- [x] ValueObject
- [x] Result pattern (Result<T>, Error)
- [x] IDomainEvent abstraction (via MediatR INotification)
- [x] Common interfaces (IRepository, IUnitOfWork)

## ✅ Phase 3: Infrastructure (COMPLETED)
- [x] EF Core DbContext + PostgreSQL (Npgsql)
- [x] Generic BaseRepository<T>
- [x] JWT Authentication setup (Bearer, token validation)
- [x] CORS configuration (AllowFrontend policy)
- [x] Serilog logging (console + request logging)
- [x] DependencyInjection extension (AddInfrastructure)
- [x] App settings updated (Serilog, JWT, Cors, ConnectionStrings)

## ✅ Phase 4: Identity Module (COMPLETED)
- [x] User entity (name, email, password hash, refresh token)
- [x] EF Core configuration (table Users, unique email index)
- [x] IUserRepository + UserRepository
- [x] Register command + handler (PasswordHasher, JWT)
- [x] Login command + handler (verify password, return JWT)
- [x] FluentValidation (register + login validators)
- [x] IJwtService + JwtService implementation
- [x] AuthController (POST /api/auth/register, POST /api/auth/login)
- [x] AuthController (GET /api/auth/me) — return user from JWT

## ⬜ Phase 5: Workspace Module
- [ ] Workspace entity
- [ ] CRUD workspaces
- [ ] User-workspace access control
- [ ] API endpoints

## ⬜ Phase 6: Document Module
- [ ] Document entity + metadata
- [ ] File upload endpoint
- [ ] Text extraction service
- [ ] Chunking pipeline
- [ ] Store chunks + embeddings
- [ ] Background worker untuk parsing

## ⬜ Phase 7: Chat Module
- [ ] Chat session entity
- [ ] Message entity
- [ ] Create/get chat sessions
- [ ] Send message endpoint
- [ ] Conversation history

## ⬜ Phase 8: AI Module (RAG Pipeline)
- [ ] Embedding generation service
- [ ] Vector search (pgvector)
- [ ] Prompt builder
- [ ] Context assembler (Top-K chunks)
- [ ] OpenClaw / LLM integration
- [ ] Streaming response

## ⬜ Phase 9: API Layer
- [ ] Controllers assembly
- [ ] MediatR pipeline (validation, logging)
- [ ] Rate limiting
- [ ] API documentation (Swagger)

## ⬜ Phase 10: Worker Service
- [ ] Background service untuk document processing
- [ ] Queue-based chunking
- [ ] Batch embedding generation

## ⬜ Phase 11: Testing
- [ ] Unit test (domain logic)
- [ ] Integration test (API)
- [ ] Module isolation test

## ⬜ Phase 12: Docker & Deployment
- [ ] Dockerfile untuk API + Worker
- [ ] Docker Compose (API, Worker, PostgreSQL, OpenClaw, Nginx)
- [ ] Environment configuration
