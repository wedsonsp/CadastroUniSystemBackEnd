# Script para corrigir todos os testes removendo FluentAssertions

$files = @(
    "Domain\DTOs\UserResponseTests.cs",
    "Domain\DTOs\LoginResponseTests.cs", 
    "Domain\Exceptions\DomainExceptionTests.cs",
    "Application\Commands\LoginCommandTests.cs",
    "Application\Commands\CreateUserCommandTests.cs",
    "Application\Queries\GetAllUsersQueryTests.cs",
    "Application\Queries\GetUserByIdQueryTests.cs"
)

foreach ($file in $files) {
    if (Test-Path $file) {
        $content = Get-Content $file -Raw
        
        # Substituições para FluentAssertions -> Assert
        $content = $content -replace '\.Should\(\)\.NotBeNull\(\)', 'Assert.NotNull($1)'
        $content = $content -replace '(\w+)\.Should\(\)\.Be\(([^)]+)\)', 'Assert.Equal($2, $1)'
        $content = $content -replace '(\w+)\.Should\(\)\.BeEmpty\(\)', 'Assert.Empty($1)'
        $content = $content -replace '(\w+)\.Should\(\)\.Contain\(([^)]+)\)', 'Assert.Contains($2, $1)'
        $content = $content -replace '(\w+)\.Should\(\)\.HaveCount\(([^)]+)\)', 'Assert.Equal($2, $1.Count)'
        $content = $content -replace '(\w+)\.Should\(\)\.BeOfType<([^>]+)>\(\)', 'Assert.IsType<$2>($1)'
        $content = $content -replace '(\w+)\.Should\(\)\.BeAssignableTo<([^>]+)>\(\)', 'Assert.IsAssignableFrom<$2>($1)'
        $content = $content -replace '(\w+)\.Should\(\)\.NotBeEmpty\(\)', 'Assert.NotEmpty($1)'
        $content = $content -replace '(\w+)\.Should\(\)\.BeTrue\(\)', 'Assert.True($1)'
        $content = $content -replace '(\w+)\.Should\(\)\.BeFalse\(\)', 'Assert.False($1)'
        $content = $content -replace '(\w+)\.Should\(\)\.BeGreaterThan\(([^)]+)\)', 'Assert.True($1 > $2)'
        $content = $content -replace '(\w+)\.Should\(\)\.BeCloseTo\(([^,]+),\s*([^)]+)\)', 'Assert.True(Math.Abs($1 - $2) <= $3.TotalSeconds)'
        
        Set-Content $file $content
        Write-Host "Arquivo $file atualizado"
    }
}

Write-Host "Todos os testes foram atualizados!"
