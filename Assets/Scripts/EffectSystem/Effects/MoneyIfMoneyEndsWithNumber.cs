using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="Effects/MoneyIfMoneyEndsWith")]
public class MoneyIfMoneyEndsWithNumber : EffectSO
{
    [SerializeField] char endingNumber;
    [SerializeField] int moneyChange = 4;
    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        if (Inventory.inventory.Money.ToString().Last() == endingNumber)
        {
            Inventory.inventory.Money += moneyChange;
            ToppingActivatedGlow toppingGlow = GetToppingActivatedGlow();
            if (toppingGlow != null) {
                toppingGlow.StartNewFireEffect("EndingNum $" + moneyChange, Color.red, 3);
            }
        }
    }
}
