using Newtonsoft.Json;

namespace WorkFlowManager.Core.Contract;

public interface IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string StarterUser { get; set; }
    public string StarterRole { get; set; }
}