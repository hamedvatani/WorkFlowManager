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
        var wf = wfManager.CreateOrGetWorkFlow("MyWorkFlow");



        // var wf = wfManager.CreateWorkFlow("MyWorkFlow");
        // var startStep = wfManager.CreateStep(WorkFlowStepTypeEnum.Start);
        // wf.SetStartStep(startStep);
        // var isExistsStep = wfManager.CreateStep(WorkFlowStepTypeEnum.Decision);
        // startStep.SetNextStep(isExistsStep, "");
        // var doShoppingStep = wfManager.CreateStep(WorkFlowStepTypeEnum.Process);
        // isExistsStep.SetNextStep(doShoppingStep, "Yes");
        // var errorReportStep = wfManager.CreateStep(WorkFlowStepTypeEnum.Process);
        // isExistsStep.SetNextStep(errorReportStep, "No");
        // var getAcceptenceStep = wfManager.CreateStep(WorkFlowStepTypeEnum.Process);
        // isExistsStep.SetNextStep(getAcceptenceStep, "Semi");
        // getAcceptenceStep.SetNextStep(doShoppingStep, "Accept");
        // getAcceptenceStep.SetNextStep(errorReportStep, "Reject");
        // var endStep = wfManager.CreateStep(WorkFlowStepTypeEnum.End);
        // wf.SetEndStep(endStep);
        // errorReportStep.SetNextStep(endStep, "");
        // doShoppingStep.SetNextStep(endStep, "");
    }
}