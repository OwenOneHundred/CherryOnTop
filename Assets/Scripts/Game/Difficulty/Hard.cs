using UnityEngine;

[System.Serializable] [CreateAssetMenu(menuName = "Difficulty/Hard")]
public class Hard : Difficulty
{
    public override void OnRoundStart()
    {
        Shop.shop.totalItems = 5;
        Shop.shop.columns = 2;
        Shop.shop.rows = 2;
        Inventory.inventory.initialMoney = 14;
        Shop.shop.Rerolls += 1;
    }
}
