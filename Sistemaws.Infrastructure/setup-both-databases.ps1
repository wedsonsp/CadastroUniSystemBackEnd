# Script para configurar ambos os bancos alternando configurações
Write-Host "🚀 Configurando ambos os bancos..." -ForegroundColor Green

# 1. Configurar para banco de produção (UniSystem)
Write-Host "📊 Configurando para UniSystem (Produção)..." -ForegroundColor Yellow

# Aplicar migration no banco de produção
dotnet ef database update --startup-project ..\Sistemaws

# 2. Configurar para banco de desenvolvimento (UniSystem_Dev)
Write-Host "🔧 Configurando para UniSystem_Dev (Desenvolvimento)..." -ForegroundColor Yellow

# Temporariamente alterar o DesignTimeDbContextFactory para usar UniSystem_Dev
$designTimeFile = "Persistence\DesignTimeDbContextFactory.cs"
$content = Get-Content $designTimeFile -Raw
$newContent = $content -replace "Database=UniSystem", "Database=UniSystem_Dev"
Set-Content $designTimeFile -Value $newContent

# Aplicar migration no banco de desenvolvimento
dotnet ef database update --startup-project ..\Sistemaws

# Restaurar configuração original
$originalContent = $content -replace "Database=UniSystem_Dev", "Database=UniSystem"
Set-Content $designTimeFile -Value $originalContent

Write-Host "✅ Ambos os bancos foram configurados!" -ForegroundColor Green
Write-Host "📊 UniSystem (Produção): Pronto" -ForegroundColor Cyan
Write-Host "🔧 UniSystem_Dev (Desenvolvimento): Pronto" -ForegroundColor Cyan

