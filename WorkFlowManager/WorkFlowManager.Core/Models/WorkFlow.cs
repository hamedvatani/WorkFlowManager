using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorkFlowManager.Core.Models;

public class WorkFlow : BaseModel
{
    [Required]
    public string Name { get; set; } = "";

    public virtual ICollection<Step> Steps { get; set; } = null!;

    public override string ToString()
    {
        return Name;
    }
}