using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/GetDuplicateOfMostRecentItem")]
public class GetDuplicateOfRecent : EffectSO
{
    public override void OnTriggered(IEvent eventObject)
    {
        if (Shop.shop.mostRecentlyBoughtItem != null)
        {
            Inventory.inventory.TryBuyItem(Shop.shop.mostRecentlyBoughtItem);
            GetToppingActivatedGlow().StartNewFireEffect("Yellow", Color.yellow, 2);
        }
    }
}
