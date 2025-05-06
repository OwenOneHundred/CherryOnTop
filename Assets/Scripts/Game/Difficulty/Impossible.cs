using UnityEngine;

[System.Serializable] [CreateAssetMenu(menuName = "Difficulty/Impossible")]
public class Impossible : Difficulty
{
    public override void OnGameStart()
    {
        Shop.shop.totalItems = 4;
        Shop.shop.columns = 2;
        Shop.shop.rows = 2;
        Inventory.inventory.initialMoney = 15;
        RoundManager.roundManager.moneyOnRoundEnd -= 1;
        GameObject.FindAnyObjectByType<CherrySpawner>().defaultCherriesPerRound += 1;
    }
}