using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/MushroomEffect")]
public class MushroomEffect : EffectSO
{
    [SerializeField] float chance;
    public override void OnTriggered(IEvent eventObject)
    {
        if (Random.value <= chance)
        {
            Inventory.inventory.Money += 2;
            Topping thisTopping = toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping;
            EventBus<SellEvent>.Raise(new SellEvent(thisTopping, toppingObj));
            Debug.Log("Calling event for topping: " + toppingObj.name);
            Destroy(toppingObj);
            Inventory.inventory.GetItemForFree(thisTopping);
        }
    }
}
