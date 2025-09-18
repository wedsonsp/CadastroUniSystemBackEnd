# Sistemaws Web API

Este é o projeto Web API ASP.NET Core que substitui o Azure Functions, mantendo toda a arquitetura DDD (Domain-Driven Design).

## 🚀 Como Executar

1. **Compilar o projeto:**
   ```bash
   dotnet build
   ```

2. **Executar o projeto:**
   ```bash
   dotnet run --project Sistemaws.WebApi
   ```

3. **Acessar a API:**
   - Swagger UI: `https://localhost:7001/swagger` ou `http://localhost:5000/swagger`
   - Base URL: `https://localhost:7001/api` ou `http://localhost:5000/api`

## 📋 Endpoints Disponíveis

### Autenticação (sem autenticação necessária)
- `POST /api/auth/authenticate` - Autenticar usuário
- `POST /api/auth/login` - Login com email/senha
- `POST /api/auth/login-with-token` - Login com token

### Usuários (requer autenticação)
- `POST /api/users` - Criar usuário
- `GET /api/users` - Listar todos os usuários
- `GET /api/users/{id}` - Buscar usuário por ID

### Teste
- `GET /api/test/public` - Endpoint público (sem autenticação)
- `GET /api/test/test-auth` - Endpoint protegido (requer autenticação)

## 🔐 Autenticação JWT

Para usar endpoints protegidos, você precisa:

1. **Fazer login:**
   ```bash
   POST /api/auth/login
   {
     "email": "admin@sistemaws.com",
     "password": "Admin123!"
   }
   ```

2. **Usar o token retornado:**
   ```bash
   Authorization: Bearer {seu_token_aqui}
   ```

## 🏗️ Arquitetura DDD

O projeto mantém a mesma estrutura DDD:

- **Domain**: Entidades, DTOs, Interfaces
- **Application**: Commands, Queries, Handlers, Validators
- **Infrastructure**: Repositories, Services, Database Context
- **WebApi**: Controllers, Program.cs, Configurações

## 🗄️ Banco de Dados

O banco de dados é inicializado automaticamente na primeira execução com um usuário administrador padrão:
- Email: `admin@sistemaws.com`
- Senha: `Admin123!`

## 🔧 Configurações

As configurações estão no `appsettings.json`:
- Connection String do SQL Server
- Configurações JWT (Key, Issuer, Audience)
- Logging
