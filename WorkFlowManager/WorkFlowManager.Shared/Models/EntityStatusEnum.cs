namespace WorkFlowManager.Shared.Models;

public enum EntityStatusEnum
{
    Idle,
    Start,
    RunningStep,
    FailInStep,
    WaitForService,
    WaitForCartable,
    End
}