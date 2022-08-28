using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Client.Models;

public class UserRoleCartable : BaseModel
{
    [ForeignKey(nameof(Entity))]
    public int EntityId { get; set; }

    [ForeignKey(nameof(Step))]
    public int StepId { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public string User { get; set; } = "";
    public string Role { get; set; } = "";
    public string PossibleActions { get; set; } = "";

    public virtual Entity Entity { get; set; } = null!;
    public virtual Step Step { get; set; } = null!;
}