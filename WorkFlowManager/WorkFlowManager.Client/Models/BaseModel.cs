using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Client.Models;

public abstract class BaseModel
{
    [Key]
    public int Id { get; set; }
}