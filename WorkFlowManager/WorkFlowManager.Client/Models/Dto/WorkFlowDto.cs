namespace WorkFlowManager.Client.Models.Dto;

public sealed class WorkFlowDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string EntityName { get; set; } = "";
    public List<StepDto> Steps { get; set; } = new();

    public WorkFlowDto()
    {
    }

    public WorkFlowDto(WorkFlow workFlow)
    {
        Id = workFlow.Id;
        Name = workFlow.Name;
        EntityName = workFlow.EntityName;
        Steps = workFlow.Steps.Select(x => new StepDto(x)).ToList();
    }

    public WorkFlow ToWorkFlow()
    {
        WorkFlow workFlow = new WorkFlow
        {
            Id = Id,
            Name = Name,
            EntityName = EntityName,
            Steps = Steps.Select(x => x.ToStep()).ToList()
        };

        foreach (var step in workFlow.Steps)
        {
            step.WorkFlow = workFlow;
            foreach (var flow in step.Heads)
            {
                flow.SourceStep = workFlow.Steps.FirstOrDefault(x => x.Id == flow.SourceStepId) ?? new();
                flow.DestinationStep = workFlow.Steps.FirstOrDefault(x => x.Id == flow.DestinationStepId) ?? new();
            }

            foreach (var flow in step.Tails)
            {
                flow.SourceStep = workFlow.Steps.FirstOrDefault(x => x.Id == flow.SourceStepId) ?? new();
                flow.DestinationStep = workFlow.Steps.FirstOrDefault(x => x.Id == flow.DestinationStepId) ?? new();
            }
        }

        return workFlow;
    }
}