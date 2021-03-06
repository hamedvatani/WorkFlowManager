using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Service.Models;

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

    [ForeignKey(nameof(AddOneWorker))]
    public int? AddOneWorkerId { get; set; }

    public virtual WorkFlow WorkFlow { get; set; } = null!;

    public virtual AddOneWorker? AddOneWorker { get; set; }

    [InverseProperty(nameof(Flow.SourceStep))]
    public virtual ICollection<Flow> Heads { get; set; } = null!;

    [InverseProperty(nameof(Flow.DestinationStep))]
    public virtual ICollection<Flow> Tails { get; set; } = null!;

    public override string ToString()
    {
        return Name;
    }
}