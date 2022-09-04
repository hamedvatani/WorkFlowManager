using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public class GetRoleCartableDto
{
    [Required]
    public string Role { get; set; } = "";
}