using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Service.Models;

public class WorkFlow : BaseModel
{
    [Required]
    public string Name { get; set; } = "";

    [Required]
    public string EntityName { get; set; } = "";

    public string StarterUser { get; set; } = "";
    public string StarterRole { get; set; } = "";

    public virtual ICollection<Step> Steps { get; set; } = null!;

    public override string ToString()
    {
        return Name;
    }
}