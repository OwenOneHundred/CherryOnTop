using UnityEngine;

[CreateAssetMenu(menuName = "Oven/MoneyReward")]
public class MoneyReward : RewardItem
{
    public override void OnClaim(float value)
    {
        Inventory.inventory.Money += Mathf.FloorToInt(value / 2);
    }
}
