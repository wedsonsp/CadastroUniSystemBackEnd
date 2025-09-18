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

public class AuthenticateFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthenticateFunction> _logger;

    public AuthenticateFunction(IMediator mediator, ILogger<AuthenticateFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Function("Authenticate")]
    public async Task<HttpResponseData> Authenticate(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/authenticate")] HttpRequestData req)
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
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid request body");
                return badRequestResponse;
            }

            var command = new AuthenticateCommand
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password
            };

            var result = await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            response.Headers.Add("Access-Control-Allow-Credentials", "true");
            await response.WriteStringAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));

            return response;
        }
        catch (DomainException ex)
        {
            _logger.LogError("Domain error during authentication: {Errors}", ex.Errors);
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Content-Type", "application/json");
            errorResponse.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
            errorResponse.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            errorResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            errorResponse.Headers.Add("Access-Control-Allow-Credentials", "true");
            await errorResponse.WriteStringAsync(JsonSerializer.Serialize(ex.Errors, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
            return errorResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during authentication");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            errorResponse.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
            errorResponse.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            errorResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            errorResponse.Headers.Add("Access-Control-Allow-Credentials", "true");
            await errorResponse.WriteStringAsync("Internal server error");
            return errorResponse;
        }
    }

    [Function("AuthenticateOptions")]
    public HttpResponseData AuthenticateOptions(
        [HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "auth/authenticate")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
        response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        response.Headers.Add("Access-Control-Allow-Credentials", "true");
        response.Headers.Add("Access-Control-Max-Age", "86400");
        return response;
    }
}


