using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public sealed class AddStarterUserRoleCartableStepDto
{
    [Required]
    public int WorkFlowId { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public StepTypeEnum StepType { get; set; }
    public string Description { get; set; } = "";
}