namespace WorkFlowManager.Client.Models.Dto;

public sealed class FlowDto
{
    public int SourceStepId { get; set; }
    public int DestinationStepId { get; set; }
    public string Condition { get; set; }

    public FlowDto(Flow flow)
    {
        SourceStepId = flow.SourceStepId;
        DestinationStepId = flow.DestinationStepId;
        Condition = flow.Condition;
    }
}