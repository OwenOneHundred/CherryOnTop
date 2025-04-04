using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/GetMoneyIfNoKillsThisRound")]
public class GetMoneyIfNoKillsThisRound : EffectSO
{
    [SerializeField] int money;
    public override void OnTriggered(IEvent eventObject)
    {
        if (toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping.killsThisRound <= 0)
        {
            Inventory.inventory.Money += money;
        }
    }
}
