using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkFlowManager.Core.Context;
using WorkFlowManager.Core.Implementations;
using WorkFlowManager.Core.Interfaces;

namespace WorkFlowManager.Core;

public static class Tools
{
    public static IServiceCollection AddWorkFlowService(this IServiceCollection services,
        Action<WorkFlowManagerConfiguration> configurationBuilder)
    {
        var config = new WorkFlowManagerConfiguration();
        configurationBuilder(config);

        if (config.UsingSqlServer)
            services.AddDbContext<WorkFlowManagerContext>(op => op.UseSqlServer(config.SqlServerConnectionString));

        if (config.UsingSqlite)
            services.AddDbContext<WorkFlowManagerContext>(op => op.UseSqlite($"Filename={config.SqliteFilename}"));

        services.AddScoped<IWfManager, WfManager>();

        return services;
    }
}