using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Workspace.Application;
using Workspace.Infrastructure;

namespace Workspace;

public static class DependencyInjection
{
    public static IServiceCollection AddWorkspaceModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        return services;
    }
}
