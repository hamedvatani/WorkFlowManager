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

    public Manager(ManagerConfiguration configuration, IRepository repository, RpcServer rpcServer)
    {
        _repository = repository;
        _rpcServer = rpcServer;
        _configuration = configuration;
        _repository = repository;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _rpcServer.Start(RpcFunction);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private RpcResultDto RpcFunction(RpcFunctionDto arg)
    {
        switch (arg.FunctionName)
        {
            default:
                return new RpcResultDto(false, false, "Unknown function name!");
        }
    }

    public MethodResult<List<WorkFlow>> GetWorkFlows(int id = 0, string name = "")
    {
        return MethodResult<List<WorkFlow>>.Ok(_repository.GetWorkFlows(id, name));
    }

    public MethodResult<WorkFlow> AddWorkFlow(string name, string entityName)
    {
        return MethodResult<WorkFlow>.Ok(_repository.AddWorkFlow(name, entityName));
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

    private MethodResult<string> StartWorkFlow(IEntity entity, int workFlowId)
    {
        var workFlows = _repository.GetWorkFlows(workFlowId);
        if (workFlows.Count != 1)
            return MethodResult<string>.Error("WorkFlow not found!");
        var workFlow = workFlows[0];
        if (!workFlow.IsValid())
            return MethodResult<string>.Error(workFlow.GetValidationError());




    }
}