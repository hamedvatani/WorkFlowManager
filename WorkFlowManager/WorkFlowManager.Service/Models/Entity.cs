using System.ComponentModel.DataAnnotations;
using WorkFlowManager.Service.Interfaces;

namespace WorkFlowManager.Service.Models;

public class Entity : BaseModel, IEntity
{
    [Required]
    public string Json { get; set; } = "";

    public int CurrentSequenceNumber { get; set; }

    public virtual ICollection<EntityFlow> EntityFlows { get; set; } = null!;

    public string StarterUser { get; set; } = "";
    public string StarterRole { get; set; } = "";
}