using WorkFlowManager.Core.Contract;

namespace WorkFlowManager.Test.ShoppingCard;

public class WorkFlowHelper
{
    private IManager _manager;

    public WorkFlowHelper(IManager manager)
    {
        _manager = manager;
    }
}