using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public class GetServiceCartableDto
{
    [Required]
    public string ServiceName { get; set; } = "";
}