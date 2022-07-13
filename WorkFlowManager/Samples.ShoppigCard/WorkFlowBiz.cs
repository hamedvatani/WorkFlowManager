using WorkFlowManager.Core.Interfaces;
using WorkFlowManager.Core.Models;

namespace Samples.ShoppigCard;

public class WorkFlowBiz:IWorkFlowBiz
{
    private IWfManager wfManager;

    public WorkFlowBiz(IWfManager wfManager)
    {
        this.wfManager = wfManager;
    }

    public void CreateMyWorkFlow()
    {
        wfManager.DeleteWorkFlow("MyWorkFlow");

        var wf = wfManager.CreateWorkFlow("MyWorkFlow");

        var startStep = wfManager.CreateStep(wf, StepTypeEnum.Start, "Start", "Start");
        var isExistsStep = wfManager.CreateStep(wf, StepTypeEnum.Condition, "IsExists", "Check Existence of Shopping Card Items");
        var doShoppingStep = wfManager.CreateStep(wf, StepTypeEnum.Process, "DoShopping", "Do Shopping");
        var errorReportStep = wfManager.CreateStep(wf, StepTypeEnum.Process, "ErrorReport", "Report Error");
        var getAcceptenceStep = wfManager.CreateStep(wf, StepTypeEnum.Condition, "GetAcceptence", "Double Check With User");
        var endStep = wfManager.CreateStep(wf, StepTypeEnum.End, "End", "End");

        wfManager.CreateFlow(startStep, isExistsStep, "");
        wfManager.CreateFlow(isExistsStep, doShoppingStep, "Yes");
        wfManager.CreateFlow(isExistsStep, errorReportStep, "No");
        wfManager.CreateFlow(isExistsStep, getAcceptenceStep, "Semi");
        wfManager.CreateFlow(doShoppingStep, endStep, "");
        wfManager.CreateFlow(errorReportStep, endStep, "");
        wfManager.CreateFlow(getAcceptenceStep, doShoppingStep, "Accept");
        wfManager.CreateFlow(getAcceptenceStep, errorReportStep, "Reject");
    }
}