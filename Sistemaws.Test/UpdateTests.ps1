# Script para atualizar todos os testes para usar Assert do XUnit

# Lista de arquivos para atualizar
$files = @(
    "Domain\DTOs\LoginRequestTests.cs",
    "Domain\DTOs\CreateUserRequestTests.cs", 
    "Domain\DTOs\UserResponseTests.cs",
    "Domain\DTOs\LoginResponseTests.cs",
    "Domain\Exceptions\DomainExceptionTests.cs",
    "Application\Commands\LoginCommandTests.cs",
    "Application\Commands\CreateUserCommandTests.cs",
    "Application\Queries\GetAllUsersQueryTests.cs",
    "Application\Queries\GetUserByIdQueryTests.cs"
)

foreach ($file in $files) {
    $content = Get-Content $file -Raw
    
    # Substituições para FluentAssertions -> Assert
    $content = $content -replace '\.Should\(\)\.NotBeNull\(\)', '.Should().NotBeNull()'
    $content = $content -replace '\.Should\(\)\.Be\(', '.Should().Be('
    $content = $content -replace '\.Should\(\)\.BeEmpty\(\)', '.Should().BeEmpty()'
    $content = $content -replace '\.Should\(\)\.Contain\(', '.Should().Contain('
    $content = $content -replace '\.Should\(\)\.HaveCount\(', '.Should().HaveCount('
    $content = $content -replace '\.Should\(\)\.BeOfType<', '.Should().BeOfType<'
    $content = $content -replace '\.Should\(\)\.BeAssignableTo<', '.Should().BeAssignableTo<'
    
    Set-Content $file $content
}

Write-Host "Testes atualizados com sucesso!"


