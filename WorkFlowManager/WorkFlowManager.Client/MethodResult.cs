using System.Xml;
using System.Collections.Generic;
using System.Globalization;

namespace WorkFlowManager.Client;

public class MethodResult
{
    public bool IsSuccess { get; set; }
    public bool IsTimeout { get; set; }
    public string Message { get; set; } = "";
    public string ReturnValue { get; set; } = "";
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    public MethodResult()
    {
    }

    public MethodResult(RpcDto dto)
    {
        try
        {
            IsSuccess = bool.Parse(dto.Parameters["IsSuccess"]);
            IsTimeout = bool.Parse(dto.Parameters["IsTimeout"]);
            Message = dto.Parameters["Message"];
            ReturnValue = dto.Parameters["ReturnValue"];
            TimeStamp = DateTime.Parse(dto.Parameters["TimeStamp"]);
        }
        catch (Exception)
        {
            Message = "Can't parse data!";
        }
    }

    public RpcDto ToRpcDto()
    {
        return new RpcDto("",
            new KeyValuePair<string, string>("IsSuccess", IsSuccess.ToString()),
            new KeyValuePair<string, string>("IsTimeout", IsTimeout.ToString()),
            new KeyValuePair<string, string>("Message", Message),
            new KeyValuePair<string, string>("ReturnValue", ReturnValue),
            new KeyValuePair<string, string>("TimeStamp", TimeStamp.ToString(CultureInfo.InvariantCulture)));
    }

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

    public static MethodResult Timeout()
    {
        return new MethodResult { IsTimeout = true };
    }
}