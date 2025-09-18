# Sistemaws API

Uma API REST desenvolvida em .NET Core 8 seguindo os princípios de Clean Architecture, com autenticação JWT e CQRS. A API utiliza ASP.NET Core Web API para fornecer endpoints escaláveis e seguros.

## Funcionalidades

- ✅ **Autenticação JWT** - Sistema completo de autenticação com tokens seguros
- ✅ **Autorização por Roles** - Controle de acesso baseado em administradores
- ✅ **Cadastro de usuários** - Apenas administradores podem criar novos usuários
- ✅ **Listagem de usuários** - Consulta de usuários com autenticação obrigatória
- ✅ **Validação de dados** - FluentValidation para validação robusta
- ✅ **Arquitetura limpa** - Separação clara de responsabilidades
- ✅ **CQRS** - Command Query Responsibility Segregation
- ✅ **Entity Framework Core** - ORM com SQL Server
- ✅ **Swagger/OpenAPI** - Documentação interativa da API
- ✅ **CORS** - Configurado para desenvolvimento frontend

## Estrutura do Projeto

```
Sistemaws/
├── Sistemaws.Domain/           # Entidades, interfaces e DTOs
├── Sistemaws.Application/      # Commands, Queries, Handlers e Validators
├── Sistemaws.Infrastructure/   # Repositórios, serviços e DbContext
├── Sistemaws.WebApi/           # ASP.NET Core Web API (API principal)
└── Sistemaws.Test/             # Testes unitários
```

## Tecnologias Utilizadas

- **.NET Core 8** - Framework principal
- **ASP.NET Core Web API** - Runtime da API
- **Entity Framework Core** - ORM
- **SQL Server** - Banco de dados único
- **JWT Authentication** - Autenticação segura
- **MediatR** - Implementação CQRS
- **FluentValidation** - Validação de dados
- **Swagger/OpenAPI** - Documentação da API

## Como Executar

### Pré-requisitos
- .NET 8 SDK
- SQL Server (LocalDB ou Express)

### Executar Web API
```bash
# Navegar para o projeto Web API
cd Sistemaws.WebApi

# Instalar dependências
dotnet restore

# Executar localmente
dotnet run
```

### Acessar a API
- **Web API**: `http://localhost:7201`
- **Swagger UI**: `http://localhost:7201/swagger`

## 🔐 Regras de Negócio e Endpoints

### **Arquitetura de Autenticação Simplificada**

A API utiliza uma arquitetura simplificada com apenas **um endpoint de autenticação**:

| Endpoint | Método | Propósito | Autenticação | Autorização |
|----------|--------|-----------|--------------|-------------|
| `/api/auth/authenticate` | POST | Gerar token JWT | ❌ Não | ❌ Não |
| `/api/users` | GET | Listar usuários | ✅ Sim | ❌ Não |
| `/api/users` | POST | Criar usuário | ✅ Sim | ✅ Admin |
| `/api/users/{id}` | GET | Buscar usuário | ✅ Sim | ❌ Não |

### **Fluxo de Autenticação**

1. **Login**: Cliente envia email + senha → Recebe token JWT
2. **Acesso**: Cliente usa token em todas as requisições protegidas
3. **Autorização**: Sistema verifica se usuário é administrador para operações restritas

### **Endpoints Detalhados**

#### 🔑 **Autenticação**
```http
POST /api/auth/authenticate
Content-Type: application/json

{
  "email": "joao.silva@email.com",
  "password": "MinhaSenh@123"
}
```

**Resposta de Sucesso (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "João Silva",
    "email": "joao.silva@email.com",
    "isActive": true,
    "isAdministrator": true,
    "createdAt": "2025-01-09T19:00:00Z",
    "updatedAt": null
  }
}
```

#### 👥 **Usuários**

**Listar Usuários (Requer Autenticação)**
```http
GET /api/users
Authorization: Bearer {seu-jwt-token}
```

**Criar Usuário (Requer Autenticação + Ser Administrador)**
```http
POST /api/users
Authorization: Bearer {seu-jwt-token}
Content-Type: application/json

