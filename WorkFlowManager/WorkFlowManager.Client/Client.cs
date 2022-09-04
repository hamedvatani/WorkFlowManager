using WorkFlowManager.Shared;
using WorkFlowManager.Shared.Models.Dto;

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

    public MethodResult<StepDto> AddStarterUserRoleCartableStep(AddStarterUserRoleCartableStepDto model)
    {
        return _apiClient.CallPostApi<AddStarterUserRoleCartableStepDto, StepDto>("AddStarterUserRoleCartableStep", model);
    }

    public MethodResult<StepDto> AddCustomUserRoleCartableStep(AddCustomUserRoleCartableStepDto model)
    {
        return _apiClient.CallPostApi<AddCustomUserRoleCartableStepDto, StepDto>("AddCartableStep", model);
    }

    public MethodResult<FlowDto> AddFlow(AddFlowDto model)
    {
        return _apiClient.CallPostApi<AddFlowDto, FlowDto>("AddFlow", model);
    }

    public MethodResult<EntityDto> StartWorkFlow(StartWorkFlowDto model)
    {
        return _apiClient.CallPostApi<StartWorkFlowDto, EntityDto>("StartWorkFlow", model);
    }

    public MethodResult<List<CartableItemDto>> GetUserCartable(GetUserCartableDto model)
    {
        return _apiClient.CallPostApi<GetUserCartableDto, List<CartableItemDto>>("GetUserCartable", model);
    }

    public MethodResult<List<CartableItemDto>> GetRoleCartable(GetRoleCartableDto model)
    {
        return _apiClient.CallPostApi<GetRoleCartableDto, List<CartableItemDto>>("GetRoleCartable", model);
    }

    public MethodResult<List<CartableItemDto>> GetServiceCartable(GetServiceCartableDto model)
    {
        return _apiClient.CallPostApi<GetServiceCartableDto, List<CartableItemDto>>("GetServiceCartable", model);
    }

    public MethodResult SetCartableItemResult(SetCartableItemResultDto model)
    {
        return _apiClient.CallPostApi("SetCartableItemResult", model);
    }
}