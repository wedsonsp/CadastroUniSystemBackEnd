using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Domain.Interfaces.Services;
using Sistemaws.Infrastructure.Persistence;
using Sistemaws.Infrastructure.Repositories;
using Sistemaws.Infrastructure.Services;

namespace Sistemaws.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<SistemawsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

        // Services
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<DatabaseInitializationService>();

        return services;
    }
}