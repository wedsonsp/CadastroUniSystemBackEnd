using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Sistemaws.Application;
using Sistemaws.Infrastructure;
using Sistemaws.Function.Middleware;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// JWT Authentication - DISABLED for now to test middleware
// var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
// var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "Sistemaws";
// var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SistemawsUsers";

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
//             ValidateIssuer = true,
//             ValidIssuer = jwtIssuer,
//             ValidateAudience = true,
//             ValidAudience = jwtAudience,
//             ValidateLifetime = true,
//             ClockSkew = TimeSpan.Zero
//         };
//     });

// builder.Services.AddAuthorization();

// Add Application Insights with proper configuration
var instrumentationKey = builder.Configuration["ApplicationInsights:InstrumentationKey"];
if (!string.IsNullOrEmpty(instrumentationKey))
{
    builder.Services
        .AddApplicationInsightsTelemetryWorkerService()
        .ConfigureFunctionsApplicationInsights();
}
else
{
    // Log warning about missing Application Insights configuration
    Console.WriteLine("Warning: Application Insights InstrumentationKey not found. Telemetry will be disabled.");
}

// Add Application and Infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Register JWT Service
builder.Services.AddScoped<Sistemaws.Domain.Interfaces.Services.IJwtService, Sistemaws.Infrastructure.Services.JwtService>();

// Register JWT Authentication Middleware
builder.Services.AddScoped<JwtAuthenticationMiddleware>();

// Add custom JWT middleware only for protected endpoints
// Note: This middleware will be applied globally, but it checks function names internally
builder.UseMiddleware<JwtAuthenticationMiddleware>();

var app = builder.Build();

app.Run();
