using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Oven/ItemReward")]
public class ItemReward : RewardItem
{
    [SerializeField] float uncommonValueThreshold = 50;
    [SerializeField] float rareValueThreshold = 100;
    Item item;

    public override void OnClaim(float value)
    {
        ToppingTypes.Rarity rarity = GetRarityFromValue(value);
        List<Item> itemsMatchingRarity = Shop.shop.availableItems.Where(x => x.rarity == rarity).ToList();
        item = itemsMatchingRarity[Random.Range(0, itemsMatchingRarity.Count)];
        Inventory.inventory.AddItem(item);
    }


    private ToppingTypes.Rarity GetRarityFromValue(float totalValue)
    {
        ToppingTypes.Rarity rarity;
        if (totalValue > rareValueThreshold)
        {
            rarity = ToppingTypes.Rarity.Rare;
        }
        else if (totalValue > uncommonValueThreshold)
        {
            rarity = ToppingTypes.Rarity.Uncommon;
        }
        else
        {
            rarity = ToppingTypes.Rarity.Common;
        }

        return rarity;
    }
}
