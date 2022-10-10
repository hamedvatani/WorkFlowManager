using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Shared.Models;

public class WorkFlow : BaseModel
{
    [Required]
    public string Name { get; set; } = "";

    [InverseProperty(nameof(Step.WorkFlow))]
    public virtual ICollection<Step> Steps { get; set; } = new List<Step>();

    public override string ToString()
    {
        return Name;
    }
}