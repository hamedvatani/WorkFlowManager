using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public sealed class GetServiceCartableDto
{
    [Required]
    public string ServiceName { get; set; } = "";
}