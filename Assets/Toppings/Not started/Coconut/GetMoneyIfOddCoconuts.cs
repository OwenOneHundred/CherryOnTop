using System;
using System.Linq;
using EventBus;
using UnityEngine;

public class GetMoneyIfOddCoconuts : EffectSO
{
    [SerializeField] int moneyAmount = 4;
    public override void OnTriggered(IEvent eventObject)
    {
        if (ToppingRegistry.toppingRegistry.allItems.Where(x => x.name == "Coconut").Count() % 2 == 1)
        {
            Inventory.inventory.Money += moneyAmount;
        }
    }
}
