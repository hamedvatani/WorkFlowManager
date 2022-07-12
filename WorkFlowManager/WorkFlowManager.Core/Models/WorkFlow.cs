namespace WorkFlowManager.Core.Models;

public class WorkFlow : BaseModel
{
    public string Name { get; set; } = "";

    public virtual ICollection<Step> Steps { get; set; } = null!;
}