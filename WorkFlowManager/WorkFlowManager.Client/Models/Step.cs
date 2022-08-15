using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Client.Models;

public class Step : BaseModel
{
    [Required]
    public StepTypeEnum StepType { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";
    public ProcessTypeEnum ProcessType { get; set; }
    public string CustomUser { get; set; } = "";
    public string CustomRole { get; set; } = "";

    [Required]
    [ForeignKey(nameof(WorkFlow))]
    public int WorkFlowId { get; set; }

    [ForeignKey(nameof(AddOnWorker))]
    public int? AddOnWorkerId { get; set; }

    public virtual WorkFlow WorkFlow { get; set; } = null!;

    public virtual AddOnWorker? AddOnWorker { get; set; }

    [InverseProperty(nameof(Flow.SourceStep))]
    public virtual ICollection<Flow> Heads { get; set; } = null!;

    [InverseProperty(nameof(Flow.DestinationStep))]
    public virtual ICollection<Flow> Tails { get; set; } = null!;

    public override string ToString()
    {
        return Name;
    }
}