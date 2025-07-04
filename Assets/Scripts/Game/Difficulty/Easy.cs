using UnityEngine;

[System.Serializable] [CreateAssetMenu(menuName = "Difficulty/Easy")]
public class Easy : Difficulty
{
    public override void OnGameStart()
    {
        RoundManager.roundManager.moneyOnRoundEnd += 1;
        Inventory.inventory.initialMoney = 20;
    }
}
