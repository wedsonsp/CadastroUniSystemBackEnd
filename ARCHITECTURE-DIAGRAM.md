# 🏗️ Diagrama de Arquitetura - Sistemaws

## 📊 Visão Geral da Arquitetura

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                              🌐 FRONTEND (Angular)                              │
│                         HTTP Requests + JWT Tokens                             │
└─────────────────────┬───────────────────────────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                          ☁️ AZURE FUNCTIONS (API)                               │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐ │
│  │   Authenticate  │  │   CreateUser    │  │    GetUsers     │  │ GetUserById │ │
│  │   Function      │  │   Function      │  │   Function      │  │  Function   │ │
│  └─────────────────┘  └─────────────────┘  └─────────────────┘  └─────────────┘ │
│           │                     │                     │                     │     │
│           ▼                     ▼                     ▼                     ▼     │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                    🛡️ JWT AUTHENTICATION MIDDLEWARE                        │ │
│  │              Validates tokens and extracts user information                 │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
└─────────────────────┬───────────────────────────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                          🧠 APPLICATION LAYER (CQRS)                           │
│                                                                                 │
│  ┌─────────────────────────────────┐    ┌─────────────────────────────────┐   │
│  │           📝 COMMANDS            │    │           🔍 QUERIES             │   │
│  │                                 │    │                                 │   │
│  │  • AuthenticateCommand           │    │  • GetAllUsersQuery             │   │
│  │  • CreateUserCommand             │    │  • GetUserByIdQuery             │   │
│  │  • LoginCommand                  │    │                                 │   │
│  │  • LoginWithTokenCommand         │    │                                 │   │
│  └─────────────────────────────────┘    └─────────────────────────────────┘   │
│           │                                         │                         │
│           ▼                                         ▼                         │
│  ┌─────────────────────────────────┐    ┌─────────────────────────────────┐   │
│  │          ⚡ HANDLERS             │    │          ⚡ HANDLERS             │   │
│  │                                 │    │                                 │   │
│  │  • AuthenticateCommandHandler   │    │  • GetAllUsersQueryHandler      │   │
│  │  • CreateUserCommandHandler     │    │  • GetUserByIdQueryHandler      │   │
│  │  • LoginCommandHandler          │    │                                 │   │
│  │  • LoginWithTokenCommandHandler │    │                                 │   │
│  └─────────────────────────────────┘    └─────────────────────────────────┘   │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                        ✅ VALIDATORS (FluentValidation)                     │ │
│  │  • CreateUserCommandValidator  • LoginCommandValidator                     │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
└─────────────────────┬───────────────────────────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                          🏛️ DOMAIN LAYER (Core)                                │
│                                                                                 │
│  ┌─────────────────────────────────┐    ┌─────────────────────────────────┐   │
│  │          🏗️ ENTITIES            │    │          🔌 INTERFACES          │   │
│  │                                 │    │                                 │   │
│  │  • BaseEntity                   │    │  • IUserRepository               │   │
│  │  • User                         │    │  • IBaseRepository              │   │
│  │                                 │    │  • IAuthenticationService       │   │
│  │                                 │    │  • ITokenService                 │   │
│  └─────────────────────────────────┘    └─────────────────────────────────┘   │
│                                                                                 │
│  ┌─────────────────────────────────┐    ┌─────────────────────────────────┐   │
│  │            📋 DTOs              │    │          ⚠️ EXCEPTIONS          │   │
│  │                                 │    │                                 │   │
│  │  • CreateUserRequest            │    │  • DomainException               │   │
│  │  • LoginRequest                 │    │                                 │   │
│  │  • LoginResponse                │    │                                 │   │
│  │  • UserResponse                 │    │                                 │   │
│  │  • LoginWithTokenRequest        │    │                                 │   │
│  └─────────────────────────────────┘    └─────────────────────────────────┘   │
└─────────────────────┬───────────────────────────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        🔧 INFRASTRUCTURE LAYER                                  │
│                                                                                 │
│  ┌─────────────────────────────────┐    ┌─────────────────────────────────┐   │
│  │         📚 REPOSITORIES         │    │          🔧 SERVICES            │   │
│  │                                 │    │                                 │   │
│  │  • BaseRepository               │    │  • AuthenticationService         │   │
│  │  • UserRepository               │    │  • JwtService                   │   │
│  │                                 │    │  • JwtTokenService              │   │
│  └─────────────────────────────────┘    └─────────────────────────────────┘   │
│                                                                                 │
│  ┌─────────────────────────────────┐    ┌─────────────────────────────────┐   │
│  │         💾 PERSISTENCE           │    │         🗄️ MIGRATIONS          │   │
│  │                                 │    │                                 │   │
│  │  • SistemawsDbContext           │    │  • InitialCreate                 │   │
│  │  • DesignTimeDbContextFactory    │    │  • AddUserFields                 │   │
│  │                                 │    │  • UpdateUserTable               │   │
│  │                                 │    │  • FinalUserSchema               │   │
│  └─────────────────────────────────┘    └─────────────────────────────────┘   │
└─────────────────────┬───────────────────────────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                           🗄️ SQL SERVER DATABASE                               │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────────┐ │
│  │                              Users Table                                    │ │
│  │  ┌─────────┬─────────────┬─────────────┬─────────────┬─────────────────────┐ │ │
│  │  │   Id    │    Name     │    Email    │ PasswordHash│   IsAdministrator   │ │ │
│  │  ├─────────┼─────────────┼─────────────┼─────────────┼─────────────────────┤ │ │
│  │  │    1    │ João Silva  │ joao@email  │   SHA256    │       true          │ │ │
│  │  │    2    │ Maria Silva │ maria@email │   SHA256    │      false          │ │ │
│  │  └─────────┴─────────────┴─────────────┴─────────────┴─────────────────────┘ │ │
│  └─────────────────────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────────────────┘
```

## 🔄 Fluxo de Autenticação

```
1. 🌐 FRONTEND
   ┌─────────────────┐
   │ Login Form      │
   │ Email + Password│
   └─────────┬───────┘
             │ HTTP POST
             ▼
