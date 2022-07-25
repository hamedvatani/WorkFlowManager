namespace WorkFlowManager.Service.Interfaces;

public interface IEntity
{
    public int Id { get; set; }
    public string StarterUser { get; set; }
    public string StarterRole { get; set; }
}