using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Rerolls/GetRerollIfSoldFruit")]
public class GetRerollIfSoldFruit : EffectSO
{
    [SerializeField] float chance;
    public override void OnTriggered(IEvent eventObject)
    {
        if (eventObject is SellEvent sellEvent && sellEvent.item is Topping topping && topping.flags.HasFlag(ToppingTypes.Flags.fruit))
        {
            if (Random.value <= chance)
            {
                Shop.shop.Rerolls += 1;
                GetToppingActivatedGlow().StartNewFireEffect("Red", Color.red, 2);
                PlayTriggeredSound();
            }
        }
    }
}
