# 🐳 Setup Docker - Sistema AWS

## 🎯 Objetivo

Este setup permite executar toda a aplicação **sem precisar instalar ou configurar o SQL Server Management Studio** ou qualquer outro gerenciador de banco de dados.

## 🚀 Execução Rápida

### Opção 1: Script PowerShell (Recomendado)
```powershell
# Execute o script PowerShell
.\docker-run.ps1
```

### Opção 2: Comando Manual
```bash
# Executar toda a aplicação
docker-compose up --build
```

## 📋 O que acontece automaticamente

1. **SQL Server**: Container SQL Server 2022 é criado e configurado
2. **Banco de Dados**: Banco `UniSystem` é criado automaticamente
3. **Migrações**: Entity Framework executa as migrações automaticamente
4. **Usuário Admin**: Usuário administrador é criado automaticamente
5. **API**: Aplicação .NET é compilada e executada
6. **Health Check**: API aguarda o banco estar pronto antes de iniciar

## 🌐 Acessos após execução

- **API**: http://localhost:7071
- **Swagger**: http://localhost:7071/swagger
- **SQL Server**: localhost:1433
- **Usuário**: sa
- **Senha**: YourStrong@Passw0rd

## 🔐 Login inicial

- **Email**: admin@admin.com.br
- **Senha**: 123456

## 🛠️ Comandos úteis

```bash
# Ver logs da aplicação
docker-compose logs -f api

# Ver logs do banco
docker-compose logs -f sqlserver

# Parar tudo
docker-compose down

# Parar e remover dados
docker-compose down -v

# Rebuild apenas a API
docker-compose up --build api
```

## 🔍 Verificar se está funcionando

1. Acesse http://localhost:7071/swagger
2. Teste o endpoint `POST /api/auth/login` com:
   - Email: `admin@admin.com.br`
   - Senha: `123456`
3. Use o token retornado para testar outros endpoints

## 📝 Vantagens deste setup

- ✅ **Sem instalação**: Não precisa instalar SQL Server
- ✅ **Sem configuração**: Tudo configurado automaticamente
- ✅ **Isolado**: Não interfere com outros projetos
- ✅ **Reproduzível**: Funciona igual em qualquer máquina
- ✅ **Rápido**: Inicia em poucos minutos
- ✅ **Persistente**: Dados são mantidos entre execuções
