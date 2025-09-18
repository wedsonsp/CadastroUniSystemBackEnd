# Script para criar e aplicar migrations em ambos os bancos
# UniSystem (Produção) e UniSystem_Dev (Desenvolvimento)

Write-Host "🚀 Criando e configurando ambos os bancos..." -ForegroundColor Green

# 1. Aplicar migration no banco de produção (UniSystem)
Write-Host "📊 Configurando banco UniSystem (Produção)..." -ForegroundColor Yellow
dotnet ef database update --startup-project ..\Sistemaws

# 2. Criar e configurar banco de desenvolvimento (UniSystem_Dev)
Write-Host "🔧 Configurando banco UniSystem_Dev (Desenvolvimento)..." -ForegroundColor Yellow

# Primeiro, vamos dropar o banco de dev se existir
Write-Host "🗑️ Removendo banco UniSystem_Dev se existir..." -ForegroundColor Red
dotnet ef database drop --startup-project ..\Sistemaws --connection "Server=MARCCIELO\\SQLEXPRESS;Database=UniSystem_Dev;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True" --force

# Agora aplicar migration no banco de desenvolvimento
Write-Host "🔧 Aplicando migration no banco UniSystem_Dev..." -ForegroundColor Yellow
dotnet ef database update --startup-project ..\Sistemaws --connection "Server=MARCCIELO\\SQLEXPRESS;Database=UniSystem_Dev;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

Write-Host "✅ Ambos os bancos foram criados e configurados com sucesso!" -ForegroundColor Green
Write-Host "📊 UniSystem (Produção): Pronto" -ForegroundColor Cyan
Write-Host "🔧 UniSystem_Dev (Desenvolvimento): Pronto" -ForegroundColor Cyan

