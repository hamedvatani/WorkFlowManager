using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Client.Models;

public class EntityLog : BaseModel
{
    [Required]
    [ForeignKey(nameof(Entity))]
    public int EntityId { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public EntityLogSeverityEnum Severity { get; set; }
    public string Subject { get; set; } = "";
    public string Description { get; set; } = "";

    public virtual Entity Entity { get; set; } = null!;
}