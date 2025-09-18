# 📁 Estrutura do Projeto Sistemaws

## 🏗️ Organograma Completo da Arquitetura

```
📦 CadastroUniSystemBackEnd/
├── 📄 README.md                           # Documentação principal da API
├── 📄 PROJECT-STRUCTURE.md                # Estrutura do projeto
├── 📄 ARCHITECTURE-DIAGRAM.md             # Diagrama de arquitetura
├── 📄 DATA-FLOW-DIAGRAM.md                # Diagrama de fluxo de dados
├── 📄 FRONTEND-AUTH-GUIDE.md              # Guia de autenticação frontend
├── 📄 README-Docker.md                    # Documentação Docker
├── 📄 README-SETUP.md                     # Guia de configuração
├── 📄 docker-compose.yml                  # Configuração Docker Compose
├── 📄 Sistemaws.sln                      # Solução Visual Studio
│
├── 📁 Sistemaws.WebApi/                   # 🌐 ASP.NET Core Web API (Principal)
│   ├── 📄 Program.cs                      # Configuração da aplicação
│   ├── 📄 Sistemaws.WebApi.csproj        # Arquivo de projeto
│   ├── 📄 appsettings.json               # Configurações da aplicação
│   ├── 📄 README.md                      # Documentação da Web API
│   │
│   ├── 📁 Controllers/                   # 🎮 Controladores da API
│   │   ├── 📄 AuthController.cs          # Controlador de autenticação
│   │   ├── 📄 UsersController.cs         # Controlador de usuários
│   │   └── 📄 TestController.cs          # Controlador de teste
│   │
│   ├── 📁 bin/                           # 📦 Binários compilados
│   │   └── 📁 Debug/                     # Versão de debug
│   │       └── 📁 net8.0/                # .NET 8.0
│   │
│   └── 📁 obj/                           # 🔧 Arquivos temporários de build
│       └── 📁 Debug/                     # Objetos de debug
│
├── 📁 Sistemaws.Application/              # 🧠 Camada de Aplicação (CQRS)
│   ├── 📄 Class1.cs                      # Classe de exemplo
│   ├── 📄 Sistemaws.Application.csproj   # Arquivo de projeto
│   │
│   ├── 📁 Commands/                      # 📝 Comandos (Write Operations)
│   │   ├── 📄 AuthenticateCommand.cs     # Comando de autenticação
│   │   ├── 📄 CreateUserCommand.cs       # Comando de criação de usuário
│   │   ├── 📄 LoginCommand.cs            # Comando de login
│   │   └── 📄 LoginWithTokenCommand.cs   # Comando de login com token
│   │
│   ├── 📁 Handlers/                      # ⚡ Handlers (Processadores)
│   │   ├── 📄 AuthenticateCommandHandler.cs      # Handler de autenticação
│   │   ├── 📄 CreateUserCommandHandler.cs        # Handler de criação
│   │   ├── 📄 GetAllUsersQueryHandler.cs         # Handler de listagem
│   │   ├── 📄 GetUserByIdQueryHandler.cs         # Handler de busca por ID
│   │   ├── 📄 LoginCommandHandler.cs             # Handler de login
│   │   └── 📄 LoginWithTokenCommandHandler.cs    # Handler de login com token
│   │
│   ├── 📁 Queries/                       # 🔍 Queries (Read Operations)
│   │   ├── 📄 GetAllUsersQuery.cs       # Query para listar usuários
│   │   └── 📄 GetUserByIdQuery.cs        # Query para buscar usuário por ID
│   │
│   ├── 📁 Validators/                    # ✅ Validadores FluentValidation
│   │   ├── 📄 CreateUserCommandValidator.cs      # Validador de criação
│   │   └── 📄 LoginCommandValidator.cs           # Validador de login
│   │
│   ├── 📁 bin/                           # 📦 Binários compilados
│   └── 📁 obj/                           # 🔧 Arquivos temporários
│
├── 📁 Sistemaws.Domain/                   # 🏛️ Camada de Domínio (Core)
│   ├── 📄 Sistemaws.Domain.csproj        # Arquivo de projeto
│   │
│   ├── 📁 DTOs/                          # 📋 Data Transfer Objects
│   │   ├── 📄 CreateUserRequest.cs       # DTO de criação de usuário
│   │   ├── 📄 LoginRequest.cs            # DTO de login
│   │   ├── 📄 LoginResponse.cs          # DTO de resposta de login
│   │   ├── 📄 LoginWithTokenRequest.cs   # DTO de login com token
│   │   └── 📄 UserResponse.cs           # DTO de resposta de usuário
│   │
│   ├── 📁 Entities/                      # 🏗️ Entidades de Domínio
│   │   ├── 📄 BaseEntity.cs              # Entidade base
│   │   └── 📄 User.cs                    # Entidade Usuário
│   │
│   ├── 📁 Exceptions/                    # ⚠️ Exceções de Domínio
│   │   └── 📄 DomainException.cs         # Exceção base do domínio
│   │
│   ├── 📁 Interfaces/                     # 🔌 Interfaces (Contratos)
│   │   ├── 📁 Repositories/              # Interfaces de Repositório
│   │   │   ├── 📄 IBaseRepository.cs      # Interface base de repositório
│   │   │   └── 📄 IUserRepository.cs      # Interface de repositório de usuário
│   │   └── 📁 Services/                  # Interfaces de Serviço
│   │       ├── 📄 IAuthenticationService.cs # Interface de autenticação
│   │       └── 📄 ITokenService.cs       # Interface de serviço de token
│   │
│   ├── 📁 bin/                           # 📦 Binários compilados
│   └── 📁 obj/                           # 🔧 Arquivos temporários
│
│
├── 📁 Sistemaws.Infrastructure/           # 🔧 Camada de Infraestrutura
│   ├── 📄 Sistemaws.Infrastructure.csproj # Arquivo de projeto
│   ├── 📄 appsettings.json               # Configurações
│   │
│   ├── 📁 Migrations/                    # 🗄️ Migrações do Entity Framework
│   │   ├── 📄 20250109000000_InitialCreate.cs # Migração inicial
│   │   ├── 📄 20250109000001_AddUserFields.cs  # Migração de campos
│   │   ├── 📄 20250109000002_UpdateUserTable.cs # Migração de atualização
│   │   ├── 📄 20250109000003_FinalUserSchema.cs # Migração final
│   │   └── 📄 SistemawsDbContextModelSnapshot.cs # Snapshot do modelo
│   │
│   ├── 📁 Persistence/                    # 💾 Persistência de Dados
│   │   ├── 📄 DesignTimeDbContextFactory.cs # Factory para design time
│   │   └── 📄 SistemawsDbContext.cs      # Contexto do Entity Framework
│   │
│   ├── 📁 Repositories/                   # 📚 Implementações de Repositório
│   │   ├── 📄 BaseRepository.cs          # Repositório base
│   │   └── 📄 UserRepository.cs          # Repositório de usuário
│   │
│   ├── 📁 Services/                       # 🔧 Implementações de Serviço
│   │   ├── 📄 AuthenticationService.cs   # Serviço de autenticação
│   │   ├── 📄 JwtService.cs              # Serviço JWT
│   │   └── 📄 JwtTokenService.cs         # Serviço de token JWT
│   │
│   ├── 📄 ServiceCollectionExtensions.cs # Extensões de DI
│   ├── 📁 bin/                           # 📦 Binários compilados
│   └── 📁 obj/                           # 🔧 Arquivos temporários
│
└── 📁 Sistemaws.Test/                     # 🧪 Camada de Testes
    ├── 📄 Sistemaws.Test.csproj          # Arquivo de projeto de teste
    ├── 📄 GlobalUsings.cs                # Usings globais
    ├── 📄 UnitTest1.cs                   # Teste unitário de exemplo
    ├── 📄 FixAllTests.ps1                # Script para corrigir testes
    ├── 📄 UpdateTests.ps1                # Script para atualizar testes
    ├── 📄 coverlet.runsettings           # Configurações de cobertura
    │
    ├── 📁 Application/                    # 🧪 Testes da Camada de Aplicação
    │   ├── 📄 CreateUserCommandHandlerTests.cs    # Testes do handler de criação
    │   ├── 📄 AuthenticateCommandHandlerTests.cs  # Testes do handler de auth
    │   ├── 📄 GetAllUsersQueryHandlerTests.cs     # Testes do handler de listagem
    │   └── 📄 GetUserByIdQueryHandlerTests.cs     # Testes do handler de busca
    │
    ├── 📁 Domain/                         # 🧪 Testes da Camada de Domínio
    │   ├── 📄 UserTests.cs               # Testes da entidade User
    │   ├── 📄 CreateUserRequestTests.cs  # Testes do DTO de criação
    │   ├── 📄 LoginRequestTests.cs       # Testes do DTO de login
    │   ├── 📄 UserResponseTests.cs       # Testes do DTO de resposta
    │   └── 📄 DomainExceptionTests.cs    # Testes de exceções
    │
    ├── 📁 WebApi/                         # 🧪 Testes da Web API
    │   ├── 📄 AuthControllerTests.cs      # Testes do controller de auth
    │   ├── 📄 UsersControllerTests.cs     # Testes do controller de usuários
    │   └── 📄 TestControllerTests.cs      # Testes do controller de teste
    │
    ├── 📁 TestHelpers/                    # 🛠️ Helpers para Testes
    │   ├── 📄 TestDataBuilder.cs         # Builder de dados de teste
    │   ├── 📄 MockServices.cs             # Serviços mockados
    │   └── 📄 TestDatabase.cs            # Banco de dados de teste
    │
    ├── 📁 bin/                           # 📦 Binários compilados
    └── 📁 obj/                           # 🔧 Arquivos temporários
```

