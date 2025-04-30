using UnityEngine;

[CreateAssetMenu(menuName = "Batter/CheeseCake")]
public class CheeseCakeBatter : Batter
{
    public override void OnGameStart()
    {
        Inventory.inventory.GetComponent<InventoryEffectManager>().AddInventoryEffect(new LimitBuying(1));
        RoundManager.roundManager.moneyOnRoundEnd += 2;
    }
}
