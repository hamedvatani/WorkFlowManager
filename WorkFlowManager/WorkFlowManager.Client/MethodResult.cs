namespace WorkFlowManager.Client;

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

    public static MethodResult<T> Error(string message, T returnValue)
    {
        return new MethodResult<T> { Message = message, ReturnValue = returnValue };
    }

    public static MethodResult<T> Timeout()
    {
        return new MethodResult<T> { IsTimeout = true };
    }

    // public MethodResult(RpcDto dto)
    // {
    //     try
    //     {
    //         IsSuccess = bool.Parse(dto.Parameters["IsSuccess"]);
    //         IsTimeout = bool.Parse(dto.Parameters["IsTimeout"]);
    //         Message = dto.Parameters["Message"];
    //         ReturnValue = dto.Parameters["ReturnValue"];
    //         TimeStamp = DateTime.Parse(dto.Parameters["TimeStamp"]);
    //     }
    //     catch (Exception)
    //     {
    //         Message = "Can't parse data!";
    //     }
    // }
    //
    // public RpcDto ToRpcDto()
    // {
    //     return new RpcDto("",
    //         new KeyValuePair<string, string>("IsSuccess", IsSuccess.ToString()),
    //         new KeyValuePair<string, string>("IsTimeout", IsTimeout.ToString()),
    //         new KeyValuePair<string, string>("Message", Message),
    //         new KeyValuePair<string, string>("ReturnValue", ReturnValue),
    //         new KeyValuePair<string, string>("TimeStamp", TimeStamp.ToString(CultureInfo.InvariantCulture)));
    // }
}