using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Sistemaws.Application;
using Sistemaws.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Sistemaws.Infrastructure.Persistence;
using Sistemaws.Infrastructure.Services;

[assembly: FunctionsStartup(typeof(Sistemaws.Function.Startup))]

namespace Sistemaws.Function;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        // JWT Authentication
        var jwtKey = Environment.GetEnvironmentVariable("Jwt:Key") ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
        var jwtIssuer = Environment.GetEnvironmentVariable("Jwt:Issuer") ?? "Sistemaws";
        var jwtAudience = Environment.GetEnvironmentVariable("Jwt:Audience") ?? "SistemawsUsers";

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();

        // Add Application and Infrastructure services
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.GetContext().Configuration);

        // Register JWT Service
        builder.Services.AddScoped<Sistemaws.Domain.Interfaces.Services.IJwtService, Sistemaws.Infrastructure.Services.JwtService>();
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        // Initialize database when Azure Functions start
        builder.ConfigurationBuilder.Build();
        
        // This will be called after services are registered
        var services = new ServiceCollection();
        services.AddApplication();
        services.AddInfrastructure(builder.ConfigurationBuilder.Build());
        
        var serviceProvider = services.BuildServiceProvider();
        
        // Initialize database
        InitializeDatabaseAsync(serviceProvider).GetAwaiter().GetResult();
    }

    private async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SistemawsDbContext>();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializationService>();
            
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();
            
            // Initialize with default admin user
            await dbInitializer.InitializeAsync();
            
            Console.WriteLine("✅ Database initialization completed for Azure Functions");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error initializing database: {ex.Message}");
        }
    }
}
