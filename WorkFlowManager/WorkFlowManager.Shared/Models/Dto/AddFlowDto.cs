using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public class AddFlowDto
{
    [Required]
    public int SourceStepId { get; set; }

    [Required]
    public int DestinationStepId { get; set; }

    public string Condition { get; set; } = "";
}