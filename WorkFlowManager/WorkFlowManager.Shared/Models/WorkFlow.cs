using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models;

public class WorkFlow : BaseModel
{
    [Required]
    public string Name { get; set; } = "";

    public virtual ICollection<Step> Steps { get; set; } = new List<Step>();

    public override string ToString()
    {
        return Name;
    }
}