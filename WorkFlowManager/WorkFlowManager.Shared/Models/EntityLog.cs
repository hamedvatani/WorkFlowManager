using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Shared.Models;

public class EntityLog : BaseModel
{
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    [Required]
    [ForeignKey(nameof(Entity))]
    public int EntityId { get; set; }

    [ForeignKey(nameof(Step))]
    public int StepId { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public EntityLogStatusEnum LogType { get; set; }

    public string Description { get; set; } = "";
    public virtual Entity Entity { get; set; } = null!;
    public virtual Step Step { get; set; } = null!;
}