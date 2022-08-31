namespace WorkFlowManager.Shared;

public class MethodResult<T>
{
    public bool IsSuccess { get; set; }
    public bool IsTimeout { get; set; }
    public T? ReturnValue { get; set; }
    public string Message { get; set; } = "";
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    public static MethodResult<T> Ok()
    {
        return new MethodResult<T> { IsSuccess = true };
    }

    public static MethodResult<T> Ok(T returnValue)
    {
        return new MethodResult<T> { IsSuccess = true, ReturnValue = returnValue };
    }

    public static MethodResult<T> Ok(T returnValue, string message)
    {
        return new MethodResult<T> { IsSuccess = true, ReturnValue = returnValue, Message = message };
    }

    public static MethodResult<T> Error(string message)
    {
        return new MethodResult<T> { Message = message };
    }

    public static MethodResult<T> Timeout()
    {
        return new MethodResult<T> { IsTimeout = true };
    }

    public T GetResult()
    {
        if (!IsSuccess)
            throw new Exception(Message);
        if (ReturnValue == null)
            throw new Exception("Null Value");
        return ReturnValue;
    }

    public override string ToString()
    {
        var retVal = IsSuccess ? "Ok." : "Fail!";
        if (IsTimeout)
            retVal += " Timeout!";
        if (IsSuccess && ReturnValue == null)
            retVal += " Result: NULL";
        if (IsSuccess && ReturnValue != null)
            retVal += $" Result: {ReturnValue}";
        return retVal;
    }
}

public class MethodResult
{
    public bool IsSuccess { get; set; }
    public bool IsTimeout { get; set; }
    public string Message { get; set; } = "";
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    public static MethodResult Ok()
    {
        return new MethodResult { IsSuccess = true };
    }

    public static MethodResult Ok(string message)
    {
        return new MethodResult { IsSuccess = true, Message = message };
    }

    public static MethodResult Error(string message)
    {
        return new MethodResult { Message = message };
    }

    public static MethodResult Timeout()
    {
        return new MethodResult { IsTimeout = true };
    }

    public override string ToString()
    {
        var retVal = IsSuccess ? "Ok." : "Fail!";
        if (IsTimeout)
            retVal += " Timeout!";
        return retVal;
    }
}