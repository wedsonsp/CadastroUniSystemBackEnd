using Microsoft.AspNetCore.Mvc;
using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.DTOs;

namespace Sistemaws.Controllers;

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
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
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
            var command = new LoginWithTokenCommand { Token = request.Token };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
