namespace WorkFlowManager.Client.Models.Dto;

public sealed class WorkFlowDto
{
    public string Name { get; set; }
    public string EntityName { get; set; }
    public List<StepDto> Steps { get; set; }

    public WorkFlowDto(WorkFlow workFlow)
    {
        Name = workFlow.Name;
        EntityName = workFlow.EntityName;
        Steps = workFlow.Steps.Select(x => new StepDto(x)).ToList();
    }
}