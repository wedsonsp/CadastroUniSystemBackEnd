# Sistemaws Azure Functions

Azure Functions implementadas para autenticação, criação e listagem de usuários usando Clean Architecture e CQRS.

## Functions Implementadas

### 1. AuthFunction - Autenticação
- **Endpoint**: `POST /api/auth/login`
- **Descrição**: Autentica um usuário e retorna um token JWT
- **Body**:
  ```json
  {
    "email": "usuario@email.com",
    "password": "senha123"
  }
  ```
- **Response**:
  ```json
  {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "name": "João Silva",
      "email": "usuario@email.com",
      "createdAt": "2024-01-01T00:00:00Z",
      "isActive": true
    }
  }
  ```

### 2. CreateUserFunction - Criar Usuário
- **Endpoint**: `POST /api/users`
- **Descrição**: Cadastra um novo usuário no sistema
- **Body**:
  ```json
  {
    "name": "João Silva",
    "email": "usuario@email.com",
    "password": "senha123"
  }
  ```
- **Response**:
  ```json
  {
    "id": 1,
    "name": "João Silva",
    "email": "usuario@email.com",
    "createdAt": "2024-01-01T00:00:00Z",
    "isActive": true
  }
  ```

### 3. GetUsersFunction - Listar Usuários
- **Endpoint**: `GET /api/users`
- **Descrição**: Lista todos os usuários cadastrados
- **Response**:
  ```json
  [
    {
      "id": 1,
      "name": "João Silva",
      "email": "usuario@email.com",
      "createdAt": "2024-01-01T00:00:00Z",
      "isActive": true
    }
  ]
  ```

- **Endpoint**: `GET /api/users/{id}`
- **Descrição**: Busca um usuário específico por ID
- **Response**:
  ```json
  {
    "id": 1,
    "name": "João Silva",
    "email": "usuario@email.com",
    "createdAt": "2024-01-01T00:00:00Z",
    "isActive": true
  }
  ```

## Como Executar

### Pré-requisitos
- .NET 8 SDK
- Azure Functions Core Tools
- SQL Server LocalDB

### Executar Localmente
```bash
cd Sistemaws.Function
func start
```

### Executar com dotnet
```bash
cd Sistemaws.Function
dotnet run
```

## Configuração

As configurações estão no arquivo `local.settings.json`:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "ConnectionStrings:DefaultConnection": "Server=MARCCIELO\\SQLEXPRESS;Database=UniSystem;Integrated Security=True;MultipleActiveResultSets=true",
    "Jwt:Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Jwt:Issuer": "Sistemaws",
    "Jwt:Audience": "SistemawsUsers"
  }
}
```

## Arquitetura

As Functions seguem os mesmos princípios da API principal:

- **Clean Architecture**: Separação clara de responsabilidades
- **CQRS**: Uso de Commands e Queries com MediatR
- **Dependency Injection**: Configuração automática de serviços
- **Validação**: FluentValidation para validação de dados
- **Tratamento de Erros**: Exceções de domínio tratadas adequadamente

## Testando as Functions

### Usando curl

1. **Criar usuário**:
```bash
curl -X POST http://localhost:7071/api/users \
  -H "Content-Type: application/json" \
  -d '{"name":"João Silva","email":"joao@email.com","password":"123456"}'
```

2. **Fazer login**:
```bash
curl -X POST http://localhost:7071/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"joao@email.com","password":"123456"}'
```

3. **Listar usuários**:
```bash
curl -X GET http://localhost:7071/api/users
```

4. **Buscar usuário por ID**:
```bash
curl -X GET http://localhost:7071/api/users/1
```

### Usando Postman

Importe as seguintes requisições:

- **POST** `http://localhost:7071/api/users`
- **POST** `http://localhost:7071/api/auth/login`
- **GET** `http://localhost:7071/api/users`
- **GET** `http://localhost:7071/api/users/{id}`

## Deploy para Azure

Para fazer deploy das Functions para Azure:

1. **Instalar Azure CLI**:
```bash
az login
```

2. **Criar Function App**:
```bash
az functionapp create --resource-group myResourceGroup --consumption-plan-location westeurope --runtime dotnet-isolated --runtime-version 8.0 --functions-version 4 --name myFunctionApp --storage-account mystorageaccount
```

3. **Fazer deploy**:
```bash
func azure functionapp publish myFunctionApp
```

## Logs e Monitoramento

As Functions incluem:
- Application Insights para monitoramento
- Logging estruturado com ILogger
- Tratamento de exceções com logs detalhados

## Segurança

- Hash de senhas com SHA256 + Salt
- Tokens JWT com configuração segura
- Validação de entrada com FluentValidation
- Tratamento seguro de exceções
