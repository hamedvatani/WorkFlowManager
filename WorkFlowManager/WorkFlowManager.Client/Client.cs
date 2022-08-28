using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using WorkFlowManager.Client.Models.Dto;

namespace WorkFlowManager.Client;

public class Client
{
    private readonly ClientConfiguration _configuration;
    private readonly ApiClient _apiClient;

    public Client(ClientConfiguration configuration, ApiClient apiClient)
    {
        _configuration = configuration;
        _apiClient = apiClient;
    }

    public MethodResult<List<WorkFlowDto>> GetWorkFlows(GetWorkFlowsDto model)
    {
        return _apiClient.CallPostApi<GetWorkFlowsDto, List<WorkFlowDto>>("GetWorkFlows", model);
    }

    public MethodResult<WorkFlowDto> AddWorkFlow(AddWorkFlowDto model)
    {
        return _apiClient.CallPostApi<AddWorkFlowDto, WorkFlowDto>("AddWorkFlow", model);
    }

    public MethodResult<StepDto> AddStartStep(AddStartStepDto model)
    {
        return _apiClient.CallPostApi<AddStartStepDto, StepDto>("AddStartStep", model);
    }

    public MethodResult<StepDto> AddEndStep(AddEndStepDto model)
    {
        return _apiClient.CallPostApi<AddEndStepDto, StepDto>("AddEndStep", model);
    }

    public MethodResult<StepDto> AddAddOnWorkerStep(AddAddOnWorkerStepDto model)
    {
        return _apiClient.CallPostApi<AddAddOnWorkerStepDto, StepDto>("AddAddOnWorkerStep", model);
    }

    public MethodResult<StepDto> AddCartableStep(AddCartableStepDto model)
    {
        return _apiClient.CallPostApi<AddCartableStepDto, StepDto>("AddCartableStep", model);
    }

    public MethodResult<FlowDto> AddFlow(AddFlowDto model)
    {
        return _apiClient.CallPostApi<AddFlowDto, FlowDto>("AddFlow", model);
    }
}