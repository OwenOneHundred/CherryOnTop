using UnityEngine;

[CreateAssetMenu(menuName ="Effects/ChangeMoney")]
public class ChangeMoney : EffectSO
{
    [SerializeField] int amountToChangeMoney = 5;

    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        Inventory.inventory.Money += amountToChangeMoney;
        GetToppingActivatedGlow().StartNewFireEffect("ChangeMoney", Color.yellow, 3);
    }
}