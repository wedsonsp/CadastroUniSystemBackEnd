# Script PowerShell para gerenciar Docker
param(
    [Parameter(Position=0)]
    [ValidateSet("start", "stop", "restart", "logs", "build", "clean", "status")]
    [string]$Action = "start",
    
    [switch]$Frontend,
    [switch]$Force
)

Write-Host "🐳 Sistema AWS - Docker Manager" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan

switch ($Action) {
    "start" {
        Write-Host "🚀 Iniciando serviços..." -ForegroundColor Green
        
        if ($Frontend) {
            Write-Host "Incluindo frontend..." -ForegroundColor Yellow
            docker-compose --profile frontend up -d
        } else {
            Write-Host "Iniciando apenas API e banco de dados..." -ForegroundColor Yellow
            docker-compose up -d api sqlserver
        }
        
        Write-Host "✅ Serviços iniciados!" -ForegroundColor Green
        Write-Host "🌐 API: http://localhost:7071" -ForegroundColor Blue
        Write-Host "🗄️  SQL Server: localhost:1433" -ForegroundColor Blue
        if ($Frontend) {
            Write-Host "💻 Frontend: http://localhost:4200" -ForegroundColor Blue
        }
    }
    
    "stop" {
        Write-Host "🛑 Parando serviços..." -ForegroundColor Yellow
        docker-compose down
        Write-Host "✅ Serviços parados!" -ForegroundColor Green
    }
    
    "restart" {
        Write-Host "🔄 Reiniciando serviços..." -ForegroundColor Yellow
        docker-compose down
        if ($Frontend) {
            docker-compose --profile frontend up -d
        } else {
            docker-compose up -d api sqlserver
        }
        Write-Host "✅ Serviços reiniciados!" -ForegroundColor Green
    }
    
    "logs" {
        Write-Host "📋 Exibindo logs..." -ForegroundColor Yellow
        docker-compose logs -f
    }
    
    "build" {
        Write-Host "🔨 Construindo imagens..." -ForegroundColor Yellow
        docker-compose build
        Write-Host "✅ Imagens construídas!" -ForegroundColor Green
    }
    
    "clean" {
        Write-Host "🧹 Limpando containers e imagens..." -ForegroundColor Yellow
        docker-compose down -v
        docker system prune -f
        Write-Host "✅ Limpeza concluída!" -ForegroundColor Green
    }
    
    "status" {
        Write-Host "📊 Status dos containers:" -ForegroundColor Yellow
        docker-compose ps
    }
}

Write-Host "`n💡 Dicas:" -ForegroundColor Cyan
Write-Host "  - Para incluir frontend: .\docker-run.ps1 start -Frontend" -ForegroundColor Gray
Write-Host "  - Para ver logs: .\docker-run.ps1 logs" -ForegroundColor Gray
Write-Host "  - Para parar: .\docker-run.ps1 stop" -ForegroundColor Gray


