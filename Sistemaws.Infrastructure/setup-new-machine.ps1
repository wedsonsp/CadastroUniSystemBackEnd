# Script para configurar o projeto em uma nova máquina
# Cria ambos os bancos e aplica migrations

Write-Host "🚀 Configurando projeto em nova máquina..." -ForegroundColor Green

# Verificar se o SQL Server está acessível
Write-Host "🔍 Verificando conexão com SQL Server..." -ForegroundColor Yellow
$serverName = "MARCCIELO\SQLEXPRESS"
$testConnection = sqlcmd -S $serverName -E -Q "SELECT 1" -h -1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Erro: Não foi possível conectar ao SQL Server!" -ForegroundColor Red
    Write-Host "Verifique se o SQL Server está rodando e acessível." -ForegroundColor Red
    exit 1
}
Write-Host "✅ Conexão com SQL Server OK!" -ForegroundColor Green

# 1. Criar ambos os bancos
Write-Host "📊 Criando bancos de dados..." -ForegroundColor Yellow
$createDatabasesSQL = @"
USE master;
GO

-- Criar banco UniSystem (Produção)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'UniSystem')
BEGIN
    CREATE DATABASE [UniSystem];
    PRINT 'Banco UniSystem criado com sucesso!';
END
ELSE
BEGIN
    PRINT 'Banco UniSystem já existe.';
END

-- Criar banco UniSystem_Dev (Desenvolvimento)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'UniSystem_Dev')
BEGIN
    CREATE DATABASE [UniSystem_Dev];
    PRINT 'Banco UniSystem_Dev criado com sucesso!';
END
ELSE
BEGIN
    PRINT 'Banco UniSystem_Dev já existe.';
END
"@

$createDatabasesSQL | sqlcmd -S $serverName -E

# 2. Aplicar migrations no banco de produção
Write-Host "📊 Aplicando migrations no UniSystem (Produção)..." -ForegroundColor Yellow
dotnet ef database update --startup-project ..\Sistemaws

# 3. Aplicar migrations no banco de desenvolvimento
Write-Host "🔧 Aplicando migrations no UniSystem_Dev (Desenvolvimento)..." -ForegroundColor Yellow
dotnet ef database update --startup-project ..\Sistemaws --connection "Server=MARCCIELO\\SQLEXPRESS;Database=UniSystem_Dev;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

Write-Host "✅ Setup completo! Ambos os bancos foram criados e configurados!" -ForegroundColor Green
Write-Host "📊 UniSystem (Produção): Pronto" -ForegroundColor Cyan
Write-Host "🔧 UniSystem_Dev (Desenvolvimento): Pronto" -ForegroundColor Cyan
Write-Host "🚀 Projeto pronto para rodar!" -ForegroundColor Green

