using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public sealed class AddCustomUserRoleCartableStepDto
{
    [Required]
    public int WorkFlowId { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public StepTypeEnum StepType { get; set; }
    public string Description { get; set; } = "";

    [Required]
    public string CustomUser { get; set; } = "";

    [Required]
    public string CustomRole { get; set; } = "";
}