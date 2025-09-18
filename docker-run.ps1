# Script PowerShell para executar a aplicação com Docker
# Execute este script para subir toda a aplicação sem precisar do Management Studio

Write-Host "🐳 Iniciando Sistema AWS com Docker..." -ForegroundColor Green
Write-Host ""

# Verificar se Docker está rodando
try {
    docker version | Out-Null
    Write-Host "✅ Docker está rodando" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker não está rodando. Por favor, inicie o Docker Desktop." -ForegroundColor Red
    exit 1
}

# Verificar se docker-compose existe
if (-not (Get-Command docker-compose -ErrorAction SilentlyContinue)) {
    Write-Host "❌ docker-compose não encontrado. Instale o Docker Desktop." -ForegroundColor Red
    exit 1
}

Write-Host "🚀 Executando docker-compose up --build..." -ForegroundColor Yellow
Write-Host ""

# Executar docker-compose
docker-compose up --build

Write-Host ""
Write-Host "🎉 Aplicação executada com sucesso!" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Acessos disponíveis:" -ForegroundColor Cyan
Write-Host "   • API Backend: http://localhost:7071" -ForegroundColor White
Write-Host "   • Swagger UI: http://localhost:7071/swagger" -ForegroundColor White
Write-Host "   • SQL Server: localhost:1433" -ForegroundColor White
Write-Host ""
Write-Host "🔐 Usuário administrador padrão:" -ForegroundColor Cyan
Write-Host "   • Email: admin@admin.com.br" -ForegroundColor White
Write-Host "   • Senha: 123456" -ForegroundColor White
Write-Host ""
Write-Host "💡 Para parar a aplicação, pressione Ctrl+C" -ForegroundColor Yellow
