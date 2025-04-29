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
            if (!CheckIfAllToppingsDifferent())
            {
                recentToppings.Clear();
                return;
            }

            if (recentToppings.Count == number)
            {
                GetTopping().moneyGained += 4;

                Inventory.inventory.Money += 4;

                GetToppingActivatedGlow().StartNewFireEffect("Red", Color.red, 2);

                PlayTriggeredSound();

                recentToppings.Clear();
            }
        }
    }

    private bool CheckIfAllToppingsDifferent()
    {
        return recentToppings.Select(x => x.name).Distinct().Count() == recentToppings.Count();
    }
}
