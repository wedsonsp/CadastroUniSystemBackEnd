using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistemaws.Application.Commands;
using Sistemaws.Application.Queries;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Exceptions;

namespace Sistemaws.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Cadastra um novo usuário
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var command = new CreateUserCommand
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Lista todos os usuários cadastrados
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
    {
        try
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuários");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Busca um usuário por ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetUserById(int id)
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
            _logger.LogError(ex, "Erro ao buscar usuário por ID");
            return StatusCode(500, "Erro interno do servidor");
        }
    }
}
