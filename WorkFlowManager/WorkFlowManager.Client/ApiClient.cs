﻿using System.Net.Http.Json;
using WorkFlowManager.Shared;

namespace WorkFlowManager.Client;

public class ApiClient
{
    private readonly HttpClient _httpClient = new();
    private readonly string _baseUri;

    public ApiClient(ClientConfiguration configuration)
    {
        _baseUri = $"http://{configuration.ApiAddress}:{configuration.ApiPort}/api/Manager/";
    }

    public MethodResult<T2> CallPostApi<T1, T2>(string methodName, T1 model)
    {
        try
        {
            var response = _httpClient.PostAsJsonAsync(_baseUri + methodName, model).Result;
            if (!response.IsSuccessStatusCode)
                return MethodResult<T2>.Error(response.Content.ReadAsStringAsync().Result);
            var result = response.Content.ReadFromJsonAsync<T2>().Result;
            if (result == null)
                return MethodResult<T2>.Error("Unknown error!");
            return MethodResult<T2>.Ok(result);
        }
        catch (Exception e)
        {
            return MethodResult<T2>.Error(e.Message);
        }
    }

    public MethodResult CallPostApi<T1>(string methodName, T1 model)
    {
        try
        {
            var response = _httpClient.PostAsJsonAsync(_baseUri + methodName, model).Result;
            if (!response.IsSuccessStatusCode)
                return MethodResult.Error(response.Content.ReadAsStringAsync().Result);
            return MethodResult.Ok();
        }
        catch (Exception e)
        {
            return MethodResult.Error(e.Message);
        }
    }
}