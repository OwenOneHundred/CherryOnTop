using UnityEngine;

[System.Serializable] [CreateAssetMenu(menuName = "Difficulty/Medium")]
public class Medium : Difficulty
{
    public override void OnRoundStart()
    {
        Shop.shop.totalItems = 6;
        Inventory.inventory.initialMoney = 15;
    }
}
