using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Application.Queries;
using Sistemaws.Domain.DTOs;

namespace Sistemaws.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous] // Permite criar usuário sem autenticação
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            // Extrair token do header Authorization se disponível
            string? token = null;
            if (Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (authHeader != null && authHeader.StartsWith("Bearer "))
                {
                    token = authHeader.Substring("Bearer ".Length).Trim();
                }
            }

            var command = new CreateUserCommand
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                IsAdministrator = request.IsAdministrator,
                Token = token
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound();
                
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("create-without-token")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUserWithoutToken([FromBody] CreateUserRequest request)
    {
        try
        {
            // Endpoint especial para criar usuários sem token (apenas desenvolvimento)
            var userRepository = HttpContext.RequestServices.GetRequiredService<Sistemaws.Domain.Interfaces.Repositories.IUserRepository>();
            var authService = HttpContext.RequestServices.GetRequiredService<Sistemaws.Domain.Interfaces.Services.IAuthenticationService>();
            
            // Verificar se o email já existe
            if (await userRepository.EmailExistsAsync(request.Email))
            {
                return BadRequest(new { message = "Este email já está sendo usado por outro usuário" });
            }

            // Hash da senha
            var (passwordHash, salt) = await authService.HashPasswordAsync(request.Password);
            
            // Criar usuário diretamente
            var user = new Sistemaws.Domain.Entities.User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash,
                Salt = salt,
                IsActive = true,
                IsAdministrator = request.IsAdministrator,
                CreatedAt = DateTime.UtcNow
            };
            
            var createdUser = await userRepository.CreateAsync(user);
            
            return Ok(new UserResponse
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email,
                CreatedAt = createdUser.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = createdUser.UpdatedAt,
                IsActive = createdUser.IsActive,
                IsAdministrator = createdUser.IsAdministrator
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
