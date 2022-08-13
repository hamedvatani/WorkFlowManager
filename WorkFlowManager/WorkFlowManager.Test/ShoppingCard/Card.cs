using System.Collections.Generic;
using WorkFlowManager.Core.Contract;

namespace WorkFlowManager.Test.ShoppingCard;

public class Card : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "ShoppingCard";
    public string StarterUser { get; set; } = "";
    public string StarterRole { get; set; } = "";
    public List<Item> Items { get; set; } = new();
}