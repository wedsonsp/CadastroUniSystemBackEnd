using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.DTOs;

namespace Sistemaws.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
    {
        try
        {
            var command = new AuthenticateCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("reset-admin-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetAdminPassword()
    {
        try
        {
            // Endpoint temporário para resetar senha do admin
            // Use apenas em desenvolvimento!
            
            var userRepository = HttpContext.RequestServices.GetRequiredService<Sistemaws.Domain.Interfaces.Repositories.IUserRepository>();
            var authService = HttpContext.RequestServices.GetRequiredService<Sistemaws.Domain.Interfaces.Services.IAuthenticationService>();
            
            // Verificar se o admin já existe
            var existingUser = await userRepository.GetByEmailAsync("admin@admin.com.br");
            
            if (existingUser != null)
            {
                // Atualizar senha do admin existente
                var (passwordHash, salt) = await authService.HashPasswordAsync("Admin123!");
                
                existingUser.PasswordHash = passwordHash;
                existingUser.Salt = salt;
                existingUser.IsAdministrator = true;
                existingUser.IsActive = true;
                
                await userRepository.UpdateAsync(existingUser);
                
                return Ok(new { 
                    message = "Senha do admin atualizada com sucesso!", 
                    user = new {
                        Id = existingUser.Id,
                        Name = existingUser.Name,
                        Email = existingUser.Email,
                        IsAdministrator = existingUser.IsAdministrator
                    }
                });
            }
            else
            {
                // Criar novo admin
                var (passwordHash, salt) = await authService.HashPasswordAsync("Admin123!");
                
                var newUser = new Sistemaws.Domain.Entities.User
                {
                    Name = "Administrador",
                    Email = "admin@admin.com.br",
                    PasswordHash = passwordHash,
                    Salt = salt,
                    IsActive = true,
                    IsAdministrator = true,
                    CreatedAt = DateTime.UtcNow
                };
                
                var createdUser = await userRepository.CreateAsync(newUser);
                
                return Ok(new { 
                    message = "Admin criado com sucesso!", 
                    user = new {
                        Id = createdUser.Id,
                        Name = createdUser.Name,
                        Email = createdUser.Email,
                        IsAdministrator = createdUser.IsAdministrator
                    }
                });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login-with-token")]
    public async Task<IActionResult> LoginWithToken([FromBody] LoginWithTokenRequest request)
    {
        try
        {
            var command = new LoginWithTokenCommand
            {
                Token = request.Token
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
