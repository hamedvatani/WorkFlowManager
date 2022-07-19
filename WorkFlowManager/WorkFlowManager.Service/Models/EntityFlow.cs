using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Service.Models;

public class EntityFlow : BaseModel
{
    [Required]
    [ForeignKey(nameof(Entity))]
    public int EntityId { get; set; }

    public DateTime TimeStamp { get; set; }
    public int SequenceNumber { get; set; }

    public virtual Entity Entity { get; set; } = null!;
}