using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/GetMoneyEqualToTotalSales")]
public class GetMoneyEqualToTotalSales : EffectSO
{
    EventSO<SellEvent> sellEvent;
    int totalSales = 0;
    public override void OnRegistered()
    {
        CalledOnSellEffect calledOnSellEffect = new();
        calledOnSellEffect.owner = this;
        sellEvent.RegisterEffect(calledOnSellEffect);
    }

    public override void OnDeregistered()
    {
        if (sellEvent == null) { return; }
        sellEvent.DeregisterAllEffects();
    }

    public override void OnTriggered(IEvent eventObject)
    {
        if (totalSales > 0)
        {
            Inventory.inventory.Money += totalSales;
        }
    }

    public void OnSellNearby()
    {
        totalSales += 1;
        PlayTriggeredSound();
    }

    public class CalledOnSellEffect : EffectSO
    {
        public GetMoneyEqualToTotalSales owner;
        public override void OnTriggered(IEvent eventObject)
        {
            if (eventObject is SellEvent sellEvent)
            {
                if (Vector3.Distance(sellEvent.toppingObj.transform.position, toppingObj.transform.position) <= 1.75f)
                {
                    owner.OnSellNearby();
                }
            }
        }
    }
}
