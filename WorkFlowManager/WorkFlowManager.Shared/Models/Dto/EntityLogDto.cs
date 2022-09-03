namespace WorkFlowManager.Shared.Models.Dto;

public sealed class EntityLogDto
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public int EntityId { get; set; }
    public int StepId { get; set; }
    public EntityLogStatusEnum LogType { get; set; }
    public string Description { get; set; } = "";

    public EntityLogDto()
    {
    }

    public EntityLogDto(EntityLog entityLog)
    {
        Id = entityLog.Id;
        TimeStamp = entityLog.TimeStamp;
        EntityId = entityLog.EntityId;
        StepId = entityLog.StepId;
        LogType = entityLog.LogType;
        Description = entityLog.Description;
    }

    public EntityLog ToEntityLog()
    {
        return new EntityLog
        {
            Id = Id,
            TimeStamp = TimeStamp,
            EntityId = EntityId,
            StepId = StepId,
            LogType = LogType,
            Description = Description
        };
    }
}