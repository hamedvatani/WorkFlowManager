namespace WorkFlowManager.Client.Models;

public enum EntityStatusEnum
{
    Idle,
    Start,
    RunningStep,
    FailInStep,
    End
}