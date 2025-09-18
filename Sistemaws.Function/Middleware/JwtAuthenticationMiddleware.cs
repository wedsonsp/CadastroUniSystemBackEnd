using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sistemaws.Function.Middleware;

public class JwtAuthenticationMiddleware : IFunctionsWorkerMiddleware
{
    private readonly IConfiguration _configuration;

    public JwtAuthenticationMiddleware(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        // Log for debugging
        Console.WriteLine($"Middleware executing for function: {context.FunctionDefinition.Name}");
        
        // Check if this is a public function that doesn't require authentication
        var functionName = context.FunctionDefinition.Name;
        Console.WriteLine($"Function name: {functionName}");
        
        // Skip authentication for public functions
        if (functionName == "Authenticate" || 
            functionName == "AuthenticateOptions")
        {
            Console.WriteLine($"Skipping authentication for public function: {functionName}");
            await next(context);
            return;
        }

        // Get HTTP request data for protected endpoints
        var httpRequestData = await context.GetHttpRequestDataAsync();
        if (httpRequestData == null)
        {
            await next(context);
            return;
        }

        // Check for Authorization header
        if (!httpRequestData.Headers.TryGetValues("Authorization", out var authHeaders))
        {
            Console.WriteLine("No Authorization header found");
            await CreateUnauthorizedResponse(context, httpRequestData);
            return;
        }

        var authHeader = authHeaders.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            await CreateUnauthorizedResponse(context, httpRequestData);
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();
        if (string.IsNullOrEmpty(token))
        {
            await CreateUnauthorizedResponse(context, httpRequestData);
            return;
        }

        // Validate JWT token
        Console.WriteLine($"Validating token: {token.Substring(0, Math.Min(50, token.Length))}...");
        if (!ValidateJwtToken(token))
        {
            Console.WriteLine("Token validation failed");
            await CreateUnauthorizedResponse(context, httpRequestData);
            return;
        }
        
        Console.WriteLine("Token validation successful");

        await next(context);
    }

    private bool ValidateJwtToken(string token)
    {
        try
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "Sistemaws";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "SistemawsUsers";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task CreateUnauthorizedResponse(FunctionContext context, HttpRequestData httpRequestData)
    {
        var response = httpRequestData.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync("{\"error\": \"Unauthorized\", \"message\": \"Invalid or missing authentication token\"}");
        
        context.GetInvocationResult().Value = response;
    }
}
