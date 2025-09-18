# 🚀 Sistema de Usuários - Setup Automático

## 📋 Pré-requisitos

- **.NET 8.0 SDK** instalado
- **SQL Server** (local ou remoto)
- **Visual Studio** ou **VS Code** (opcional)

## ⚡ Setup Automático

### 1. **Clone o repositório**
```bash
git clone <seu-repositorio>
cd ProjetoWedson
```

### 2. **Execute o setup automático**
```bash
# Windows PowerShell
powershell -ExecutionPolicy Bypass -File setup-automatic.ps1

# Ou manualmente:
dotnet restore
dotnet build
```

### 3. **Inicie as aplicações**
```bash
# Opção 1: Script automático (recomendado)
powershell -ExecutionPolicy Bypass -File start-applications.ps1

# Opção 2: Manual (duas janelas separadas)
# Janela 1 - API
dotnet run --project Sistemaws\Sistemaws.csproj

# Janela 2 - Azure Functions  
dotnet run --project Sistemaws.Function\Sistemaws.Function.csproj
```

## 🌐 Endpoints Disponíveis

### **API (.NET Core) - Porta 7201**
- **Login:** `POST http://localhost:7201/api/auth/authenticate`
- **Listar Usuários:** `GET http://localhost:7201/api/users`
- **Buscar Usuário:** `GET http://localhost:7201/api/users/{id}`
- **Criar Usuário:** `POST http://localhost:7201/api/users`

### **Azure Functions - Porta 7205**
- **Login:** `POST http://localhost:7205/api/auth/authenticate`
- **Listar Usuários:** `GET http://localhost:7205/api/users`
- **Buscar Usuário:** `GET http://localhost:7205/api/users/{id}`
- **Criar Usuário:** `POST http://localhost:7205/api/users`

## 🔐 Usuário Admin Padrão

**Email:** `admin@admin.com.br`  
**Senha:** `123456`

*Este usuário é criado automaticamente quando o banco está vazio.*

## 🗄️ Bancos de Dados

- **Desenvolvimento:** `UniSystem_Dev`
- **Produção:** `UniSystem`

*Os bancos são criados automaticamente na primeira execução.*

## 🛠️ Comandos Úteis

```bash
# Parar todas as aplicações
powershell -ExecutionPolicy Bypass -File stop-applications.ps1

# Ou manualmente
taskkill /F /IM dotnet.exe

# Recompilar tudo
dotnet clean
dotnet build

# Aplicar migrations (se necessário)
cd Sistemaws.Infrastructure
dotnet ef database update --startup-project ..\Sistemaws
```

## 🚨 Solução de Problemas

### **Erro de Porta em Uso**
```bash
# Parar todas as aplicações
taskkill /F /IM dotnet.exe

# Aguardar 5 segundos e reiniciar
powershell -ExecutionPolicy Bypass -File start-applications.ps1
```

### **Erro de CORS**
- Verifique se está usando a porta correta (7201 para API, 7205 para Functions)
- Certifique-se de que apenas uma aplicação está rodando por porta

### **Erro de Banco de Dados**
- Verifique se o SQL Server está rodando
- Confirme a string de conexão em `appsettings.json`
- Execute as migrations se necessário

## 📦 Deploy para Produção

### **1. Configurar appsettings.Production.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=UniSystem;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_SECRETA_PRODUCAO",
    "Issuer": "SistemawsProduction",
    "Audience": "SistemawsProductionUsers"
  }
}
```

### **2. Deploy**
```bash
# Build para produção
dotnet publish -c Release

# Deploy da API
dotnet run --project Sistemaws\Sistemaws.csproj --environment Production

# Deploy das Functions
dotnet run --project Sistemaws.Function\Sistemaws.Function.csproj --environment Production
```

## ✅ Checklist de Verificação

- [ ] .NET 8.0 SDK instalado
- [ ] SQL Server rodando
- [ ] Aplicações iniciadas sem erros
- [ ] Login funcionando com admin@admin.com.br
- [ ] Endpoints respondendo corretamente
- [ ] Banco de dados criado automaticamente

## 🎯 Próximos Passos

1. **Teste todos os endpoints** no Postman
2. **Configure seu frontend** para usar as URLs corretas
3. **Personalize as configurações** conforme necessário
4. **Deploy para produção** quando estiver pronto

---

**🎉 Sistema pronto para uso!** As aplicações são iniciadas automaticamente nas portas corretas e o usuário admin é criado automaticamente.
