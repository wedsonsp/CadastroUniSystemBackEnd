# 🐳 Configuração Docker - Sistema AWS

Este guia explica como configurar e executar o projeto usando Docker.

## 📋 Pré-requisitos

- Docker Desktop instalado e rodando
- Docker Compose (vem com Docker Desktop)

## 🚀 Como Executar

### 1. Executar apenas a API (Backend)

```bash
# Navegar para o diretório do projeto
cd C:\Users\gabri\OneDrive\Documentos\ProjetoWedson

# Executar apenas a API e banco de dados



```

### 2. 
Executar tudo (API + Frontend)

```bash
# Executar todos os serviços
docker-compose --profile frontend up
```

### 3. Executar em background

```bash
# Executar em modo detached (background)
docker-compose up -d
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

Após executar o `docker-compose up`:

- **API Backend**: http://localhost:7071
- **SQL Server**: localhost:1433
- **Frontend** (se executado): http://localhost:4200

## 📊 Endpoints da API

- `POST /api/auth/login` - Login
- `POST /api/users` - Criar usuário
- `GET /api/users` - Listar usuários
- `GET /api/users/{id}` - Buscar usuário por ID

## 🗄️ Banco de Dados

- **Servidor**: localhost:1433
- **Usuário**: sa
- **Senha**: YourStrong@Passw0rd
- **Banco**: UniSystem

## 🔍 Troubleshooting

### Container não inicia
```bash
# Ver logs detalhados
docker-compose logs -f api
```

### Problemas de conexão com banco
```bash
# Verificar se o SQL Server está rodando
docker-compose ps
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

## 📝 Notas Importantes

1. **Primeira execução**: Pode demorar alguns minutos para baixar as imagens
2. **Banco de dados**: Os dados persistem entre execuções (volume `sqlserver_data`)
3. **Portas**: Certifique-se de que as portas 7071 e 1433 estão livres
4. **Desenvolvimento**: Use `docker-compose up --build` após mudanças no código
