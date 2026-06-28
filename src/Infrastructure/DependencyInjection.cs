using System.Reflection;
using System.Text;
using Infrastructure.Auth;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SharedKernel;
using SharedKernel.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddPersistence(config);
        services.AddAuth(config);
        services.AddCors(config);
        services.AddSingleton<IJwtService, JwtService>();
        return services;
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default"),
                npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    npgsql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);

                }));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
    }

    private static void AddAuth(this IServiceCollection services, IConfiguration config)
    {
        var jwtSettings = config.GetSection("Jwt").Get<JwtSettings>()
            ?? throw new InvalidOperationException("Jwt settings not configured");

        services.Configure<JwtSettings>(config.GetSection("Jwt"));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                };
            });

        services.AddAuthorization();
    }

    private static void AddCors(this IServiceCollection services, IConfiguration config)
    {
        var origins = config.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? ["http://localhost:5173", "http://localhost:5174"];

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
    }
}