2. ☁️ AZURE FUNCTIONS
   ┌─────────────────┐
   │ Authenticate    │
   │ Function        │
   └─────────┬───────┘
             │
             ▼
3. 🧠 APPLICATION
   ┌─────────────────┐
   │ Authenticate    │
   │ Command         │
   └─────────┬───────┘
             │
             ▼
   ┌─────────────────┐
   │ Authenticate    │
   │ Handler          │
   └─────────┬───────┘
             │
             ▼
4. 🏛️ DOMAIN
   ┌─────────────────┐
   │ User Entity     │
   │ Validation      │
   └─────────┬───────┘
             │
             ▼
5. 🔧 INFRASTRUCTURE
   ┌─────────────────┐
   │ UserRepository  │
   │ Password Check  │
   └─────────┬───────┘
             │
             ▼
6. 🗄️ DATABASE
   ┌─────────────────┐
   │ Users Table     │
   │ Credential Check│
   └─────────┬───────┘
             │
             ▼
7. 🔄 RESPONSE
   ┌─────────────────┐
   │ JWT Token +     │
   │ User Data       │
   └─────────────────┘
```

## 🛡️ Fluxo de Autorização

```
1. 🌐 FRONTEND
   ┌─────────────────┐
   │ API Request     │
   │ + JWT Token     │
   └─────────┬───────┘
             │ HTTP Request
             ▼
2. ☁️ AZURE FUNCTIONS
   ┌─────────────────┐
   │ JWT Middleware  │
   │ Token Validation│
   └─────────┬───────┘
             │
             ▼
3. 🧠 APPLICATION
   ┌─────────────────┐
   │ Command/Query   │
   │ Handler          │
   └─────────┬───────┘
             │
             ▼
4. 🏛️ DOMAIN
   ┌─────────────────┐
   │ Authorization   │
   │ Check           │
   └─────────┬───────┘
             │
             ▼
5. 🔧 INFRASTRUCTURE
   ┌─────────────────┐
   │ Business Logic  │
   │ Execution       │
   └─────────┬───────┘
             │
             ▼
6. 🗄️ DATABASE
   ┌─────────────────┐
   │ Data Operation  │
   └─────────────────┘
```

## 📊 Resumo das Responsabilidades

| Camada | Responsabilidade | Tecnologias |
|--------|------------------|-------------|
| **🌐 Frontend** | Interface do usuário | Angular, HTTP Client |
| **☁️ Azure Functions** | Endpoints HTTP | Azure Functions, Middleware |
| **🧠 Application** | Casos de uso | MediatR, CQRS, FluentValidation |
| **🏛️ Domain** | Regras de negócio | Entities, Interfaces, DTOs |
| **🔧 Infrastructure** | Implementações | EF Core, Repositories, Services |
| **🗄️ Database** | Persistência | SQL Server, Migrations |

## 🎯 Princípios Arquiteturais

- **✅ Clean Architecture**: Separação clara de responsabilidades
- **✅ CQRS**: Separação entre comandos e consultas
- **✅ Dependency Injection**: Inversão de dependências
- **✅ SOLID Principles**: Princípios de design orientado a objetos
- **✅ Repository Pattern**: Abstração da camada de dados
- **✅ JWT Authentication**: Autenticação stateless
- **✅ FluentValidation**: Validação declarativa
- **✅ Entity Framework**: ORM para acesso a dados
