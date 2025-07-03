using System.Collections.Generic;
using EventBus;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Effects/Money/MoneyOnConsecutiveDistinctPurchases")]
public class OnBuyConsecutiveDifferentToppings : EffectSO
{
    [SerializeField] int number = 5;
    List<Topping> recentToppings = new List<Topping>();
    Topping topping;
    public override void OnTriggered(IEvent eventObject)
    {
        if (topping == null) { topping = GetTopping(); }

        if (eventObject is BuyEvent buyEvent && buyEvent.item is Topping boughtTopping)
        {
            recentToppings.Add(boughtTopping);
            if (!CheckIfAllToppingsDifferent())
            {
                recentToppings.Clear();
                topping.TriggersCount = 0;
                return;
            }

            if (recentToppings.Count == number)
            {
                topping.moneyGained += 4;

                Inventory.inventory.Money += 4;

                GetToppingActivatedGlow().StartNewFireEffect("Red", Color.red, 2);

                PlayTriggeredSound();

                recentToppings.Clear();

                topping.TriggersCount = 0;
            }
            else
            {
                topping.TriggersCount += 1;
            }
        }
    }

    private bool CheckIfAllToppingsDifferent()
    {
        return recentToppings.Select(x => x.name).Distinct().Count() == recentToppings.Count();
    }
}