## 🎯 Resumo das Camadas

### **1. 🏛️ Domain (Domínio)**
- **Propósito**: Regras de negócio puras, entidades e interfaces
- **Arquivos Principais**: `User.cs`, `BaseEntity.cs`, interfaces
- **Responsabilidade**: Definir o que o sistema faz

### **2. 🧠 Application (Aplicação)**
- **Propósito**: Casos de uso, commands, queries e handlers
- **Arquivos Principais**: Commands, Queries, Handlers, Validators
- **Responsabilidade**: Orquestrar as operações do sistema

### **3. 🔧 Infrastructure (Infraestrutura)**
- **Propósito**: Implementações concretas (banco, serviços externos)
- **Arquivos Principais**: Repositories, Services, DbContext
- **Responsabilidade**: Como o sistema faz (implementação)

### **4. 🌐 WebApi (ASP.NET Core Web API)**
- **Propósito**: Endpoints HTTP tradicionais com Controllers
- **Arquivos Principais**: Controllers, Program.cs, Swagger
- **Responsabilidade**: Interface HTTP da API

### **5. 🧪 Test (Testes)**
- **Propósito**: Testes unitários e de integração
- **Arquivos Principais**: Testes por camada
- **Responsabilidade**: Garantir qualidade do código

## 🔄 Fluxo de Dados

```
Frontend → Web API → Application → Domain ← Infrastructure → Database
    ↓         ↓          ↓           ↓           ↓
   HTTP    Controllers Commands/    Entities    Repositories
  Request  (Auth JWT)   Queries     (Rules)     (EF Core)
```

## 📊 Estatísticas do Projeto

- **Total de Projetos**: 5 (.csproj)
- **Total de Camadas**: 5 (Domain, Application, Infrastructure, WebApi, Test)
- **Total de Controllers**: 3 (Auth, Users, Test)
- **Total de Commands**: 4 (Authenticate, CreateUser, Login, LoginWithToken)
- **Total de Queries**: 2 (GetAllUsers, GetUserById)
- **Total de Handlers**: 6 (um para cada command/query)
- **Total de Validators**: 2 (CreateUser, Login)
- **Total de Repositories**: 2 (Base, User)
- **Total de Services**: 4 (Authentication, JWT, DatabaseInitialization, Token)
- **Total de DTOs**: 5 (Requests e Responses)
- **Total de Entities**: 2 (BaseEntity, User)
- **Total de Testes**: ~20 arquivos de teste
- **Banco de Dados**: 1 único (UniSystem)
