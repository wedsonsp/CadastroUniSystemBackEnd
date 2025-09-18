# 🔄 Diagrama de Fluxo de Dados - Sistemaws

## 📊 Fluxo Completo de Dados

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                              🌐 FRONTEND (Angular)                              │
│                                                                                 │
│  ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐            │
│  │   Login Form    │    │  User List      │    │  Create User    │            │
│  │                 │    │  Component      │    │  Component      │            │
│  │ Email: string   │    │                 │    │                 │            │
│  │ Password: string│    │ Display users   │    │ Form validation │            │
│  └─────────────────┘    └─────────────────┘    └─────────────────┘            │
│           │                       │                       │                   │
│           ▼                       ▼                       ▼                   │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                        🔧 AuthService                                      │ │
│  │                                                                             │ │
│  │  authenticate(email, password) → POST /api/auth/authenticate               │ │
│  │  getUsers() → GET /api/users (with JWT token)                             │ │
│  │  createUser(userData) → POST /api/users (with JWT token)                  │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
└─────────────────────┬───────────────────────────────────────────────────────────┘
                      │ HTTP Requests
                      ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                          ☁️ AZURE FUNCTIONS (API)                               │
│                                                                                 │
│  ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐            │
│  │ Authenticate    │    │   GetUsers      │    │  CreateUser     │            │
│  │ Function        │    │   Function      │    │  Function       │            │
│  │                 │    │                 │    │                 │            │
│  │ [HttpTrigger]   │    │ [HttpTrigger]   │    │ [HttpTrigger]   │            │
│  │ POST /auth/     │    │ GET /users      │    │ POST /users     │            │
│  │ authenticate    │    │                 │    │                 │            │
│  └─────────────────┘    └─────────────────┘    └─────────────────┘            │
│           │                       │                       │                   │
│           ▼                       ▼                       ▼                   │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                    🛡️ JWT AUTHENTICATION MIDDLEWARE                        │ │
│  │                                                                             │ │
│  │  • Validates JWT tokens                                                    │ │
│  │  • Extracts user information                                               │ │
│  │  • Skips authentication for public endpoints                               │ │
│  │  • Returns 401 for invalid/missing tokens                                  │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
└─────────────────────┬───────────────────────────────────────────────────────────┘
                      │ MediatR Commands/Queries
                      ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                          🧠 APPLICATION LAYER (CQRS)                           │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                           📝 COMMANDS                                       │ │
│  │                                                                             │ │
│  │  AuthenticateCommand {                    CreateUserCommand {               │ │
│  │    Email: string                            Name: string                    │ │
│  │    Password: string                         Email: string                   │ │
│  │  }                                          Password: string                │ │
│  │                                             }                               │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
│           │                                         │                         │
│           ▼                                         ▼                         │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                          ⚡ HANDLERS                                       │ │
│  │                                                                             │ │
│  │  AuthenticateCommandHandler {              CreateUserCommandHandler {      │ │
│  │    1. Validate credentials                 1. Check if user is admin       │ │
│  │    2. Generate JWT token                   2. Validate user data           │ │
│  │    3. Return token + user data             3. Hash password                │ │
│  │  }                                         4. Save to database             │ │
│  │                                            }                               │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                           🔍 QUERIES                                       │ │
│  │                                                                             │ │
│  │  GetAllUsersQuery {                    GetUserByIdQuery {                   │ │
│  │    // No parameters                      Id: int                           │ │
│  │  }                                       }                                 │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
│           │                                         │                         │
│           ▼                                         ▼                         │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                          ⚡ HANDLERS                                       │ │
│  │                                                                             │ │
│  │  GetAllUsersQueryHandler {              GetUserByIdQueryHandler {           │ │
│  │    1. Get all users from repo           1. Get user by ID from repo        │ │
│  │    2. Return user list                  2. Return user data               │ │
│  │  }                                       }                                 │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
└─────────────────────┬───────────────────────────────────────────────────────────┘
                      │ Repository Calls
                      ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                          🏛️ DOMAIN LAYER (Core)                                │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                           🏗️ ENTITIES                                      │ │
│  │                                                                             │ │
│  │  User {                                                                     │ │
│  │    Id: int                                                                  │ │
│  │    Name: string                                                             │ │
│  │    Email: string                                                            │ │
│  │    PasswordHash: string                                                     │ │
│  │    Salt: string                                                             │ │
│  │    IsActive: bool                                                           │ │
│  │    IsAdministrator: bool                                                   │ │
│  │    CreatedAt: DateTime                                                     │ │
│  │    UpdatedAt: DateTime?                                                    │ │
│  │  }                                                                          │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                           📋 DTOs                                          │ │
│  │                                                                             │ │
│  │  CreateUserRequest {                   LoginRequest {                       │ │
│  │    Name: string                            Email: string                    │ │
│  │    Email: string                           Password: string                 │ │
│  │    Password: string                      }                                 │ │
│  │  }                                                                          │ │
│  │                                                                             │ │
│  │  UserResponse {                        LoginResponse {                      │ │
│  │    Id: int                                 Token: string                    │ │
│  │    Name: string                            User: UserResponse               │ │
│  │    Email: string                         }                                 │ │
│  │    IsActive: bool                                                          │ │
│  │    IsAdministrator: bool                                                  │ │
│  │    CreatedAt: DateTime                                                    │ │
│  │  }                                                                          │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
└─────────────────────┬───────────────────────────────────────────────────────────┘
                      │ Repository Implementation
                      ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        🔧 INFRASTRUCTURE LAYER                                  │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                        📚 REPOSITORIES                                     │ │
