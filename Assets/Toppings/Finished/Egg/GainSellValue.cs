using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/GainSellValue")]
public class GainSellValue : EffectSO
{
    public int sellPriceChange = 2;
    public override void OnTriggered(IEvent eventObject)
    {
        GetTopping().SellPrice += sellPriceChange;
        GetToppingActivatedGlow().StartNewFireEffect("Blue", Color.blue, 2);
    }
}
