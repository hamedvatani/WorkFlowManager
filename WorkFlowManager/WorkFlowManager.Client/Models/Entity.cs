﻿using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Client.Models;

public class Entity : BaseModel
{
    [Required]
    public string Json { get; set; } = "";

    public string StarterUser { get; set; } = "";
    public string StarterRole { get; set; } = "";
 
    public virtual ICollection<EntityLog> EntityLogs { get; set; } = null!;
}