using System.ComponentModel.DataAnnotations;

namespace WorkFlowManager.Shared.Models.Dto;

public sealed class SetCartableItemResultDto
{
    [Required]
    public int CartableItemId { get; set; }

    [Required]
    public string Result { get; set; } = "";
}