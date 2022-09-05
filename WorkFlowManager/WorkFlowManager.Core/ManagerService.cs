using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkFlowManager.Core.Repository;
using WorkFlowManager.Shared;
using WorkFlowManager.Shared.Models;

namespace WorkFlowManager.Core;

public class ManagerService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ConcurrentBag<ManagerServiceJob> _jobs = new();

    public ManagerService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Parallel.ForEach(_jobs, item =>
            {
                if (item.JobType == "StartWorkFlow")
                    DoStartWorkFlowWorks(item.EntityId, item.WorkFlowId);
                else if (item.JobType == "SetCartableItemResult")
                    DoSetCartableItemResultWorks(item.CartableItemId, item.Result);
            });
            _jobs.Clear();
            await Task.Delay(1000, stoppingToken);
        }
    }

    public void StartWorkFlow(int entityId, int workFlowId)
    {
        _jobs.Add(new ManagerServiceJob
        {
            JobType = "StartWorkFlow",
            EntityId = entityId,
            WorkFlowId = workFlowId
        });
    }

    public void SetCartableItemResult(int cartableItemId, string result)
    {
        _jobs.Add(new ManagerServiceJob
        {
            JobType = "SetCartableItemResult",
            CartableItemId = cartableItemId,
            Result = result
        });
    }

    private void DoStartWorkFlowWorks(int entityId, int workFlowId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetService<IRepository>();
        if (repository == null)
            return;
        var entity = repository.GetEntityById(entityId);
        if (entity == null)
            return;
        if (entity.Status != EntityStatusEnum.Idle)
            return;
        var workFlows = repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return;
        var workFlow = workFlows[0];
        if (!workFlow.IsValid())
            return;
        var startStep = workFlow.Steps.FirstOrDefault(s => s.StepType == StepTypeEnum.Start);
        if (startStep == null)
            return;
        repository.ChangeEntityStatus(entity, EntityStatusEnum.Running);
        RunStep(startStep, entity, repository);
    }

    private void RunStep(Step step, Entity entity, IRepository repository)
    {
        repository.AddEntityLog(entity, step, EntityLogStatusEnum.StepStart, step.GetDescription(entity));

        Step? nextStep;
        switch (step.ProcessType)
        {
            case ProcessTypeEnum.AddOnWorker:
                var worker = GetStepWorker(step);
                if (worker == null)
                {
                    repository.AddEntityLog(entity, step, EntityLogStatusEnum.StepFailed,
                        step.GetDescription(entity) + ", Worker not found!");
                    return;
                }

                repository.AddEntityLog(entity, step, EntityLogStatusEnum.WaitForProcess, step.GetDescription(entity));
                string result;
                try
                {
                    result = worker.RunWorker(entity);
                }
                catch (Exception e)
                {
                    repository.AddEntityLog(entity, step, EntityLogStatusEnum.StepFailed, step.GetDescription(entity) +
                        ", Worker Exception : " +
                        e.Message);
                    return;
                }

                repository.AddEntityLog(entity, step, EntityLogStatusEnum.StepSucceed,
                    step.GetDescription(entity) + $", Result : {result}");

                nextStep = repository.GetNextStep(step.Id, result);
                if (nextStep == null)
                {
                    repository.AddEntityLog(entity, step, EntityLogStatusEnum.StepFailed,
                        step.GetDescription(entity) + ", Next step not found!");
                    return;
                }

                RunStep(nextStep, entity, repository);
                break;
            case ProcessTypeEnum.Service:
                repository.AddEntityLog(entity, step, EntityLogStatusEnum.WaitForProcess, step.GetDescription(entity));
                repository.AddCartableItem(entity, step, "", "", step.ServiceName, GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.StarterUserOrRole:
                repository.AddEntityLog(entity, step, EntityLogStatusEnum.WaitForProcess, step.GetDescription(entity));
                repository.AddCartableItem(entity, step, entity.StarterUser, entity.StarterRole, "",
                    GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.CustomUserOrRole:
                repository.AddEntityLog(entity, step, EntityLogStatusEnum.WaitForProcess, step.GetDescription(entity));
                repository.AddCartableItem(entity, step, step.CustomUser, step.CustomRole, "",
                    GetStepPossibleActions(step));
                break;
            case ProcessTypeEnum.None:
                repository.AddEntityLog(entity, step, EntityLogStatusEnum.StepSucceed, step.GetDescription(entity));
                if (step.StepType == StepTypeEnum.End)
                {
                    repository.ChangeEntityStatus(entity, EntityStatusEnum.Done);
                    break;
                }

                nextStep = repository.GetNextStep(step.Id, "");
                if (nextStep == null)
                {
                    repository.AddEntityLog(entity, step, EntityLogStatusEnum.StepFailed,
                        step.GetDescription(entity) + ", Next step not found!");
                    return;
                }

                RunStep(nextStep, entity, repository);
                break;
        }
    }

    private IWorker? GetStepWorker(Step step)
    {
        return Extensions.GetWorker(step.AddOnWorkerDllFileName, step.AddOnWorkerClassName);
    }

    private string GetStepPossibleActions(Step step)
    {
        var actions = step.Heads.Select(x => x.Condition).Distinct().ToList();
        return actions.Count == 0 ? "" : string.Join(";", actions);
    }

    private void DoSetCartableItemResultWorks(int cartableItemId, string result)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetService<IRepository>();
        if (repository == null)
            return;
        var cartableItem = repository.GetCartableItemById(cartableItemId);
        if (cartableItem == null)
            return;
        var entity = repository.GetEntityById(cartableItem.EntityId);
        if (entity == null)
            return;
        var step = cartableItem.Step;
        if (!cartableItem.PossibleActions.Contains(result))
        {
            repository.AddEntityLog(entity, step, EntityLogStatusEnum.StepFailed,
                step.GetDescription(entity) + ", Invalid result");
            return;
        }

        repository.AddEntityLog(entity, step, EntityLogStatusEnum.StepSucceed,
            step.GetDescription(entity) + $", Result : {result}");

        var nextStep = repository.GetNextStep(step.Id, result);
        if (nextStep == null)
        {
            repository.AddEntityLog(entity, step, EntityLogStatusEnum.StepFailed,
                step.GetDescription(entity) + ", Next step not found!");
            return;
        }

        RunStep(nextStep, entity, repository);

        repository.DeleteCartableItem(cartableItemId);
    }
}