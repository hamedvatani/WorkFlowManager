using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Client.Models;

public class ServiceCartable : BaseModel
{
    [ForeignKey(nameof(Entity))]
    public int EntityId { get; set; }

    [ForeignKey(nameof(Step))]
    public int StepId { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public string ServiceName { get; set; } = "";
    public string PossibleActions { get; set; } = "";

    public virtual Entity Entity { get; set; } = null!;
    public virtual Step Step { get; set; } = null!;
}