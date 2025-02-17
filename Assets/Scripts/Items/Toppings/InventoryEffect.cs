using UnityEngine;

public class InventoryEffect
{
    public Item origin;
}
public abstract class Limit : InventoryEffect
{
    public Limit(int limit)
    {
        this.limit = limit;
    }
    public int limit = 0;
}

public class LimitBuying : Limit
{
    public LimitBuying(int limit) : base(limit) { }
}

public class LimitSelling : Limit
{
    public LimitSelling(int limit) : base(limit) { }
}

public class LimitMoney : Limit
{
    public LimitMoney(int limit) : base(limit) { }
}

public class LimitPurchasePrice : Limit
{
    public LimitPurchasePrice(int limit) : base(limit) { }
}
