﻿using Microsoft.Extensions.DependencyInjection;
using WorkFlowManager.Core.Repository;

namespace WorkFlowManager.Core;

public static class Extensions
{
    public static IServiceCollection AddWorkFlowManager(this IServiceCollection services,
        Action<ManagerConfiguration>? configBuilder = null)
    {
        var config = new ManagerConfiguration();
        configBuilder?.Invoke(config);
        services.AddSingleton(config);
        services.AddSingleton<IRepository, TestRepository>();
        services.AddSingleton<Manager>();
        services.AddHostedService<Manager>();
        return services;
    }
}