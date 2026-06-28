using Infrastructure;
using Document;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, config) =>
            config.ReadFrom.Configuration(context.Configuration))
        .ConfigureServices((context, services) =>
        {
            services.AddInfrastructure(context.Configuration);
            services.AddDocumentModule();
            services.AddHostedService<Worker.DocumentWorker>();
        })
        .Build();

    // Ensure database is created
    using (var scope = host.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Worker terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
