using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Core.Models;

public abstract class BaseModel
{
    [Key]
    public int Id { get; set; }
}