﻿namespace WorkFlowManager.Core.Contract;

public class ManagerConfiguration
{
    public string RabbitMqHostName { get; set; } = "127.0.0.1";
    public string RabbitMqUserName { get; set; } = "guest";
    public string RabbitMqPassword { get; set; } = "guest";
    public string InputQueueName { get; set; } = "WorkFlowManager.Input";
}