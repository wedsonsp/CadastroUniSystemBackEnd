using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Exceptions;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Sistemaws.Function;

public class AuthenticateFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthenticateFunction> _logger;

    public AuthenticateFunction(IMediator mediator, ILogger<AuthenticateFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [FunctionName("Authenticate")]
    public async Task<IActionResult> Authenticate(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/authenticate")] HttpRequest req)
    {
        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var loginRequest = JsonSerializer.Deserialize<LoginRequest>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (loginRequest == null)
            {
                return new BadRequestObjectResult("Invalid request body");
            }

            var command = new AuthenticateCommand
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password
            };

            var result = await _mediator.Send(command);

            return new OkObjectResult(result);
        }
        catch (DomainException ex)
        {
            _logger.LogError("Domain error during authentication: {Errors}", ex.Errors);
            return new BadRequestObjectResult(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during authentication");
            return new StatusCodeResult(500);
        }
    }

    [FunctionName("AuthenticateOptions")]
    public IActionResult AuthenticateOptions(
        [HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "auth/authenticate")] HttpRequest req)
    {
        return new OkResult();
    }
}




