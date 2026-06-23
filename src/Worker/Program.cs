using Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<DocumentWorker>();

var host = builder.Build();
host.Run();
