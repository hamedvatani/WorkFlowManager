using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Core.Models;

public class Flow : BaseModel
{
    [Required]
    [ForeignKey(nameof(SourceStep))]
    public int SourceStepId { get; set; }

    [Required]
    [ForeignKey(nameof(DestinationStep))]
    public int DestinationStepId { get; set; }

    public string Condition { get; set; } = "";

    public Step SourceStep { get; set; } = null!;

    [ForeignKey("DestinationStepId")]
    public virtual Step DestinationStep { get; set; } = null!;

    public override string ToString()
    {
        return $"{SourceStep.Name} -- {Condition} --> {DestinationStep.Name}";
    }
}