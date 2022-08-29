using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public class AddWorkFlowDto
{
    [Required]
    public string Name { get; set; } = "";
}