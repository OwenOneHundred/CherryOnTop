using System.Linq;
using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/GetCommonOnBoughtUncommon")]
public class GetCommonOnBoughtUncommon : EffectSO
{
    public override void OnTriggered(IEvent eventObject)
    {
        if (eventObject is BuyEvent buyEvent && buyEvent.item != null && buyEvent.item.rarity == ToppingTypes.Rarity.Uncommon)
        {
            var allCommons = Shop.shop.availableItems.Where(x => x.rarity == ToppingTypes.Rarity.Common).ToList();
            Item randomCommon = allCommons[Random.Range(0, allCommons.Count)];
            Inventory.inventory.AddItem(randomCommon);
            GetToppingActivatedGlow().StartNewFireEffect("Fire", Color.yellow, 2);
        }
    }
}
