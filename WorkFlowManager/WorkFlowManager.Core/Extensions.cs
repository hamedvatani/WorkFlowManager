using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkFlowManager.Core.Repository;
using WorkFlowManager.Shared;
using WorkFlowManager.Shared.Data;
using WorkFlowManager.Shared.Models;

namespace WorkFlowManager.Core;

public static class Extensions
{
    public static IServiceCollection AddWorkFlowManager(this IServiceCollection services,
        Action<ManagerConfiguration>? configBuilder = null)
    {
        var config = new ManagerConfiguration();
        configBuilder?.Invoke(config);
    
        services.AddSingleton(config);
        services.AddDbContext<WorkFlowManagerContext>(options => { options.UseSqlServer(config.ConnectionString); });
        services.AddScoped<IRepository, WfmRepository>();
        services.AddScoped<Manager>();
        services.AddSingleton<ManagerService>();
        services.AddHostedService(serviceProvider => serviceProvider.GetService<ManagerService>() ?? null!);
        return services;
    }
    
    public static string GetValidationError(this WorkFlow workFlow)
    {
        if (workFlow.Steps.Count(step => step.StepType == StepTypeEnum.Start) != 1)
            return "Workflow has to have exact one start step";
        var start = workFlow.Steps.FirstOrDefault(step => step.StepType == StepTypeEnum.Start);
        if (start == null)
            return "Unknown";
        if (start.Tails.Count > 0)
            return "There is a flow to start step";
        if (start.ProcessType != ProcessTypeEnum.None)
            return "Start step process type must be None";
    
        if (workFlow.Steps.Count(step => step.StepType == StepTypeEnum.End) != 1)
            return "Workflow has to have exact one end step";
        var end = workFlow.Steps.FirstOrDefault(step => step.StepType == StepTypeEnum.End);
        if (end == null)
            return "Unknown";
        if (end.Heads.Count > 0)
            return "There is a flow from end step";
        if (end.ProcessType != ProcessTypeEnum.None)
            return "End step process type must be None";
    
        foreach (var step in workFlow.Steps)
        {
            if (step.Heads.Count(flow => flow.Condition == "") > 1)
                return $"Step {step.Name} has more than one unconditional flow";
            if (step.StepType != StepTypeEnum.Start && step.Tails.Count == 0)
                return $"There is no flow to step {step.Name}";
            if (step.StepType != StepTypeEnum.End && step.Heads.Count == 0)
                return $"There is no flow from step {step.Name}";
            if (step.StepType == StepTypeEnum.Start || step.StepType == StepTypeEnum.Process)
            {
                if (step.Heads.Count != 1)
                    return $"step {step.Name} has to have exact one unconditional flow out";
                var f = step.Heads.ToList()[0];
                if (f.Condition != "")
                    return $"step {step.Name} has to have exact one unconditional flow out";
            }
    
            if (step.StepType == StepTypeEnum.Condition && step.Heads.Count < 2)
                return $"step {step.Name} has to have more than one condition";
            if (step.Heads.Count > 0 && step.Heads.Count != step.Heads.Select(s => s.Condition).Distinct().Count())
                return $"step {step.Name} has repetitious condition";
        }
    
        return "";
    }
    
    public static bool IsValid(this WorkFlow workFlow)
    {
        return workFlow.GetValidationError() == "";
    }
    
    public static IWorker? GetWorker(string addOnWorkerDllFileName, string addOnWorkerClassName)
    {
        var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", addOnWorkerDllFileName);
        if (!File.Exists(filename))
            return null;
        var assembly = Assembly.LoadFile(filename);
        var type = assembly.GetType(addOnWorkerClassName);
        if (type == null)
            return null;
        var instance = Activator.CreateInstance(type);
        return (IWorker?)instance;
    }
}