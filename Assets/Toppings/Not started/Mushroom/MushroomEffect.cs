using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/MushroomEffect")]
public class MushroomEffect : EffectSO
{
    [SerializeField] float chance;
    public override void OnTriggered(IEvent eventObject)
    {
        Debug.Log("Called OnTriggered on topping at position:" + toppingObj.transform.position);
        if (Random.value <= chance)
        {
            Inventory.inventory.Money += 2;
            Topping thisTopping = toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping;
            EventBus<SellEvent>.Raise(new SellEvent(thisTopping, toppingObj));
            Destroy(toppingObj);
            Inventory.inventory.GetItemForFree(thisTopping);
        }
    }
}
