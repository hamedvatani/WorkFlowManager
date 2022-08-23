using System.Text;
using Newtonsoft.Json;

namespace RPC.RabbitMq;

public class RpcFunctionDto
{
    public string FunctionName { get; set; } = "";
    public Dictionary<string, string> Parameters { get; set; } = null!;

    public RpcFunctionDto()
    {
    }

    public RpcFunctionDto(string functionName, params KeyValuePair<string, string>[] parameters)
    {
        FunctionName = functionName;
        Parameters = new Dictionary<string, string>();
        foreach (var parameter in parameters)
            Parameters.Add(parameter.Key, parameter.Value);
    }

    public RpcFunctionDto(byte[] data)
    {
        var dto = JsonConvert.DeserializeObject<RpcFunctionDto>(Encoding.UTF8.GetString(data));
        if (dto == null)
            throw new Exception("Can't deserialize data");
        FunctionName = dto.FunctionName;
        Parameters = dto.Parameters;
    }

    public byte[] Serialize()
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
    }
}