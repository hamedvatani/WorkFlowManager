using JobHandler.Executor;
using JobHandler.RabbitMq.Executor;
using JobHandler.Sender;
using Microsoft.Extensions.Hosting;
using RPC.RabbitMq;
using WorkFlowManager.Client;
using WorkFlowManager.Client.Models;
using WorkFlowManager.Core.Repository;

namespace WorkFlowManager.Core;

public class Manager : IHostedService
{
    private readonly ManagerConfiguration _configuration;
    private readonly IRepository _repository;
    private readonly RpcServer _rpcServer;
    private readonly ISender _sender;
    private readonly IExecutor<Job> _executor;

    public Manager(ManagerConfiguration configuration, IRepository repository, RpcServer rpcServer, ISender sender, IExecutor<Job> executor)
    {
        _repository = repository;
        _rpcServer = rpcServer;
        _sender = sender;
        _executor = executor;
        _configuration = configuration;
        _repository = repository;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _rpcServer.Start(RpcFunction);
        _executor.StartExecution(ExecuteJob, FailJob);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _rpcServer.Stop();
        _executor.StopExecution();
        return Task.CompletedTask;
    }

    private RpcResultDto RpcFunction(RpcFunctionDto arg)
    {
        switch (arg.FunctionName)
        {
            case "StartWorkFlow":
                try
                {
                    string json = arg.Parameters["Json"];
                    string starterUser = arg.Parameters["StarterUser"];
                    string starterRole = arg.Parameters["StarterRole"];
                    int workFlowId = int.Parse(arg.Parameters["WorkFlowId"]);
                    var result = StartWorkFlow(json, starterUser, starterRole, workFlowId);
                    var rpcResult = new RpcResultDto(result.IsSuccess, result.IsTimeout, result.Message);
                    if (result.IsSuccess)
                        rpcResult.Parameters.Add("EntityId", result.GetResult().ToString());
                    return rpcResult;
                }
                catch (Exception e)
                {
                    return new RpcResultDto(false, false, e.Message);
                }
            default:
                return new RpcResultDto(false, false, "Unknown function name!");
        }
    }

    private FuncResult ExecuteJob(Job job, CancellationToken cancellationToken)
    {
        if (RunStep(job.StepId, job.EntityId))
            return FuncResult.Success();
        else
            return FuncResult.Fail("");
    }

    private void FailJob(Job job, List<FuncResult> funcResults)
    {
    }

    public MethodResult<List<WorkFlow>> GetWorkFlows(int id = 0, string name = "")
    {
        return MethodResult<List<WorkFlow>>.Ok(_repository.GetWorkFlows(id, name));
    }

    public MethodResult<WorkFlow> AddWorkFlow(string name)
    {
        return MethodResult<WorkFlow>.Ok(_repository.AddWorkFlow(name));
    }

    public MethodResult<Step> AddStep(int workFlowId, string name, StepTypeEnum stepType, ProcessTypeEnum processType,
        string description, string customUser, string customRole)
    {
        var workFlows = _repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<Step>.Error("Workflow not found!");
        return MethodResult<Step>.Ok(_repository.AddStep(workFlows[0], name, stepType, processType, description,
            customUser, customRole));
    }

    public MethodResult<Flow> AddFlow(int sourceStepId, int destinationStepId, string condition)
    {
        var sourceStep = _repository.GetStepById(sourceStepId);
        if (sourceStep == null)
            return MethodResult<Flow>.Error("Source Step not found!");
        var destinationStep = _repository.GetStepById(destinationStepId);
        if (destinationStep == null)
            return MethodResult<Flow>.Error("Destination Step not found!");
        return MethodResult<Flow>.Ok(_repository.AddFlow(sourceStep, destinationStep, condition));
    }

    private MethodResult<int> StartWorkFlow(string json, string starterUser, string starterRole, int workFlowId)
    {
        var workFlows = _repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<int>.Error("WorkFlow not found!");
        var workFlow = workFlows[0];
        if (!workFlow.IsValid())
            return MethodResult<int>.Error(workFlow.GetValidationError());

        var entity = _repository.AddEntity(json, starterUser, starterRole);

        var startStep = workFlow.Steps.FirstOrDefault(s => s.StepType == StepTypeEnum.Start);
        if (startStep == null)
            return MethodResult<int>.Error("Workflow validation error!");
        _sender.SendAsync(new Job
        {
            EntityId = entity.Id,
            StepId = startStep.Id
        });

        return MethodResult<int>.Ok(entity.Id);
    }

    private bool RunStep(int stepId, int entityId)
    {
        var entity = _repository.GetEntityById(entityId);
        if (entity == null)
            return false;
        var step = _repository.GetStepById(stepId);
        if (step == null)
        {
            _repository.AddEntityLog(entity, DateTime.Now, EntityLogSeverityEnum.Error, "Unknown Step",
                "StepId not found");
            return false;
        }

        switch (step.ProcessType)
        {
            case ProcessTypeEnum.AddOnWorker:
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
                break;
        }

        return true;
    }
}