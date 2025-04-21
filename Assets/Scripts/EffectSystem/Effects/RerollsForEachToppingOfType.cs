using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="Effects/MoneyIfMoneyEndsWith")]
public class RerollsForEachToppingOfType : EffectSO
{
    [SerializeField] char endingNumber;
    [SerializeField] ToppingTypes.Flags flag;
    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        foreach (ToppingRegistry.ItemInfo itemInfo in ToppingRegistry.toppingRegistry.PlacedToppings)
        {
            if (itemInfo.topping.flags.HasFlag(flag))
            {
                Debug.Log("This would add rerolls to the shop but that's not supported yet.");
            }
        }
    }
}