│  │                                                                             │ │
│  │  UserRepository : IUserRepository {                                        │ │
│  │    GetByEmail(email) → User                                                │ │
│  │    GetById(id) → User                                                      │ │
│  │    GetAll() → List<User>                                                   │ │
│  │    Create(user) → User                                                     │ │
│  │    Update(user) → User                                                     │ │
│  │    Delete(id) → void                                                       │ │
│  │  }                                                                          │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                        🔧 SERVICES                                         │ │
│  │                                                                             │ │
│  │  AuthenticationService {                   JwtService {                     │ │
│  │    ValidateCredentials(email, password)     GenerateToken(user)             │ │
│  │    HashPassword(password, salt)             ValidateToken(token)            │ │
│  │  }                                          ExtractUserInfo(token)          │ │
│  │                                            }                               │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                        💾 PERSISTENCE                                     │ │
│  │                                                                             │ │
│  │  SistemawsDbContext : DbContext {                                           │ │
│  │    DbSet<User> Users                                                        │ │
│  │    OnModelCreating() → Configure entities                                   │ │
│  │  }                                                                          │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
└─────────────────────┬───────────────────────────────────────────────────────────┘
                      │ SQL Queries
                      ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                           🗄️ SQL SERVER DATABASE                               │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                              Users Table                                   │ │
│  │                                                                             │ │
│  │  CREATE TABLE [Users] (                                                    │ │
│  │    [Id] int NOT NULL IDENTITY,                                             │ │
│  │    [Name] nvarchar(100) NOT NULL,                                          │ │
│  │    [Email] nvarchar(255) NOT NULL,                                        │ │
│  │    [PasswordHash] nvarchar(max) NOT NULL,                                  │ │
│  │    [Salt] nvarchar(max) NOT NULL,                                          │ │
│  │    [IsActive] bit NOT NULL,                                                │ │
│  │    [IsAdministrator] bit NOT NULL,                                         │ │
│  │    [Deleted] bit NOT NULL DEFAULT CAST(0 AS bit),                         │ │
│  │    [CreatedBy] int NULL,                                                   │ │
│  │    [CreatedAt] datetime2 NOT NULL,                                         │ │
│  │    [UpdatedBy] int NULL,                                                   │ │
│  │    [UpdatedAt] datetime2 NULL,                                             │ │
│  │    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])                                │ │
│  │  );                                                                         │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────────────────┘
```

## 🔄 Fluxos Específicos

### **1. 🔐 Fluxo de Autenticação**

```
Frontend → AuthenticateFunction → AuthenticateCommand → AuthenticateCommandHandler
    ↓              ↓                      ↓                        ↓
Login Form    [HttpTrigger]         Email + Password        Validate Credentials
    ↓              ↓                      ↓                        ↓
HTTP POST → Middleware Skip → MediatR Send → UserRepository.GetByEmail()
    ↓              ↓                      ↓                        ↓
JSON Body → Function Execute → Handler Process → Database Query
    ↓              ↓                      ↓                        ↓
Response ← JWT Token ← Password Check ← User Entity ← SQL Result
```

### **2. 👥 Fluxo de Criação de Usuário**

```
Frontend → CreateUserFunction → CreateUserCommand → CreateUserCommandHandler
    ↓              ↓                    ↓                      ↓
User Form    [HttpTrigger]         User Data            Check Admin Role
    ↓              ↓                    ↓                      ↓
HTTP POST → Middleware Auth → MediatR Send → Validate Admin Token
    ↓              ↓                    ↓                      ↓
JSON Body → JWT Validation → Handler Process → UserRepository.Create()
    ↓              ↓                    ↓                      ↓
Response ← User Created ← Password Hash ← Database Insert ← SQL Execute
```

### **3. 📋 Fluxo de Listagem de Usuários**

```
Frontend → GetUsersFunction → GetAllUsersQuery → GetAllUsersQueryHandler
    ↓              ↓                 ↓                    ↓
User List    [HttpTrigger]      No Parameters        Get All Users
    ↓              ↓                 ↓                    ↓
HTTP GET → Middleware Auth → MediatR Send → UserRepository.GetAll()
    ↓              ↓                 ↓                    ↓
JSON Body → JWT Validation → Handler Process → Database Query
    ↓              ↓                 ↓                    ↓
Response ← User List ← Map to DTOs ← User Entities ← SQL Result
```

## 📊 Resumo dos Fluxos

| Operação | Endpoint | Método | Autenticação | Autorização | Fluxo |
|----------|----------|--------|--------------|-------------|-------|
| **Login** | `/api/auth/authenticate` | POST | ❌ Não | ❌ Não | Frontend → Function → Command → Handler → Repository → Database |
| **Listar Usuários** | `/api/users` | GET | ✅ Sim | ❌ Não | Frontend → Function → Query → Handler → Repository → Database |
| **Criar Usuário** | `/api/users` | POST | ✅ Sim | ✅ Admin | Frontend → Function → Command → Handler → Repository → Database |
| **Buscar Usuário** | `/api/users/{id}` | GET | ✅ Sim | ❌ Não | Frontend → Function → Query → Handler → Repository → Database |

## 🎯 Pontos de Validação

1. **🌐 Frontend**: Validação de formulários, formato de dados
2. **☁️ Azure Functions**: Middleware de autenticação, validação de headers
3. **🧠 Application**: FluentValidation, regras de negócio
4. **🏛️ Domain**: Validação de entidades, regras de domínio
5. **🔧 Infrastructure**: Validação de dados de entrada, sanitização
6. **🗄️ Database**: Constraints, índices únicos, integridade referencial
