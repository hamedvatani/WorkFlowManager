namespace WorkFlowManager.Shared.Models.Dto;

public sealed class EntityDto
{
    public int Id { get; set; }
    public string Json { get; set; } = "";
    public string StarterUser { get; set; } = "";
    public string StarterRole { get; set; } = "";
    public List<EntityLogDto> EntityLogs { get; set; } = new();

    public EntityDto()
    {
    }

    public EntityDto(Entity entity)
    {
        Id = entity.Id;
        Json = entity.Json;
        StarterUser = entity.StarterUser;
        StarterRole = entity.StarterRole;
        EntityLogs = entity.EntityLogs.Select(x => new EntityLogDto(x)).ToList();
    }

    public Entity ToEntity()
    {
        return new Entity
        {
            Id = Id,
            Json = Json,
            StarterUser = StarterUser,
            StarterRole = StarterRole,
            EntityLogs = EntityLogs.Select(x => x.ToEntityLog()).ToList()

        };
    }
}