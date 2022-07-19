using Microsoft.AspNetCore.Mvc;

namespace WorkFlowManager.Service.Models.Dto;

public class MethodResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";

    public static MethodResult Success => new MethodResult {IsSuccess = true};
    public static MethodResult Error(string message) => new MethodResult {IsSuccess = false, Message = message};
}