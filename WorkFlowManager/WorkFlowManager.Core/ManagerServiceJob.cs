namespace WorkFlowManager.Core;

public class ManagerServiceJob
{
    public string JobType { get; set; } = "";
    public int EntityId { get; set; }
    public int WorkFlowId { get; set; }
    public int CartableItemId { get; set; }
    public string Result { get; set; } = "";
}