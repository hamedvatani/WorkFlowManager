using System.Text;
using Newtonsoft.Json;

namespace RPC.RabbitMq;

public class RpcResultDto
{
    public bool IsSuccess { get; set; }
    public bool IsTimeout { get; set; }
    public string Message { get; set; } = "";
    public Dictionary<string, string> Parameters { get; set; } = null!;

    public RpcResultDto()
    {
    }

    public RpcResultDto(bool isSuccess, bool isTimeout, string message,
        params KeyValuePair<string, string>[] parameters)
    {
        IsSuccess = isSuccess;
        IsTimeout = isTimeout;
        Message = message;
        Parameters = new Dictionary<string, string>();
        foreach (var parameter in parameters)
            Parameters.Add(parameter.Key, parameter.Value);
    }

    public RpcResultDto(byte[] data)
    {
        var dto = JsonConvert.DeserializeObject<RpcResultDto>(Encoding.UTF8.GetString(data));
        if (dto == null)
            throw new Exception("Can't deserialize data");
        IsSuccess = dto.IsSuccess;
        IsTimeout = dto.IsTimeout;
        Message = dto.Message;
        Parameters = dto.Parameters;
    }

    public byte[] Serialize()
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
    }
}