using WorkFlowManager.Service.Models;

namespace WorkFlowManager.Service.Repositories;

public interface IWorkFlowRepository
{
    WorkFlow? GetWorkFlow(string workFlowName);
}