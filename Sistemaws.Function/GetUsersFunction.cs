using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using Sistemaws.Application.Queries;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

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


    [FunctionName("GetUsers")]
    public async Task<IActionResult> GetUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequest req)
    {
        try
        {
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
