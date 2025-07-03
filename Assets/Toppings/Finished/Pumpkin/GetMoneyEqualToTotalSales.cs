using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/GetMoneyEqualToTotalSales")]
public class GetMoneyEqualToTotalSales : EffectSO
{
    OnSellAnyTopping sellEvent;
    int totalSales = 0;
    public override void OnRegistered()
    {
        sellEvent = (OnSellAnyTopping) CreateInstance(typeof(OnSellAnyTopping));
        CalledOnSellEffect calledOnSellEffect = (CalledOnSellEffect) CreateInstance(typeof(CalledOnSellEffect));
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
            GetTopping().moneyGained += totalSales;
            Inventory.inventory.Money += totalSales;
        }
    }

    public void OnSellNearby()
    {
        totalSales += 1;
        GetTopping().TriggersCount += 1;
        PlayTriggeredSound();
    }

    public class CalledOnSellEffect : EffectSO
    {
        public GetMoneyEqualToTotalSales owner;
        public override void OnTriggered(IEvent eventObject)
        {
            if (eventObject is SellEvent sellEvent && sellEvent.toppingObj != null)
            {
                if (Vector3.Distance(sellEvent.toppingObj.transform.position, owner.toppingObj.transform.position) <= 1.75f)
                {
                    owner.OnSellNearby();
                }
            }
        }
    }
}
