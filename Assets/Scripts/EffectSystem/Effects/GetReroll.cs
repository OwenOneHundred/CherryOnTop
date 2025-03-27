using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName ="Effects/GetReroll")]
public class GetReroll : EffectSO
{
    [SerializeField] int rerolls = 1;
    [SerializeField] float chanceOfHappening0to1 = 1;
    public override void OnTriggered(IEvent eventObject)
    {
        if (Random.value <= chanceOfHappening0to1)
        {
            Shop.shop.Rerolls += rerolls;
        }
    }
}
