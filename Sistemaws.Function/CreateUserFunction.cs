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

public class CreateUserFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<CreateUserFunction> _logger;

    public CreateUserFunction(IMediator mediator, ILogger<CreateUserFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [FunctionName("CreateUser")]
    public async Task<IActionResult> CreateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req)
    {
        try
        {
            // Verificar se há token (opcional para criação de usuário)
            string? token = null;
            var authHeader = req.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                token = authHeader.Substring("Bearer ".Length).Trim();
            }

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var createUserRequest = JsonSerializer.Deserialize<CreateUserRequest>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (createUserRequest == null)
            {
                return new BadRequestObjectResult("Invalid request body");
            }

            var command = new CreateUserCommand
            {
                Name = createUserRequest.Name,
                Email = createUserRequest.Email,
                Password = createUserRequest.Password,
                IsAdministrator = createUserRequest.IsAdministrator,
                Token = token
            };

            var result = await _mediator.Send(command);

            return new CreatedResult("", result);
        }
        catch (DomainException ex)
        {
            _logger.LogError("Domain error during user creation: {Errors}", ex.Errors);
            return new BadRequestObjectResult(ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during user creation");
            return new StatusCodeResult(500);
        }
    }
}
