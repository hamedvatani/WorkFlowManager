using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RPC.RabbitMq;
using WorkFlowManager.Client.Models.Dto;

namespace WorkFlowManager.Client;

public class Client : IHostedService
{
    private readonly ClientConfiguration _configuration;
    private readonly ApiClient _apiClient;
    private readonly RpcClient _rpcClient;

    public Client(ClientConfiguration configuration, ApiClient apiClient)
    {
        _configuration = configuration;
        _apiClient = apiClient;
        _rpcClient = new RpcClient(new RpcConfiguration
        {
            RabbitMqHostName = configuration.RabbitMqHostName,
            RabbitMqUserName = configuration.RabbitMqUserName,
            RabbitMqPassword = configuration.RabbitMqPassword,
            InputQueueName = configuration.QueueName + ".Input",
            OutputQueueName = configuration.QueueName + ".Output",
            Timeout = 10000
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _rpcClient.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _rpcClient.Stop();
        return Task.CompletedTask;
    }

    public MethodResult<List<WorkFlowDto>> GetWorkFlows(GetWorkFlowsDto model)
    {
        return _apiClient.CallPostApi<GetWorkFlowsDto, List<WorkFlowDto>>("GetWorkFlows", model);
    }

    public MethodResult<WorkFlowDto> AddWorkFlow(AddWorkFlowDto model)
    {
        return _apiClient.CallPostApi<AddWorkFlowDto, WorkFlowDto>("AddWorkFlow", model);
    }

    public MethodResult<StepDto> AddStep(AddStepDto model)
    {
        return _apiClient.CallPostApi<AddStepDto, StepDto>("AddStep", model);
    }

    public MethodResult<FlowDto> AddFlow(AddFlowDto model)
    {
        return _apiClient.CallPostApi<AddFlowDto, FlowDto>("AddFlow", model);
    }

    public MethodResult<int> StartWorkFlow(object entity, string starterUser, string starterRole, int workFlowId)
    {
        var result = _rpcClient.Call(new RpcFunctionDto("StartWorkFlow",
            new KeyValuePair<string, string>("Json", JsonConvert.SerializeObject(entity)),
            new KeyValuePair<string, string>("StarterUser", starterUser),
            new KeyValuePair<string, string>("StarterRole", starterRole),
            new KeyValuePair<string, string>("WorkFlowId", workFlowId.ToString())));
        if (result.IsTimeout)
            return MethodResult<int>.Timeout();
        if (!result.IsSuccess)
            return MethodResult<int>.Error(result.Message);
        return MethodResult<int>.Ok(int.Parse(result.Parameters["EntityId"]));
    }
}