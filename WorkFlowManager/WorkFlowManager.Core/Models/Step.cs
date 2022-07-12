using System.ComponentModel.DataAnnotations.Schema;
using WorkFlowManager.Core.Models.Steps;

namespace WorkFlowManager.Core.Models;

public class Step : BaseModel
{
    public StepTypeEnum StepType { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public int WorkFlowId { get; set; }
    
    [ForeignKey("WorkFlowId")]
    public WorkFlow? WorkFlow { get; set; }
}