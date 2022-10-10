using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models;

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

    public string AddOnWorkerDllFileName { get; set; } = "";
    public string AddOnWorkerClassName { get; set; } = "";

    public string ServiceName { get; set; } = "";

    [Required]
    [ForeignKey(nameof(WorkFlow))]
    public int WorkFlowId { get; set; }

    public virtual WorkFlow WorkFlow { get; set; } = null!;

    [InverseProperty(nameof(Flow.SourceStep))]
    public virtual ICollection<Flow> Heads { get; set; } = new List<Flow>();

    [InverseProperty(nameof(Flow.DestinationStep))]
    public virtual ICollection<Flow> Tails { get; set; } = new List<Flow>();

    public override string ToString()
    {
        return Name;
    }

    public string GetDescription(Entity entity)
    {
        string owner = ", Owner : ";
        switch (ProcessType)
        {
            case ProcessTypeEnum.AddOnWorker:
                owner += $"Add On Worker : {Path.GetFileName(AddOnWorkerDllFileName)}:{AddOnWorkerClassName}";
                break;
            case ProcessTypeEnum.Service:
                owner += $"Service : {ServiceName}";
                break;
            case ProcessTypeEnum.StarterUserOrRole:
                owner += $"Cartable User,Role : {entity.StarterUser},{entity.StarterRole}";
                break;
            case ProcessTypeEnum.CustomUserOrRole:
                owner += $"Cartable User,Role : {CustomUser},{CustomRole}";
                break;
            default:
                owner = "";
                break;
        }

        return $"Name : {Name}, Type : {StepType}, Process : {ProcessType}{owner}";
    }
}