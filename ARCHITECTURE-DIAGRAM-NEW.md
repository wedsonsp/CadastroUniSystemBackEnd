# 🏗️ Diagrama de Arquitetura - Sistemaws Web API

## 📊 Arquitetura Atualizada

```mermaid
graph TB
    subgraph "🌐 Frontend"
        FE[Angular Frontend<br/>localhost:4200]
    end
    
    subgraph "🔧 Web API Layer"
        API[Sistemaws.WebApi<br/>localhost:7201]
        SWAGGER[Swagger UI<br/>/swagger]
        CORS[CORS Policy<br/>AllowAnyOrigin]
    end
    
    subgraph "🎮 Controllers"
        AUTH[AuthController<br/>/api/auth]
        USERS[UsersController<br/>/api/users]
        TEST[TestController<br/>/api/test]
    end
    
    subgraph "🧠 Application Layer"
        MEDIATR[MediatR<br/>CQRS Pattern]
        
        subgraph "📝 Commands"
            AUTH_CMD[AuthenticateCommand]
            CREATE_CMD[CreateUserCommand]
            LOGIN_CMD[LoginCommand]
            LOGIN_TOKEN_CMD[LoginWithTokenCommand]
        end
        
        subgraph "🔍 Queries"
            GET_ALL_QRY[GetAllUsersQuery]
            GET_BY_ID_QRY[GetUserByIdQuery]
        end
        
        subgraph "⚡ Handlers"
            AUTH_HANDLER[AuthenticateCommandHandler]
            CREATE_HANDLER[CreateUserCommandHandler]
            GET_ALL_HANDLER[GetAllUsersQueryHandler]
            GET_BY_ID_HANDLER[GetUserByIdQueryHandler]
        end
        
        subgraph "✅ Validators"
            CREATE_VAL[CreateUserCommandValidator]
            LOGIN_VAL[LoginCommandValidator]
        end
    end
    
    subgraph "🏛️ Domain Layer"
        subgraph "🏗️ Entities"
            USER[User Entity]
            BASE[BaseEntity]
        end
        
        subgraph "📋 DTOs"
            CREATE_REQ[CreateUserRequest]
            LOGIN_REQ[LoginRequest]
            LOGIN_RES[LoginResponse]
            USER_RES[UserResponse]
        end
        
        subgraph "🔌 Interfaces"
            USER_REPO_INT[IUserRepository]
            AUTH_SVC_INT[IAuthenticationService]
            JWT_SVC_INT[IJwtService]
        end
        
        subgraph "⚠️ Exceptions"
            DOMAIN_EXC[DomainException]
        end
    end
    
    subgraph "🔧 Infrastructure Layer"
        subgraph "💾 Persistence"
            DBCONTEXT[SistemawsDbContext<br/>EF Core]
            MIGRATIONS[Database Migrations]
        end
        
        subgraph "📚 Repositories"
            USER_REPO[UserRepository]
            BASE_REPO[BaseRepository]
        end
        
        subgraph "🔧 Services"
            AUTH_SVC[AuthenticationService]
            JWT_SVC[JwtService]
            DB_INIT[DatabaseInitializationService]
        end
    end
    
    subgraph "🗄️ Database"
        DB[(SQL Server<br/>UniSystem Database)]
        USERS_TABLE[Users Table]
    end
    
    subgraph "🔐 Security"
        JWT[JWT Authentication<br/>Bearer Token]
        HASH[Password Hashing<br/>SHA256 + Salt]
        VALIDATION[Input Validation<br/>FluentValidation]
    end
    
    %% Connections
    FE -->|HTTP Requests| API
    API --> SWAGGER
    API --> CORS
    
    API --> AUTH
    API --> USERS
    API --> TEST
    
    AUTH --> MEDIATR
    USERS --> MEDIATR
    TEST --> MEDIATR
    
    MEDIATR --> AUTH_CMD
    MEDIATR --> CREATE_CMD
    MEDIATR --> LOGIN_CMD
    MEDIATR --> LOGIN_TOKEN_CMD
    MEDIATR --> GET_ALL_QRY
    MEDIATR --> GET_BY_ID_QRY
    
    AUTH_CMD --> AUTH_HANDLER
    CREATE_CMD --> CREATE_HANDLER
    LOGIN_CMD --> AUTH_HANDLER
    LOGIN_TOKEN_CMD --> AUTH_HANDLER
    GET_ALL_QRY --> GET_ALL_HANDLER
    GET_BY_ID_QRY --> GET_BY_ID_HANDLER
    
    CREATE_CMD --> CREATE_VAL
    LOGIN_CMD --> LOGIN_VAL
    
    AUTH_HANDLER --> USER_REPO_INT
    CREATE_HANDLER --> USER_REPO_INT
    GET_ALL_HANDLER --> USER_REPO_INT
    GET_BY_ID_HANDLER --> USER_REPO_INT
    
    AUTH_HANDLER --> JWT_SVC_INT
    CREATE_HANDLER --> AUTH_SVC_INT
    
    USER_REPO_INT --> USER_REPO
    AUTH_SVC_INT --> AUTH_SVC
    JWT_SVC_INT --> JWT_SVC
    
    USER_REPO --> DBCONTEXT
    BASE_REPO --> DBCONTEXT
    AUTH_SVC --> USER_REPO
    JWT_SVC --> JWT
    
    DBCONTEXT --> DB
    MIGRATIONS --> DB
    DB --> USERS_TABLE
    
    AUTH_SVC --> HASH
    CREATE_HANDLER --> VALIDATION
    
    %% Styling
    classDef frontend fill:#e1f5fe
    classDef webapi fill:#f3e5f5
    classDef controllers fill:#fff3e0
    classDef application fill:#e8f5e8
    classDef domain fill:#fff8e1
    classDef infrastructure fill:#fce4ec
    classDef database fill:#e0f2f1
    classDef security fill:#ffebee
    
    class FE frontend
    class API,SWAGGER,CORS webapi
    class AUTH,USERS,TEST controllers
    class MEDIATR,AUTH_CMD,CREATE_CMD,LOGIN_CMD,LOGIN_TOKEN_CMD,GET_ALL_QRY,GET_BY_ID_QRY,AUTH_HANDLER,CREATE_HANDLER,GET_ALL_HANDLER,GET_BY_ID_HANDLER,CREATE_VAL,LOGIN_VAL application
    class USER,BASE,CREATE_REQ,LOGIN_REQ,LOGIN_RES,USER_RES,USER_REPO_INT,AUTH_SVC_INT,JWT_SVC_INT,DOMAIN_EXC domain
    class DBCONTEXT,MIGRATIONS,USER_REPO,BASE_REPO,AUTH_SVC,JWT_SVC,DB_INIT infrastructure
    class DB,USERS_TABLE database
    class JWT,HASH,VALIDATION security
```

