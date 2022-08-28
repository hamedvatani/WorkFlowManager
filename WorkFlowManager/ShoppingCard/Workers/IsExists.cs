using Newtonsoft.Json;
using WorkFlowManager.Client;
using WorkFlowManager.Client.Models;

namespace ShoppingCard.Workers;

public class IsExists : IWorker
{

    public Task<string> RunWorkerAsync(Entity entity)
    {
        var card = JsonConvert.DeserializeObject<Card>(entity.Json);
        if (card == null)
            throw new Exception("Invalid data!");
        var result = Check(card);
        if (result.Item1)
            return Task.FromResult("Yes");
        if (result.Item2)
            return Task.FromResult("No");
        if (result.Item3)
            return Task.FromResult("Semi");
        throw new Exception("Unknown!");
    }

    private Tuple<bool, bool, bool> Check(Card card)
    {
        bool allExists = true;
        bool allNotExists = false;
        foreach (var item in card.Items)
        {
            bool itemExists = item.Quantity <= item.Stock;
            allExists &= itemExists;
            allNotExists |= itemExists;
        }

        var someExists = !allNotExists & !allExists;

        return new Tuple<bool, bool, bool>(allExists, allNotExists, someExists);
    }
}