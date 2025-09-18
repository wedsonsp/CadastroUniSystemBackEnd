# Script para aplicar migrations em ambos os bancos
# UniSystem (Produção) e UniSystem_Dev (Desenvolvimento)

Write-Host "🚀 Aplicando migrations em ambos os bancos..." -ForegroundColor Green

# 1. Aplicar migration no banco de produção (UniSystem)
Write-Host "📊 Aplicando migration no banco UniSystem (Produção)..." -ForegroundColor Yellow
dotnet ef database update --startup-project ..\Sistemaws

# 2. Aplicar migration no banco de desenvolvimento (UniSystem_Dev)
Write-Host "🔧 Aplicando migration no banco UniSystem_Dev (Desenvolvimento)..." -ForegroundColor Yellow

# Temporariamente alterar a string de conexão para desenvolvimento
$devConnectionString = "Server=MARCCIELO\\SQLEXPRESS;Database=UniSystem_Dev;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

# Aplicar migration com string de conexão específica
dotnet ef database update --startup-project ..\Sistemaws --connection "$devConnectionString"

Write-Host "✅ Migrations aplicadas com sucesso em ambos os bancos!" -ForegroundColor Green
Write-Host "📊 UniSystem (Produção): Pronto" -ForegroundColor Cyan
Write-Host "🔧 UniSystem_Dev (Desenvolvimento): Pronto" -ForegroundColor Cyan
