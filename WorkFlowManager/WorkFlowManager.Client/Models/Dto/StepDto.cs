namespace WorkFlowManager.Client.Models.Dto;

public sealed class StepDto
{
    public StepTypeEnum StepType { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ProcessTypeEnum ProcessType { get; set; }
    public string CustomUser { get; set; }
    public string CustomRole { get; set; }
    public int WorkFlowId { get; set; }
    public int? AddOnWorkerId { get; set; }
    public AddOnWorker? AddOnWorker { get; set; }
    public List<FlowDto> Heads { get; set; }
    public List<FlowDto> Tails { get; set; }

    public StepDto(Step step)
    {
        StepType = step.StepType;
        Name = step.Name;
        Description = step.Description;
        ProcessType = step.ProcessType;
        CustomUser = step.CustomUser;
        CustomRole = step.CustomRole;
        WorkFlowId = step.WorkFlowId;
        AddOnWorkerId = step.AddOnWorkerId;
        AddOnWorker = step.AddOnWorker;
        Heads = step.Heads.Select(x => new FlowDto(x)).ToList();
        Tails = step.Tails.Select(x => new FlowDto(x)).ToList();
    }
}