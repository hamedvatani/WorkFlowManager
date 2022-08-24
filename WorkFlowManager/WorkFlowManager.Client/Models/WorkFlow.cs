using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Client.Models;

public class WorkFlow : BaseModel
{
    [Required]
    public string Name { get; set; } = "";

    public virtual ICollection<Step> Steps { get; set; } = null!;

    public override string ToString()
    {
        return Name;
    }
}