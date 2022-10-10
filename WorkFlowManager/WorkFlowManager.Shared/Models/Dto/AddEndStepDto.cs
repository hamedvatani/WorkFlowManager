using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public sealed class AddEndStepDto
{
    [Required]
    public int WorkFlowId { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";
}