using Microsoft.Extensions.DependencyInjection;

namespace WorkFlowManager.Core;

public static class Extensions
{
    public static IServiceCollection AddWorkFlowManager(this IServiceCollection services,
        Action<ManagerConfiguration>? configBuilder = null)
    {
        var config = new ManagerConfiguration();
        configBuilder?.Invoke(config);
        services.AddSingleton<Manager>(x => new Manager(config));
        return services;
    }
}