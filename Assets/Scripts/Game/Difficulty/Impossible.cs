using UnityEngine;

[System.Serializable] [CreateAssetMenu(menuName = "Difficulty/Impossible")]
public class Impossible : Difficulty
{
    public override void OnRoundStart()
    {
        Shop.shop.totalItems = 4;
        Shop.shop.columns = 2;
        Shop.shop.rows = 2;
        Inventory.inventory.initialMoney = 12;
        Shop.shop.Rerolls += 1;
        RoundManager.roundManager.moneyOnRoundEnd -= 1;
    }
}