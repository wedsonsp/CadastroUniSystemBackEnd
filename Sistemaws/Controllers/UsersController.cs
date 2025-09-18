using Microsoft.AspNetCore.Mvc;
using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Application.Queries;
using Microsoft.AspNetCore.Authorization;

namespace Sistemaws.Controllers;

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
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        try
        {
            // Extrair token do header Authorization
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                command.Token = authHeader.Substring("Bearer ".Length).Trim();
            }
            
            var result = await _mediator.Send(command);
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
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Log the full exception for debugging
            Console.WriteLine($"Error in GetUserById: {ex}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, new { message = ex.Message, details = ex.ToString() });
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
            // Log the full exception for debugging
            Console.WriteLine($"Error in GetAllUsers: {ex}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, new { message = ex.Message, details = ex.ToString() });
        }
    }
}
