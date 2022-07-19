using WorkFlowManager.Service.Models;
using WorkFlowManager.Service.Models.Dto;

namespace WorkFlowManager.Service.Core;

public interface IWorkFlowManagerCore
{
    MethodResult StartWorkFlow(Entity entity, WorkFlow workFlow);
    MethodResult RunStep(Step step, Entity entity);
}