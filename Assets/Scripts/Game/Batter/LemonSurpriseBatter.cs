using UnityEngine;

[CreateAssetMenu(menuName = "Batter/LemonSurprise")]
public class LemonSurpriseBatter : Batter
{
    public override void OnGameStart()
    {
        Shop.shop.columns = 1;
        Shop.shop.rows = 1;
        Shop.shop.totalItems = 1;
        Shop.shop.LiveRerollPrice = 1;
    }
}
