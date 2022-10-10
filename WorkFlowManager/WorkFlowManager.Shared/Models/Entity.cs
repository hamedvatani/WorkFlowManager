using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models;

public class Entity : BaseModel
{
    [Column(TypeName = "nvarchar(50)")]
    public EntityStatusEnum Status { get; set; }

    [Required]
    public string Json { get; set; } = "";

    public string StarterUser { get; set; } = "";
    public string StarterRole { get; set; } = "";

    [InverseProperty(nameof(EntityLog.Entity))]
    public virtual ICollection<EntityLog> EntityLogs { get; set; } = new List<EntityLog>();
}