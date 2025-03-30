using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/AddToppingToInventory")]
public class AddToppingToInventory : EffectSO
{
    [SerializeField] Item item;
    [SerializeField] float odds0to1 = 1;
    public override void OnTriggered(IEvent eventObject)
    {
        if (odds0to1 == 0) { return; }
        if (Random.value <= odds0to1)
        {
            Inventory.inventory.GetItemForFree(item);
        }
    }
}
