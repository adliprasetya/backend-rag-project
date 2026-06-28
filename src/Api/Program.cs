using Chat;
using Document;
using Infrastructure.Persistence;
using Identity;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Workspace;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, config) =>
        config.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddIdentityModule();
    builder.Services.AddWorkspaceModule();
    builder.Services.AddDocumentModule();
    builder.Services.AddChatModule();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Auto-create database in development with retry
    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        for (int i = 0; i < 5; i++)
        {
            try
            {
                db.Database.EnsureCreated();
                break;
            }
            catch (Exception ex) when (i < 4)
            {
                Log.Warning("Database not ready, retrying in 2s... (attempt {Attempt}/5)", i + 1);
                await Task.Delay(2000);
            }
            catch (Exception ex)
            {
                Log.Warning("Database unavailable after 5 attempts. Continuing without DB: {Message}", ex.Message);
            }
        }
    }

    app.UseSerilogRequestLogging();
    app.UseCors("AllowFrontend");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
