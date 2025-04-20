using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/RerollForEachToppingOfType")]
public class GetRerollForEachToppingOfType : EffectSO
{
    [SerializeField] float chanceOfHappening0to1 = 1;
    [SerializeField] ToppingTypes.Flags type;
    public override void OnTriggered(IEvent eventObject)
    {
        int produceCount = ToppingRegistry.toppingRegistry.GetAllToppingsOfType(type).Count;
        for (int i = 0; i < produceCount; i++)
        {
            if (Random.value <= chanceOfHappening0to1)
            {
                GetToppingActivatedGlow().StartNewFireEffect("Green", Color.green, 2);
                Shop.shop.Rerolls += 1;
                PlayTriggeredSound();
            }
        }

    }
}