{
  "name": "Maria Silva",
  "email": "maria.silva@email.com",
  "password": "MinhaSenh@456"
}
```

**Buscar Usuário por ID (Requer Autenticação)**
```http
GET /api/users/1
Authorization: Bearer {seu-jwt-token}
```

## 📋 Regras de Negócio Detalhadas

### **🔐 Sistema de Autenticação**

#### **Validação de Credenciais**
- ✅ Email deve existir no banco de dados
- ✅ Senha deve corresponder ao hash armazenado
- ✅ Usuário deve estar ativo (`IsActive = true`)
- ✅ Usuário não pode estar deletado (`Deleted = false`)

#### **Geração de Token JWT**
- ✅ Token válido por **24 horas**
- ✅ Contém informações do usuário (ID, Email, IsAdministrator)
- ✅ Assinado com chave secreta configurada

### **👥 Sistema de Usuários**

#### **Criação de Usuários**
- ✅ **Apenas administradores** podem criar novos usuários
- ✅ Email deve ser único no sistema
- ✅ Senha é automaticamente hasheada com SHA256 + Salt
- ✅ Novos usuários são criados como **não-administradores** por padrão
- ✅ Usuário é criado como ativo (`IsActive = true`)

#### **Validações de Dados**
- ✅ **Nome**: Obrigatório, máximo 100 caracteres
- ✅ **Email**: Obrigatório, formato válido, máximo 255 caracteres
- ✅ **Senha**: Obrigatória, mínimo 6 caracteres

#### **Consultas de Usuários**
- ✅ Todos os usuários autenticados podem listar usuários
- ✅ Todos os usuários autenticados podem buscar usuários por ID
- ✅ Sistema retorna apenas usuários não deletados

### **🛡️ Sistema de Autorização**

#### **Middleware de Autenticação**
- ✅ Intercepta todas as requisições para endpoints protegidos
- ✅ Valida token JWT no header `Authorization: Bearer {token}`
- ✅ Permite acesso apenas a usuários autenticados
- ✅ Retorna erro 401 para tokens inválidos ou ausentes

#### **Controle de Acesso por Roles**
- ✅ **Administradores**: Podem criar usuários
- ✅ **Usuários Comuns**: Não podem criar usuários
- ✅ Sistema verifica `IsAdministrator` no token JWT

### **📊 Códigos de Resposta HTTP**

| Código | Significado | Quando Ocorre |
|--------|-------------|---------------|
| **200** | Sucesso | Operação realizada com sucesso |
| **400** | Bad Request | Dados inválidos ou credenciais incorretas |
| **401** | Unauthorized | Token inválido ou ausente |
| **403** | Forbidden | Usuário não tem permissão (ex: não é admin) |
| **404** | Not Found | Usuário não encontrado |
| **500** | Internal Server Error | Erro interno do servidor |

## ⚙️ Configuração

### **Banco de Dados**
A aplicação usa SQL Server com Entity Framework Core. O banco é criado automaticamente na primeira execução.

**String de Conexão:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=MARCCIELO\\SQLEXPRESS;Database=UniSystem;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

**Banco Único:**
- ✅ **Simplicidade**: Um único banco de dados para toda a aplicação
- ✅ **Transações ACID**: Garantia de consistência entre operações
- ✅ **Manutenção**: Backup/restore simplificado
- ✅ **Performance**: Menos overhead de conexões múltiplas

### **JWT Configuration**
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "Sistemaws",
    "Audience": "SistemawsUsers",
    "ExpirationHours": 24
  }
}
```

### **Usuários de Teste**

#### **Administrador Padrão**
- **Email**: `admin@admin.com.br`
- **Senha**: `Admin123!`
- **Role**: Administrador
- **Status**: Ativo

#### **Usuários Criados pelo Admin**
- **Email**: `roberta.silva@gmail.com`
- **Senha**: `123456`
- **Role**: Administrador
- **Status**: Ativo

- **Email**: `marcio.antunes@gmail.com`
- **Senha**: `123456`
- **Role**: Usuário Comum
- **Status**: Ativo

### **Criando Usuários de Teste**

Para criar usuários de teste no banco de dados, execute o script SQL:

```sql
-- Criar usuário administrador
INSERT INTO Users (Name, Email, PasswordHash, Salt, IsActive, IsAdministrator, Deleted, CreatedAt)
VALUES ('João Silva', 'joao.silva@email.com', 'HASH_DA_SENHA', 'SALT_ALEATORIO', 1, 1, 0, GETDATE());
```

## 🏗️ Arquitetura

### **Clean Architecture**
- **Domain**: Entidades, interfaces e regras de negócio puras
- **Application**: Casos de uso, commands, queries e handlers
- **Infrastructure**: Implementações concretas (repositórios, serviços, DbContext)
- **WebApi**: ASP.NET Core Web API com Controllers

### **CQRS (Command Query Responsibility Segregation)**
- **Commands**: Operações que modificam dados
  - `CreateUserCommand` - Criar novo usuário
  - `AuthenticateCommand` - Autenticar usuário
- **Queries**: Operações que apenas leem dados
  - `GetAllUsersQuery` - Listar todos os usuários
  - `GetUserByIdQuery` - Buscar usuário por ID
