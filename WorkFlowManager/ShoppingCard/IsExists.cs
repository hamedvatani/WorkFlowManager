namespace ShoppingCard;

public class IsExists
{
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