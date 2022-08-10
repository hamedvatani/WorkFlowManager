using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WorkFlowManager.Core.Contract;

namespace WorkFlowManager.Core.Models;

[Index(nameof(Name), IsUnique = true)]
public class Entity : BaseModel, IEntity
{
    [Required]
    public string Json { get; set; } = "";

    public int CurrentSequenceNumber { get; set; }

    public virtual ICollection<EntityFlow> EntityFlows { get; set; } = null!;

    [Required]
    public string Name { get; set; } = "";

    public string StarterUser { get; set; } = "";
    public string StarterRole { get; set; } = "";
}