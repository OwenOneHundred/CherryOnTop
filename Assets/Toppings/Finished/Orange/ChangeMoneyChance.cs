using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/ChangeMoneyChance")]
public class ChangeMoneyChance : EffectSO
{
    [SerializeField] int amountToChangeMoney = 1;
    [SerializeField] float chanceOfHappening0to1 = 1;
    [SerializeField] GameObject effectToSpawn;
    [SerializeField] bool spawnOnCamera = false;
    public override void OnTriggered(IEvent eventObject)
    {
        if (Random.value <= chanceOfHappening0to1)
        {
            GetTopping().moneyGained += amountToChangeMoney;
            Inventory.inventory.Money += amountToChangeMoney;
            GetToppingActivatedGlow().StartNewFireEffect("Orange", new Color(1, 0.57f, 0.2f), 2);
            if (effectToSpawn != null)
            {
                if (spawnOnCamera)
                {
                    Destroy(Instantiate(effectToSpawn, Camera.main.transform), 7);
                }
                else
                {
                    Destroy(Instantiate(effectToSpawn, toppingObj.transform.position, Quaternion.identity, toppingObj.transform), 7);
                }
            }
        }
    }
}
