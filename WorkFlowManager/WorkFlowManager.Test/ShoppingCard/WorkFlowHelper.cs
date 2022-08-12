using WorkFlowManager.Core.Contract;
using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Test.ShoppingCard;

public class WorkFlowHelper
{
    private readonly IManager _manager;
    private WorkFlow workFlow = null!;

    public WorkFlowHelper(IManager manager)
    {
        _manager = manager;
        GetOrCreateWorkFlow();
    }

    private void GetOrCreateWorkFlow()
    {
        var wf = _manager.GetWorkFlow("MyWorkFlow");
        if (wf != null)
        {
            workFlow = wf;
            return;
        }

        workFlow = _manager.AddWorkFlow("MyWorkFlow", "ShoppingCard");
    }
}