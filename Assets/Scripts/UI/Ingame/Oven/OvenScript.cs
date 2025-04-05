using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OvenScript : MonoBehaviour
{
    [SerializeField] List<RewardItem> rewardItems;

    public void GetOutput(int money, List<Item> items)
    {
        int totalValue = items.Sum(x => x.price * GetRarityValue(x.rarity));
        totalValue += money * 2;

        float adjustedValue = AddRandomnessToTotalValue(totalValue);

        RewardItem rewardItem = rewardItems[GeneralUtil.RandomWeighted(rewardItems.Select(x => x.weight).ToList())];
        rewardItem.OnClaim(adjustedValue);
    }

    private float AddRandomnessToTotalValue(float value)
    {
        float lowValuePenalty = Mathf.Clamp01((value / 20) + 0.3f);
        return value + (Mathf.Abs(GeneralUtil.RandomAccordingToNormalDistribution(0, 35)) * lowValuePenalty);
    }

    public int GetRarityValue(ToppingTypes.Rarity rarity)
    {
        return rarity switch
        {
            ToppingTypes.Rarity.Common => 1,
            ToppingTypes.Rarity.Uncommon => 2,
            ToppingTypes.Rarity.Rare => 4,
            _ => 1
        };
    }
}
