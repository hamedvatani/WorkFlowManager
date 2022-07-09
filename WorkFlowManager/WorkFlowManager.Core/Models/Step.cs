namespace WorkFlowManager.Core.Models;

public abstract class Step
{
    public void SetNextStep(Step nextStep,string condition)
    {
        throw new NotImplementedException();
    }
}