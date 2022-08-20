using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using WorkFlowManager.Client.Models;
using WorkFlowManager.Client.Models.Dto;

namespace WorkFlowManager.Client;

public class Client : IHostedService
{
    private readonly ClientConfiguration _configuration;
    private readonly ApiClient _apiClient;
    private readonly RpcClient _rpcClient;

    public Client(ClientConfiguration configuration, ApiClient apiClient, RpcClient rpcClient)
    {
        _configuration = configuration;
        _apiClient = apiClient;
        _rpcClient = rpcClient;
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

    public MethodResult<List<WorkFlow>> GetWorkFlows(GetWorkFlowsDto model)
    {
        return _apiClient.CallPostApi<GetWorkFlowsDto, List<WorkFlow>>("GetWorkFlows", model);
    }

    public MethodResult<WorkFlow> AddWorkFlow(AddWorkFlowDto model)
    {
        return _apiClient.CallPostApi<AddWorkFlowDto, WorkFlow>("AddWorkFlow", model);
    }

    public MethodResult<Step> AddStep(AddStepDto model)
    {
        return _apiClient.CallPostApi<AddStepDto, Step>("AddStep", model);
    }
}