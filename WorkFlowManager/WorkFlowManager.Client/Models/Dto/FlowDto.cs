namespace WorkFlowManager.Client.Models.Dto;

public sealed class FlowDto
{
    public int Id { get; set; }
    public int SourceStepId { get; set; }
    public int DestinationStepId { get; set; }
    public string Condition { get; set; } = "";

    public FlowDto()
    {
    }

    public FlowDto(Flow flow)
    {
        Id = flow.Id;
        SourceStepId = flow.SourceStepId;
        DestinationStepId = flow.DestinationStepId;
        Condition = flow.Condition;
    }

    public Flow ToFlow()
    {
        return new Flow
        {
            Id = Id,
            SourceStepId = SourceStepId,
            DestinationStepId = DestinationStepId,
            Condition = Condition
        };
    }
}