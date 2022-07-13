using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Core.Models;

public class Step : BaseModel
{
    [Required]
    public StepTypeEnum StepType { get; set; }
    [Required]
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    [Required]
    public int WorkFlowId { get; set; }

    [ForeignKey("WorkFlowId")]
    public virtual WorkFlow WorkFlow { get; set; } = null!;

    [InverseProperty(nameof(Flow.SourceStep))]
    public virtual ICollection<Flow> Heads { get; set; } = null!;
    [InverseProperty(nameof(Flow.DestinationStep))]
    public virtual ICollection<Flow> Tails { get; set; } = null!;

    public override string ToString()
    {
        return Name;
    }
}