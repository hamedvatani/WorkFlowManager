using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkFlowManager.Core.Repository;
using WorkFlowManager.Shared;
using WorkFlowManager.Shared.Models;

namespace WorkFlowManager.Core;

public class ManagerService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ManagerService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public MethodResult<int> StartWorkFlow(string json, string starterUser, string starterRole, int workFlowId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetService<IRepository>();
        if (repository == null)
            return MethodResult<int>.Error("Repository DI error!");
        var workFlows = repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<int>.Error("WorkFlow not found!");
        var workFlow = workFlows[0];
        if (!workFlow.IsValid())
            return MethodResult<int>.Error(workFlow.GetValidationError());

        var entity = repository.AddEntity(json, starterUser, starterRole);

        var startStep = workFlow.Steps.FirstOrDefault(s => s.StepType == StepTypeEnum.Start);
        if (startStep == null)
            return MethodResult<int>.Error("Workflow validation error!");

        Task.Run(() => RunStep(startStep, entity));

        return MethodResult<int>.Ok(entity.Id);
    }

    private void RunStep(Step step, Entity entity)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetService<IRepository>();
        if (repository == null)
            return;
        repository.AddEntityLog(entity, step, EntityStatusEnum.StepStart, 
        step.GetDescription(entity.StarterUser, entity.StarterRole));

        Step? nextStep;
        switch (step.ProcessType)
        {
            case ProcessTypeEnum.AddOnWorker:
                var worker = GetStepWorker(step);
                if (worker == null)
                {
                    repository.AddEntityLog(entity, step, EntityStatusEnum.StepFailed,
                        step.GetDescription(entity.StarterUser, entity.StarterRole) + ", Worker not found!");
                    return;
                }

                repository.AddEntityLog(entity, step, EntityStatusEnum.WaitForProcess,
                    step.GetDescription(entity.StarterUser, entity.StarterRole));
                string result;
                try
                {
                    result = worker.RunWorker(entity);
                }
                catch (Exception e)
                {
                    repository.AddEntityLog(entity, step, EntityStatusEnum.StepFailed,
                        step.GetDescription(entity.StarterUser, entity.StarterRole) + ", Worker Exception : " +
                        e.Message);
                    return;
                }

                repository.AddEntityLog(entity, step, EntityStatusEnum.StepSucceed,
                    step.GetDescription(entity.StarterUser, entity.StarterRole) + $", Result : {result}");

                nextStep = GetNextStep(step, result);
                if (nextStep == null)
                {
                    repository.AddEntityLog(entity, step, EntityStatusEnum.StepFailed,
                        step.GetDescription(entity.StarterUser, entity.StarterRole) + ", Next step not found!");
                    return;
                }

                RunStep(nextStep, entity);
                break;
            case ProcessTypeEnum.Service:
                repository.AddEntityLog(entity, step, EntityStatusEnum.WaitForProcess,
                    step.GetDescription(entity.StarterUser, entity.StarterRole));
                repository.AddServiceCartable(entity, step, step.ServiceName, GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.StarterUserOrRole:
                repository.AddEntityLog(entity, step, EntityStatusEnum.WaitForProcess,
                    step.GetDescription(entity.StarterUser, entity.StarterRole));
                repository.AddUserRoleCartable(entity, step, entity.StarterUser, entity.StarterRole,
                    GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.CustomUserOrRole:
                repository.AddEntityLog(entity, step, EntityStatusEnum.WaitForProcess,
                    step.GetDescription(entity.StarterUser, entity.StarterRole));
                repository.AddUserRoleCartable(entity, step, step.CustomUser, step.CustomRole,
                    GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.None:
                nextStep = GetNextStep(step, "");
                if (nextStep == null)
                {
                    repository.AddEntityLog(entity, step, EntityStatusEnum.StepFailed,
                        step.GetDescription(entity.StarterUser, entity.StarterRole) + ", Next step not found!");
                    return;
                }

                repository.AddEntityLog(entity, step, EntityStatusEnum.StepSucceed,
                    step.GetDescription(entity.StarterUser, entity.StarterRole));
                RunStep(nextStep, entity);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static Step? GetNextStep(Step step, string condition)
    {
        var flow = step.Heads.FirstOrDefault(f => f.Condition == condition);
        return flow?.DestinationStep;
    }

    private IWorker? GetWorker(string addOnWorkerDllFileName, string addOnWorkerClassName)
    {
        var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", addOnWorkerDllFileName);
        if (!File.Exists(filename))
            return null;
        var assembly = Assembly.LoadFile(filename);
        var type = assembly.GetType(addOnWorkerClassName);
        if (type == null)
            return null;
        var instance = Activator.CreateInstance(type);
        return (IWorker?) instance;
    }

    private IWorker? GetStepWorker(Step step)
    {
        return GetWorker(step.AddOnWorkerDllFileName, step.AddOnWorkerClassName);
    }

    private string GetStepServiceName(Step step)
    {
        throw new NotImplementedException();
    }

    private string GetStepPossibleActions(Step step)
    {
        var actions = step.Heads.Select(x => x.Condition).Distinct().ToList();
        return actions.Count == 0 ? "" : string.Join(";", actions);
    }
}