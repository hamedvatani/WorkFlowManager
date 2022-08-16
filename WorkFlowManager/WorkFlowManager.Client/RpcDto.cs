using System.Text;
using Newtonsoft.Json;

namespace WorkFlowManager.Client;

public class RpcDto
{
    public string FunctionName { get; set; } = null!;
    public Dictionary<string, string> Parameters { get; set; } = null!;

    public RpcDto()
    {
    }

    public RpcDto(string functionName, params KeyValuePair<string, string>[] parameters)
    {
        FunctionName = functionName;
        Parameters = new Dictionary<string, string>();
        foreach (var parameter in parameters)
            Parameters.Add(parameter.Key, parameter.Value);
    }

    public RpcDto(byte[] data)
    {
        var dto = JsonConvert.DeserializeObject<RpcDto>(Encoding.UTF8.GetString(data));
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