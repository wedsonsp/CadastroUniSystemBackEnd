# 🐳 Configuração Docker - Sistema AWS

Este guia explica como configurar e executar o projeto usando Docker **sem precisar do Management Studio**.

## 📋 Pré-requisitos

- Docker Desktop instalado e rodando
- Docker Compose (vem com Docker Desktop)
- .NET 8 SDK (para desenvolvimento local)

## 🚀 Como Executar

### 1. Executar a aplicação completa (API + Banco)

```bash
# Navegar para o diretório do projeto
cd C:\Users\gabri\OneDrive\Área de Trabalho\WedsonValidacaoBrackEnd\CadastroUniSystemBackEnd

# Executar todos os serviços (API + SQL Server)
docker-compose up --build
```

### 2. Executar em background

```bash
# Executar em modo detached (background)
docker-compose up -d --build
```

### 3. Executar apenas o banco de dados

```bash
# Executar apenas o SQL Server
docker-compose up sqlserver
```

## 🔧 Comandos Úteis

### Ver logs dos containers
```bash
# Ver logs de todos os serviços
docker-compose logs

# Ver logs de um serviço específico
docker-compose logs api
docker-compose logs sqlserver
```

### Parar os containers
```bash
# 

Parar todos os serviços
docker-compose down

# Parar e remover volumes (CUIDADO: apaga dados do banco)
docker-compose down -v
```

### Rebuild da API
```bash
# Reconstruir a imagem da API
docker-compose build api

# Reconstruir e executar
docker-compose up --build api
```

## 🌐 Acessos

Após executar o `docker-compose up --build`:

- **API Backend**: http://localhost:7071
- **API HTTPS**: https://localhost:7072
- **SQL Server**: localhost:1433
- **Swagger UI**: http://localhost:7071/swagger

## 📊 Endpoints da API

- `POST /api/auth/login` - Login
- `POST /api/users` - Criar usuário (apenas admins)
- `GET /api/users` - Listar usuários
- `GET /api/users/{id}` - Buscar usuário por ID

## 🗄️ Banco de Dados

- **Servidor**: localhost:1433 (ou `sqlserver` dentro da rede Docker)
- **Usuário**: sa
- **Senha**: YourStrong@Passw0rd
- **Banco**: UniSystem
- **Criação**: Automática via script de inicialização
- **Migrações**: Executadas automaticamente pela aplicação

## 🔐 Usuário Administrador Padrão

Quando o banco está vazio, a aplicação cria automaticamente:
- **Email**: admin@admin.com.br
- **Senha**: 123456
- **Role**: Administrador

## 🔍 Troubleshooting

### Container não inicia
```bash
# Ver logs detalhados da API
docker-compose logs -f api

# Ver logs do SQL Server
docker-compose logs -f sqlserver
```

### Problemas de conexão com banco
```bash
# Verificar se os containers estão rodando
docker-compose ps

# Verificar saúde do SQL Server
docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -Q "SELECT 1"
```

### Aplicação não conecta ao banco
```bash
# Verificar se a API está aguardando o banco
docker-compose logs api | grep -i "connection\|database\|sql"

# Reiniciar apenas a API
docker-compose restart api
```

### Limpar tudo e recomeçar
```bash
# Parar e remover tudo
docker-compose down -v

# Remover imagens
docker system prune -a

# Executar novamente
docker-compose up --build
```

## 🛠️ Desenvolvimento

### Executar migrações manualmente
```bash
# Entrar no container da API
docker-compose exec api bash

# Executar migrações
dotnet ef database update
```

### Acessar o banco via linha de comando
```bash
# Conectar ao SQL Server
docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd

# Listar bancos
SELECT name FROM sys.databases;

# Usar o banco UniSystem
USE UniSystem;

# Listar tabelas
SELECT name FROM sys.tables;
```

## 📝 Notas Importantes

1. **Primeira execução**: Pode demorar alguns minutos para baixar as imagens
2. **Banco de dados**: Os dados persistem entre execuções (volume `sqlserver_data`)
3. **Portas**: Certifique-se de que as portas 7071, 7072 e 1433 estão livres
4. **Desenvolvimento**: Use `docker-compose up --build` após mudanças no código
5. **Health Check**: A API aguarda o banco estar saudável antes de iniciar
6. **Migrações**: Executadas automaticamente na primeira execução
7. **Usuário Admin**: Criado automaticamente se o banco estiver vazio
