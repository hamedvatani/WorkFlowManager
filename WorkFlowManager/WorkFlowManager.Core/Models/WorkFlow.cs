namespace WorkFlowManager.Core.Models;

public class WorkFlow:BaseModel
{
    public string Name { get; set; }

    public void SetStartStep(Step startStep)
    {
        throw new NotImplementedException();
    }

    public void SetEndStep(Step endStep)
    {
        throw new NotImplementedException();
    }
}