using WorkFlowManager.Core.Contract;
using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Test.ShoppingCard;

public class WorkFlowHelper
{
    private readonly IManager _manager;
    private WorkFlow _workFlow = null!;
    public Card Card { get; set; } = null!;

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
            _workFlow = wf;
            return;
        }

        var worker = _manager.AddWorker(
            @"C:\Projects\WorkFlowManager\WorkFlowManager\WorkFlowManager.Test\bin\Debug\net6.0\WorkFlowManager.Test.dll",
            "WorkFlowManager.Test.ShoppingCard.IsExists");

        _workFlow = _manager.AddWorkFlow("MyWorkFlow", "ShoppingCard");
        var startStep = _manager.AddStep(_workFlow, "Start", StepTypeEnum.Start, ProcessTypeEnum.None, "Start Step", "",
            "", null);
        var isExistsStep = _manager.AddStep(_workFlow, "IsExists", StepTypeEnum.Condition, ProcessTypeEnum.AddOnWorker,
            "Check For Items Existence", "", "", worker);
        var doShoppingStep = _manager.AddStep(_workFlow, "DoShopping", StepTypeEnum.Process, ProcessTypeEnum.Service,
            "Do Shopping Process", "", "", null);
        var errorReportStep = _manager.AddStep(_workFlow, "ErrorReport", StepTypeEnum.Process, ProcessTypeEnum.Service,
            "Error Report Process", "", "", null);
        var getAcceptanceStep = _manager.AddStep(_workFlow, "GetAcceptance", StepTypeEnum.Condition,
            ProcessTypeEnum.StarterUser, "Get User Acceptance", "", "", null);
        var endStep = _manager.AddStep(_workFlow, "End", StepTypeEnum.End, ProcessTypeEnum.None, "End Step", "", "",
            null);

        _manager.AddFlow(startStep, isExistsStep, "");
        _manager.AddFlow(isExistsStep, doShoppingStep, "Yes");
        _manager.AddFlow(isExistsStep, errorReportStep, "No");
        _manager.AddFlow(isExistsStep, getAcceptanceStep, "Semi");
        _manager.AddFlow(doShoppingStep, endStep, "");
        _manager.AddFlow(errorReportStep, endStep, "");
        _manager.AddFlow(getAcceptanceStep, doShoppingStep, "Accept");
        _manager.AddFlow(getAcceptanceStep, errorReportStep, "Reject");
    }
}