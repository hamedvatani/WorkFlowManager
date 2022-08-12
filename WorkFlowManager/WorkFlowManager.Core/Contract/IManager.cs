﻿using WorkFlowManager.Core.Models;

namespace WorkFlowManager.Core.Contract;

public interface IManager
{
    WorkFlow? GetWorkFlow(string name);
    WorkFlow AddWorkFlow(string name, string entityName);
}