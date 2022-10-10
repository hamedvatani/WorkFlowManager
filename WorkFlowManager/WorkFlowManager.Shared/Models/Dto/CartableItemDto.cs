namespace WorkFlowManager.Shared.Models.Dto;

public sealed class CartableItemDto
{
    public int Id { get; set; }
    public int EntityId { get; set; }
    public int StepId { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public string User { get; set; } = "";
    public string Role { get; set; } = "";
    public string ServiceName { get; set; } = "";
    public string PossibleActions { get; set; } = "";
    public EntityDto Entity { get; set; } = null!;
    public StepDto Step { get; set; } = null!;

    public CartableItemDto()
    {
    }

    public CartableItemDto(CartableItem cartableItem)
    {
        Id = cartableItem.Id;
        EntityId = cartableItem.EntityId;
        StepId = cartableItem.StepId;
        TimeStamp = cartableItem.TimeStamp;
        User = cartableItem.User;
        Role = cartableItem.Role;
        ServiceName = cartableItem.ServiceName;
        PossibleActions = cartableItem.PossibleActions;
        Entity = new EntityDto(cartableItem.Entity);
        Step = new StepDto(cartableItem.Step);
    }

    public CartableItem ToCartableItem()
    {
        return new CartableItem
        {
            Id = Id,
            EntityId = EntityId,
            StepId = StepId,
            TimeStamp = TimeStamp,
            User = User,
            Role = Role,
            ServiceName = ServiceName,
            PossibleActions = PossibleActions,
            Entity = Entity.ToEntity(),
            Step = Step.ToStep()
        };
    }
}