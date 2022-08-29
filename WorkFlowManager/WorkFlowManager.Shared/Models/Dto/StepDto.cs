namespace WorkFlowManager.Shared.Models.Dto;

public sealed class StepDto
{
    public int Id { get; set; }
    public StepTypeEnum StepType { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public ProcessTypeEnum ProcessType { get; set; }
    public string CustomUser { get; set; } = "";
    public string CustomRole { get; set; } = "";
    public int WorkFlowId { get; set; }
    public List<FlowDto> Heads { get; set; } = new();
    public List<FlowDto> Tails { get; set; } = new();

    public StepDto()
    {
    }

    public StepDto(Step step)
    {
        Id = step.Id;
        StepType = step.StepType;
        Name = step.Name;
        Description = step.Description;
        ProcessType = step.ProcessType;
        CustomUser = step.CustomUser;
        CustomRole = step.CustomRole;
        WorkFlowId = step.WorkFlowId;
        Heads = step.Heads.Select(x => new FlowDto(x)).ToList();
        Tails = step.Tails.Select(x => new FlowDto(x)).ToList();
    }

    public Step ToStep()
    {
        return new Step
        {
            Id = Id,
            StepType = StepType,
            Name = Name,
            Description = Description,
            ProcessType = ProcessType,
            CustomUser = CustomUser,
            CustomRole = CustomRole,
            WorkFlowId = WorkFlowId,
            Heads = Heads.Select(x => x.ToFlow()).ToList(),
            Tails = Tails.Select(x => x.ToFlow()).ToList()
        };
    }
}