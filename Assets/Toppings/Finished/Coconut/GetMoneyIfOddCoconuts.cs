using System;
using System.Linq;
using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/GetMoneyIfOddCoconuts")]
public class GetMoneyIfOddCoconuts : EffectSO
{
    [SerializeField] bool odd = false;
    [SerializeField] int moneyAmount = 4;
    public override void OnTriggered(IEvent eventObject)
    {
        int onCake = ToppingRegistry.toppingRegistry.PlacedToppings.Where(x => x.topping.name == "Coconut").Count();
        int inInventory = Inventory.inventory.ownedItems.Where(x => x.name == "Coconut").Count();
        if (odd ? (onCake + inInventory) % 2 == 1 : (onCake + inInventory) % 2 == 0)
        {
            GetTopping().moneyGained += moneyAmount;
            Inventory.inventory.Money += moneyAmount;
        }
    }
}
