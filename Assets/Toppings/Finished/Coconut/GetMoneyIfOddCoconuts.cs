using System;
using System.Linq;
using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/GetMoneyIfOddCoconuts")]
public class GetMoneyIfOddCoconuts : EffectSO
{
    [SerializeField] int moneyAmount = 4;
    public override void OnTriggered(IEvent eventObject)
    {
        int onCake = ToppingRegistry.toppingRegistry.PlacedToppings.Where(x => x.topping.name == "Coconut").Count();
        int inInventory = Inventory.inventory.ownedItems.Where(x => x.name == "Coconut").Count();
        if ((onCake + inInventory) % 2 == 1)
        {
            Inventory.inventory.Money += moneyAmount;
        }
    }
}
