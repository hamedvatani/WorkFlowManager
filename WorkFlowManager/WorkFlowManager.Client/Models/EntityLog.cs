using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Client.Models;

public class EntityLog : BaseModel
{
    [Required]
    [ForeignKey(nameof(Entity))]
    public int EntityId { get; set; }

    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
    public string ReturnValue { get; set; } = "";
    public DateTime TimeStamp { get; set; }

    public virtual Entity Entity { get; set; } = null!;
}