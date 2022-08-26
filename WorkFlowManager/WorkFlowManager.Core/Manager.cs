using Microsoft.Extensions.Hosting;
using WorkFlowManager.Client;
using WorkFlowManager.Client.Models;
using WorkFlowManager.Core.Repository;

namespace WorkFlowManager.Core;

public class Manager : IHostedService
{
    private readonly ManagerConfiguration _configuration;
    private readonly IRepository _repository;

    public Manager(ManagerConfiguration configuration, IRepository repository)
    {
        _repository = repository;
        _configuration = configuration;
        _repository = repository;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task<MethodResult<List<WorkFlow>>> GetWorkFlowsAsync(int id = 0, string name = "")
    {
        return MethodResult<List<WorkFlow>>.Ok(await _repository.GetWorkFlowsAsync(id, name));
    }

    public async Task<MethodResult<WorkFlow>> AddWorkFlowAsync(string name)
    {
        return MethodResult<WorkFlow>.Ok(await _repository.AddWorkFlowAsync(name));
    }

    public async Task<MethodResult<Step>> AddStepAsync(int workFlowId, string name, StepTypeEnum stepType,
        ProcessTypeEnum processType, string description, string customUser, string customRole)
    {
        var workFlows = await _repository.GetWorkFlowsAsync(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(await _repository.AddStepAsync(workFlows[0], name, stepType, processType,
            description, customUser, customRole));
    }

    public async Task<MethodResult<Flow>> AddFlowAsync(int sourceStepId, int destinationStepId, string condition)
    {
        var sourceStep = await _repository.GetStepByIdAsync(sourceStepId);
        if (sourceStep == null)
            return MethodResult<Flow>.Error("Source Step not found!");
        var destinationStep = await _repository.GetStepByIdAsync(destinationStepId);
        if (destinationStep == null)
            return MethodResult<Flow>.Error("Destination Step not found!");
        return MethodResult<Flow>.Ok(await _repository.AddFlowAsync(sourceStep, destinationStep, condition));
    }

    public async Task<MethodResult<int>> StartWorkFlow(string json, string starterUser, string starterRole, int workFlowId)
    {
        var workFlows = await _repository.GetWorkFlowsAsync(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<int>.Error("WorkFlow not found!");
        var workFlow = workFlows[0];
        if (!workFlow.IsValid())
            return MethodResult<int>.Error(workFlow.GetValidationError());
    
        var entity = await _repository.AddEntityAsync(json, starterUser, starterRole, EntityStatusEnum.Idle);
    
        var startStep = workFlow.Steps.FirstOrDefault(s => s.StepType == StepTypeEnum.Start);
        if (startStep == null)
            return MethodResult<int>.Error("Workflow validation error!");

        RunStepAsync(startStep, entity).Start();
    
        return MethodResult<int>.Ok(entity.Id);
    }

    private async Task RunStepAsync(Step step, Entity entity)
    {
        await _repository.ChangeStatusAsync(entity, step, EntityStatusEnum.RunningStep);
        await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.StartStep, "Start Running Step", "");
        Step? nextStep;
        switch (step.ProcessType)
        {
            case ProcessTypeEnum.AddOnWorker:
                var worker = GetStepWorker(step);
                if (worker == null)
                {
                    await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.AddOnFailed, "Worker not found!",
                        "");
                    return;
                }

                var result = await worker.RunWorkerAsync(entity);
                nextStep = GetNextStep(step, result);
                if (nextStep == null)
                {
                    await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.AddOnFailed,
                        "Next step not found!", "");
                    return;
                }

                await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.AddOnSucceed, "", "");
                await RunStepAsync(nextStep, entity);
                break;
            case ProcessTypeEnum.Service:
                break;
            case ProcessTypeEnum.StarterUser:
                break;
            case ProcessTypeEnum.StarterRole:
                break;
            case ProcessTypeEnum.CustomUser:
                break;
            case ProcessTypeEnum.CustomRole:
                break;
            case ProcessTypeEnum.None:
                nextStep = GetNextStep(step, "");
                if (nextStep == null)
                {
                    await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.GeneralFailed,
                        "Next step not found!", "");
                    return;
                }

                await _repository.AddEntityLogAsync(entity, step, EntityLogTypeEnum.GeneralSucceed, "", "");
                await RunStepAsync(nextStep, entity);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private Step? GetNextStep(Step step, string condition)
    {
        var flow = step.Heads.FirstOrDefault(f => f.Condition == condition);
        if (flow == null)
            return null;
        return flow.DestinationStep;
    }

    private IWorker? GetStepWorker(Step step)
    {
        return null;
    }

    private string GetStepQueue(Step step)
    {
        return null;
    }
}