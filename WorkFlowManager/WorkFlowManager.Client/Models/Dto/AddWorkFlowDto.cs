﻿using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Client.Models.Dto;

public class AddWorkFlowDto
{
    [Required]
    public string Name { get; set; } = "";

    [Required]
    public string EntityName { get; set; } = "";
}