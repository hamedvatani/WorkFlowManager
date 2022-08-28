﻿using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Client.Models.Dto;

public class AddStartStepDto
{
    [Required]
    public int WorkFlowId { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";
}