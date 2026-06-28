using System.Reflection;
using Chat.Application;
using Chat.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Chat;

public static class DependencyInjection
{
    public static IServiceCollection AddChatModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<IChatSessionRepository, ChatSessionRepository>();
        return services;
    }
}
