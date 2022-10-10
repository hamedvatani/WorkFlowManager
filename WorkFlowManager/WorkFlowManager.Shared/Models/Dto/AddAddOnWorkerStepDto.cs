using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public sealed class AddAddOnWorkerStepDto
{
    [Required]
    public int WorkFlowId { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public StepTypeEnum StepType { get; set; }

    public string Description { get; set; } = "";

    [Required]
    public string AddOnWorkerDllFileName { get; set; } = "";

    [Required]
    public string AddOnWorkerClassName { get; set; } = "";
}