## 🔄 Fluxo de Requisições

### **1. Autenticação**
```
Frontend → POST /api/auth/authenticate → AuthController → AuthenticateCommand → AuthenticateCommandHandler → UserRepository → Database
```

### **2. Criação de Usuário**
```
Frontend → POST /api/users (com token) → UsersController → CreateUserCommand → CreateUserCommandHandler → UserRepository → Database
```

### **3. Listagem de Usuários**
```
Frontend → GET /api/users (com token) → UsersController → GetAllUsersQuery → GetAllUsersQueryHandler → UserRepository → Database
```

## 🎯 Endpoints Disponíveis

| Método | Endpoint | Autenticação | Autorização | Descrição |
|--------|----------|--------------|-------------|-----------|
| POST | `/api/auth/authenticate` | ❌ | ❌ | Gerar token JWT |
| POST | `/api/auth/reset-admin-password` | ❌ | ❌ | Resetar senha do admin |
| POST | `/api/users` | ✅ | ✅ Admin | Criar usuário |
| POST | `/api/users/create-without-token` | ❌ | ❌ | Criar usuário sem token |
| GET | `/api/users` | ✅ | ❌ | Listar usuários |
| GET | `/api/users/{id}` | ✅ | ❌ | Buscar usuário por ID |
| GET | `/api/test/test-auth` | ✅ | ❌ | Teste de autenticação |

## 🔐 Configurações de Segurança

- **JWT Key**: Configurável via `appsettings.json`
- **Expiração**: 24 horas
- **Algoritmo**: HS256
- **CORS**: Permitido para desenvolvimento
- **Hash de Senha**: SHA256 + Salt único

## 🗄️ Banco de Dados

- **Tipo**: SQL Server
- **Nome**: UniSystem
- **Criação**: Automática na primeira execução
- **Migrações**: Entity Framework Core
- **Tabela Principal**: Users

## 📊 Tecnologias Utilizadas

- **.NET 8**: Framework principal
- **ASP.NET Core Web API**: Runtime da API
- **Entity Framework Core**: ORM
- **MediatR**: CQRS Pattern
- **FluentValidation**: Validação
- **Swagger/OpenAPI**: Documentação
- **JWT Bearer**: Autenticação


