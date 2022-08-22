using Microsoft.Extensions.Hosting;
using WorkFlowManager.Client.Models.Dto;

namespace WorkFlowManager.Client;

public class Client : IHostedService
{
    private readonly ClientConfiguration _configuration;
    private readonly ApiClient _apiClient;

    public Client(ClientConfiguration configuration, ApiClient apiClient)
    {
        _configuration = configuration;
        _apiClient = apiClient;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
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
}