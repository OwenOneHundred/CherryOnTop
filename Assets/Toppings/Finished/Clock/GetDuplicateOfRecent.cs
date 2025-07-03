using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/GetDuplicateOfMostRecentItem")]
public class GetDuplicateOfRecent : EffectSO
{
    int count = 0;
    int uses = 3;
    public override void OnRegistered()
    {
        GetTopping().TriggersCount = uses;
    }
    public override void OnTriggered(IEvent eventObject)
    {
        if (Shop.shop.mostRecentlyBoughtItem != null && Shop.shop.mostRecentlyBoughtItem.name != "Clock")
        {
            count += 1;
            GetTopping().TriggersCount = uses - Mathf.Clamp(count, 0, uses);
            if (count > uses) { return; }

            Inventory.inventory.GetItemForFree(Shop.shop.mostRecentlyBoughtItem);
            GetToppingActivatedGlow().StartNewFireEffect("Yellow", Color.yellow, 2);
            PlayTriggeredSound();
        }
    }
}
