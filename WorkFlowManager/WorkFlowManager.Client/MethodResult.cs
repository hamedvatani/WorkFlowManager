namespace WorkFlowManager.Client;

public class MethodResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
    public string ReturnValue { get; set; } = "";
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    public static MethodResult Ok()
    {
        return new MethodResult { IsSuccess = true };
    }

    public static MethodResult Ok(string returnValue)
    {
        return new MethodResult { IsSuccess = true, ReturnValue = returnValue };
    }

    public static MethodResult Ok(string returnValue, string message)
    {
        return new MethodResult { IsSuccess = true, ReturnValue = returnValue, Message = message };
    }

    public static MethodResult Error(string message)
    {
        return new MethodResult { Message = message };
    }

    public static MethodResult Error(string message, string returnValue)
    {
        return new MethodResult { Message = message, ReturnValue = returnValue };
    }
}