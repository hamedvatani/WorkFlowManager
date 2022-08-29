using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Shared.Models;

public class EntityLog : BaseModel
{
    [Required]
    [ForeignKey(nameof(Entity))]
    public int EntityId { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public EntityLogTypeEnum LogType { get; set; }
    public string LogTypeDescription { get; set; } = "";

    [ForeignKey(nameof(Step))]
    public int? StepId { get; set; } = null;

    public string Subject { get; set; } = "";
    public string Description { get; set; } = "";

    public virtual Entity Entity { get; set; } = null!;
    public virtual Step? Step { get; set; } = null;
}