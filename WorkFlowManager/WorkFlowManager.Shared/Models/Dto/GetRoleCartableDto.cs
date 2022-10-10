using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public sealed class GetRoleCartableDto
{
    [Required]
    public string Role { get; set; } = "";
}