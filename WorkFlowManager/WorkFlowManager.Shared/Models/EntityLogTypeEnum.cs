namespace WorkFlowManager.Shared.Models;

public enum EntityLogTypeEnum
{
    Idle,
    StartStep,
    AddOnSucceed,
    AddOnFailed,
    ServiceSucceed,
    ServiceFailed,
    CartableSucceed,
    GeneralSucceed,
    GeneralFailed
}