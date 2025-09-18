using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using Sistemaws.Application.Queries;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sistemaws.Function;

public class GetUsersFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<GetUsersFunction> _logger;

    public GetUsersFunction(IMediator mediator, ILogger<GetUsersFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    private bool ValidateJwtToken(HttpRequest req)
    {
        try
        {
            var authHeader = req.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                return false;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key") ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
            var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer") ?? "Sistemaws";
            var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience") ?? "SistemawsUsers";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    [FunctionName("GetUsers")]
    public async Task<IActionResult> GetUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req)
    {
        try
        {
            // Validate JWT token
            if (!ValidateJwtToken(req))
            {
                return new UnauthorizedObjectResult("Invalid or missing JWT token");
            }

            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during users retrieval");
            return new StatusCodeResult(500);
        }
    }

    [FunctionName("GetUserById")]
    public async Task<IActionResult> GetUserById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{id}")] HttpRequest req,
        int id)
    {
        try
        {
            // Validate JWT token
            if (!ValidateJwtToken(req))
            {
                return new UnauthorizedObjectResult("Invalid or missing JWT token");
            }

            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return new NotFoundObjectResult("User not found");
            }

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during user retrieval by ID");
            return new StatusCodeResult(500);
        }
    }
}
