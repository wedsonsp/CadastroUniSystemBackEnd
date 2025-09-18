using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Exceptions;
using System.Text.Json;

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

    [Function("CreateUser")]
    public async Task<HttpResponseData> CreateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestData req)
    {
        try
        {
            // Extrair token do header Authorization
            var authHeader = req.Headers.GetValues("Authorization").FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                var unauthorizedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("Authorization header missing or invalid");
                return unauthorizedResponse;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var createUserRequest = JsonSerializer.Deserialize<CreateUserRequest>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (createUserRequest == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid request body");
                return badRequestResponse;
            }

            var command = new CreateUserCommand
            {
                Name = createUserRequest.Name,
                Email = createUserRequest.Email,
                Password = createUserRequest.Password,
                Token = token
            };

            var result = await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));

            return response;
        }
        catch (DomainException ex)
        {
            _logger.LogError("Domain error during user creation: {Errors}", ex.Errors);
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Content-Type", "application/json");
            await errorResponse.WriteStringAsync(JsonSerializer.Serialize(ex.Errors, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
            return errorResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during user creation");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }
}
