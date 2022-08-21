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
}