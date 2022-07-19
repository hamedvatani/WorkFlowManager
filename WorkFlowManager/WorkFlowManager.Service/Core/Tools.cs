using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorkFlowManager.Service.Models;
using WorkFlowManager.Service.Models.Dto;

namespace WorkFlowManager.Service.Core;

public static class Tools
{
    public static Step? GetStartStep(this WorkFlow workFlow)
    {
        return workFlow.Steps.FirstOrDefault(s => s.StepType == StepTypeEnum.Start);
    }

    public static Entity? CreateEntity(this string json)
    {
        if (string.IsNullOrEmpty(json))
            return null;

        dynamic d = JsonConvert.DeserializeObject(json) ?? "";
        if (d == "" || d.Id == null)
            return null;

        return new Entity
        {
            Id = d.Id,
            Json = json
        };
    }
}