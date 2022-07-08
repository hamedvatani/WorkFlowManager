using WorkFlowManager.Core.Interfaces;

namespace Samples.ShoppigCard;

public class WorkFlowBiz
{
    private IWfManager wfManager;

    public WorkFlowBiz(IWfManager wfManager)
    {
        this.wfManager = wfManager;
    }

    public void CreateWorkFlow()
    {
        
    }
}