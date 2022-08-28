using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Client.Models.Dto;

public class AddCartableStepDto
{
    [Required]
    public int WorkFlowId { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public StepTypeEnum StepType { get; set; }

    [Required]
    public ProcessTypeEnum ProcessType { get; set; }

    public string Description { get; set; } = "";
    public string CustomUser { get; set; } = "";
    public string CustomRole { get; set; } = "";
}