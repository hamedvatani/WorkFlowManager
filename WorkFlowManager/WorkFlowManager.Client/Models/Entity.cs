using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Client.Models;

public class Entity : BaseModel, IEntity
{
    [Required]
    public string Json { get; set; } = "";

    public int CurrentSequenceNumber { get; set; }

    public virtual ICollection<EntityLog> EntityLogs { get; set; } = null!;

    [Required]
    public string Name { get; set; } = "";

    public string StarterUser { get; set; } = "";
    public string StarterRole { get; set; } = "";
}