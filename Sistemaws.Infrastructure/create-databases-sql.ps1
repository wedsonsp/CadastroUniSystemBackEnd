# Script para criar ambos os bancos usando SQL direto
Write-Host "🚀 Criando ambos os bancos usando SQL direto..." -ForegroundColor Green

# Comando SQL para criar ambos os bancos
$sqlCommand = @"
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

# Executar SQL usando sqlcmd
Write-Host "📊 Criando bancos no SQL Server..." -ForegroundColor Yellow
$sqlCommand | sqlcmd -S "MARCCIELO\SQLEXPRESS" -E

Write-Host "✅ Bancos criados! Agora aplicando migrations..." -ForegroundColor Green

# Aplicar migration no banco de produção
Write-Host "📊 Aplicando migration no UniSystem..." -ForegroundColor Yellow
dotnet ef database update --startup-project ..\Sistemaws

# Aplicar migration no banco de desenvolvimento
Write-Host "🔧 Aplicando migration no UniSystem_Dev..." -ForegroundColor Yellow
dotnet ef database update --startup-project ..\Sistemaws --connection "Server=MARCCIELO\\SQLEXPRESS;Database=UniSystem_Dev;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

Write-Host "🎉 Ambos os bancos foram criados e configurados!" -ForegroundColor Green

