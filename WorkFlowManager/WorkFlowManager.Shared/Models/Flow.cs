﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models;

public class Flow : BaseModel
{
    [Required]
    [ForeignKey(nameof(SourceStep))]
    public int SourceStepId { get; set; }

    [Required]
    [ForeignKey(nameof(DestinationStep))]
    public int DestinationStepId { get; set; }

    public string Condition { get; set; } = "";

    public virtual Step SourceStep { get; set; } = null!;

    public virtual Step DestinationStep { get; set; } = null!;

    public override string ToString()
    {
        return $"{SourceStep.Name} -- {Condition} --> {DestinationStep.Name}";
    }
}