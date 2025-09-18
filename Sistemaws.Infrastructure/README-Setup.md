# 🚀 Setup do Projeto em Nova Máquina

## 📋 Pré-requisitos
- .NET 8.0 SDK
- SQL Server (local ou remoto)
- Entity Framework Core Tools (`dotnet tool install --global dotnet-ef`)

## 🔧 Configuração Rápida

### 1. **Executar Script Automático (Recomendado)**
```powershell
# Na pasta Sistemaws.Infrastructure
.\setup-new-machine.ps1
```

### 2. **Configuração Manual**

#### **Passo 1: Criar Bancos no SQL Server**
```sql
USE master;
GO

-- Criar banco de produção
CREATE DATABASE [UniSystem];

-- Criar banco de desenvolvimento
CREATE DATABASE [UniSystem_Dev];
```

#### **Passo 2: Aplicar Migrations**
```powershell
# Na pasta Sistemaws.Infrastructure

# Migration para produção
dotnet ef database update --startup-project ..\Sistemaws

# Migration para desenvolvimento
dotnet ef database update --startup-project ..\Sistemaws --connection "Server=MARCCIELO\\SQLEXPRESS;Database=UniSystem_Dev;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

## 🎯 **Resultado Esperado**
- ✅ Banco `UniSystem` criado e configurado
- ✅ Banco `UniSystem_Dev` criado e configurado
- ✅ Tabelas criadas em ambos os bancos
- ✅ Projeto pronto para rodar

## 🔧 **Configurações de Conexão**
- **Produção**: `UniSystem`
- **Desenvolvimento**: `UniSystem_Dev`
- **Servidor**: `MARCCIELO\SQLEXPRESS`
- **Autenticação**: Windows Authentication

## ⚠️ **Importante**
- Ajuste o nome do servidor se necessário
- Verifique se o SQL Server está rodando
- Certifique-se de ter permissões para criar bancos
