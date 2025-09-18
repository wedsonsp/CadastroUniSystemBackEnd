using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Sistemaws.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("test-auth")]
    [Authorize]
    public IActionResult TestAuth()
    {
        var user = HttpContext.User;
        var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();
        
        return Ok(new
        {
            message = "Authentication successful!",
            user = user.Identity?.Name,
            claims = claims,
            isAuthenticated = user.Identity?.IsAuthenticated
        });
    }

    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok(new { message = "This is a public endpoint" });
    }
}
