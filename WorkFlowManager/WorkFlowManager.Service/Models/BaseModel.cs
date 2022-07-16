using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Service.Models;

public abstract class BaseModel
{
    [Key]
    public int Id { get; set; }
}