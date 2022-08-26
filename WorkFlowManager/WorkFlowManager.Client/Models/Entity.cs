using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkFlowManager.Client.Models;

public class Entity : BaseModel
{
    [Required]
    public string Json { get; set; } = "";

    public string StarterUser { get; set; } = "";
    public string StarterRole { get; set; } = "";
    public EntityStatusEnum Status { get; set; }
    public DateTime LastRunDate { get; set; } = DateTime.Now;

    [ForeignKey(nameof(CurrentStep))]
    public int? CurrentStepId { get; set; } = null;

    public virtual Step? CurrentStep { get; set; } = null;
    
    public virtual ICollection<EntityLog> EntityLogs { get; set; } = null!;
}