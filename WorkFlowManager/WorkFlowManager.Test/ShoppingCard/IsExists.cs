using System;
using System.Collections.Generic;
using WorkFlowManager.Core.Contract;

namespace WorkFlowManager.Test.ShoppingCard;

public class IsExists : IWorker
{
    public MethodResult Process(IEntity entity, List<MethodResult> resultLogs)
    {
        try
        {
            var result = Check((Card)entity);
            if (result.Item1)
                return MethodResult.Ok("Yes");
            if (result.Item2)
                return MethodResult.Ok("No");
            if (result.Item3)
                return MethodResult.Ok("Semi");
            return MethodResult.Error("Unknown");
        }
        catch (Exception ex)
        {
            return MethodResult.Error(ex.Message);
        }
    }

    private Tuple<bool,bool,bool> Check(Card card)
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