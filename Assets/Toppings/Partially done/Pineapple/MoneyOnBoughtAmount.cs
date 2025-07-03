using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/MoneyOnBoughtAmount")]
public class MoneyOnBoughtAmount : EffectSO
{
    [SerializeField] int purchasesAmount = 2;
    [SerializeField] int moneyAmount = 2;
    public override void OnTriggered(IEvent eventObject)
    {
        if (Shop.shop.purchasesThisRound == purchasesAmount)
        {
            Inventory.inventory.Money += moneyAmount;
            GetTopping().moneyGained += moneyAmount;
            GetTopping().TriggersCount += 1;
        }
    }
}
    
