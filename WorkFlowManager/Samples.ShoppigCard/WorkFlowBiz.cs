using WorkFlowManager.Core.Interfaces;
using WorkFlowManager.Core.Models;

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
        if (wfManager.GetWorkFlow("MyWorkFlow") != null)
            return;
    }
}