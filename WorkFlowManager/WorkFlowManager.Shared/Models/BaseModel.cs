using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models;

public abstract class BaseModel
{
    [Key]
    public int Id { get; set; }
}