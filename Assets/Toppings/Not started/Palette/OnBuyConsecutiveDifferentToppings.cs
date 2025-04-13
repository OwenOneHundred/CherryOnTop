using System.Collections.Generic;
using EventBus;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Effects/Money/MoneyOnConsecutiveDistinctPurchases")]
public class OnBuyConsecutiveDifferentToppings : EffectSO
{
    [SerializeField] int number = 5;
    List<Topping> recentToppings = new List<Topping>();
    public override void OnTriggered(IEvent eventObject)
    {
        if (eventObject is BuyEvent buyEvent && buyEvent.item is Topping topping)
        {
            recentToppings.Add(topping);

            if (recentToppings.Count == number)
            {
                if (CheckIfAllToppingsDifferent())
                {
                    Inventory.inventory.Money += 8;
                }

                recentToppings.Clear();
            }
        }
    }

    private bool CheckIfAllToppingsDifferent()
    {
        return recentToppings.Distinct().Count() == recentToppings.Count();
    }
}