- **Handlers**: Processam commands e queries usando MediatR

### **Segurança Implementada**
- ✅ **Hash de Senhas**: SHA256 + Salt único por usuário
- ✅ **JWT Tokens**: Assinados com chave secreta, válidos por 24h
- ✅ **Validação**: FluentValidation para todos os inputs
- ✅ **Middleware**: Autenticação automática em endpoints protegidos
- ✅ **Autorização**: Controle de acesso baseado em roles (Admin/User)

### **ASP.NET Core Web API**
- ✅ **Controllers**: Endpoints REST tradicionais
- ✅ **Middleware**: Pipeline de requisições configurável
- ✅ **Dependency Injection**: Container DI nativo
- ✅ **Swagger**: Documentação interativa automática
- ✅ **CORS**: Configurado para desenvolvimento frontend

## 🗄️ Banco de Dados

### **Estrutura da Tabela Users**
```sql
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Salt] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [IsAdministrator] bit NOT NULL,
    [Deleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [CreatedBy] int NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
```

### **Migrações**
```bash
# Criar migração
dotnet ef migrations add InitialCreate --project Sistemaws.Infrastructure --startup-project Sistemaws.WebApi

# Aplicar migração
dotnet ef database update --project Sistemaws.Infrastructure --startup-project Sistemaws.WebApi
```

## 🧪 Testes

### **Testando com Postman**

#### **1. Autenticação**
```http
POST http://localhost:7201/api/auth/authenticate
Content-Type: application/json

{
  "email": "admin@admin.com.br",
  "password": "Admin123!"
}
```

#### **2. Listar Usuários**
```http
GET http://localhost:7201/api/users
Authorization: Bearer {token-do-passo-1}
```

#### **3. Criar Usuário (Admin)**
```http
POST http://localhost:7201/api/users
Authorization: Bearer {token-do-passo-1}
Content-Type: application/json

{
  "name": "Maria Silva",
  "email": "maria.silva@email.com",
  "password": "MinhaSenh@456"
}
```

### **Testando com PowerShell**
```powershell
# Autenticação
$body = @{ email = "admin@admin.com.br"; password = "Admin123!" } | ConvertTo-Json
$response = Invoke-RestMethod -Uri "http://localhost:7201/api/auth/authenticate" -Method POST -ContentType "application/json" -Body $body
$token = $response.token

# Listar usuários
$headers = @{ Authorization = "Bearer $token" }
$users = Invoke-RestMethod -Uri "http://localhost:7201/api/users" -Method GET -Headers $headers
```

## 🚀 Desenvolvimento

### **Adicionando Novas Funcionalidades**

1. **Domain**: Crie entidades e interfaces
2. **Application**: Implemente commands/queries e handlers
3. **Infrastructure**: Implemente repositórios e serviços
4. **WebApi**: Crie Controllers para expor endpoints

### **Validações com FluentValidation**
```csharp
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
```

## 🌐 Frontend Integration

### **AuthService para Angular**
```typescript
@Injectable()
export class AuthService {
  private apiUrl = 'http://localhost:7201/api';
  
  // ✅ CORRETO: Usar apenas /authenticate
  authenticate(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/authenticate`, {
      email, password
    });
  }
  
  // ✅ CORRETO: Usar token em endpoints protegidos
  getUsers(): Observable<User[]> {
    const token = this.getToken();
    return this.http.get<User[]>(`${this.apiUrl}/users`, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }
  
  // ✅ CORRETO: Criar usuários com token
  createUser(userData: any): Observable<User> {
    const token = this.getToken();
    return this.http.post<User>(`${this.apiUrl}/users`, userData, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }
}
```

### **Fluxo no Frontend**
1. **Login**: Chama `/api/auth/authenticate` com email + senha
2. **Armazena Token**: Salva JWT no localStorage/sessionStorage
3. **Requisições**: Inclui `Authorization: Bearer {token}` em todas as chamadas
4. **Logout**: Remove token do storage

## 📚 Recursos Adicionais

- **ASP.NET Core Web API**: [docs.microsoft.com/aspnet/core/web-api](https://docs.microsoft.com/aspnet/core/web-api)
- **JWT Authentication**: [jwt.io](https://jwt.io)
- **Clean Architecture**: [blog.cleancoder.com](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- **CQRS Pattern**: [martinfowler.com/bliki/CQRS.html](https://martinfowler.com/bliki/CQRS.html)
- **Swagger/OpenAPI**: [swagger.io](https://swagger.io)

## 📄 Licença

Este projeto é apenas para fins educacionais/demonstrativos.
