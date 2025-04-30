using UnityEngine;

[CreateAssetMenu(menuName = "Batter/Strawberry")]
public class StrawberryBatter : Batter
{
    public Topping strawberry;
    public override void OnGameStart()
    {
        RoundManager.roundManager.moneyOnRoundEnd = 0;
        Inventory.inventory.GetItemForFree(strawberry);
        Inventory.inventory.GetItemForFree(strawberry);
    }
}
