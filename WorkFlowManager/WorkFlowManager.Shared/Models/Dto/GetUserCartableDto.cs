using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public class GetUserCartableDto
{
    [Required]
    public string User { get; set; } = "";
}