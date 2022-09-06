using Microsoft.Extensions.DependencyInjection;

namespace WorkFlowManager.Client;

public static class Extensions
{
    public static IServiceCollection AddWorkFlowClient(this IServiceCollection services,
        Action<ClientConfiguration>? configBuilder = null)
    {
        var config = new ClientConfiguration();
        configBuilder?.Invoke(config);

        services.AddSingleton(config);
        services.AddSingleton<ApiClient>();
        services.AddSingleton<Client>();
        return services;
    }
}