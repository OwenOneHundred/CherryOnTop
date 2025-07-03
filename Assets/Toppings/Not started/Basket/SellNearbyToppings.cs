using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/SellNearbyToppings")]
public class SellNearbyToppings : EffectSO
{
    [Tooltip("Only sells topping of the specified type. If none, sells every type.")]
    [SerializeField] ToppingTypes.Flags onlySellTypes;
    public override void OnTriggered(IEvent eventObject)
    {
        Collider thisObjectCollider = toppingObj.transform.root.GetComponentInChildren<Collider>();
        Topping thisTopping = GetTopping();
        int totalSellPrice = 0;
        TargetingSystem targetingSystem = toppingObj.transform.root.GetComponentInChildren<TargetingSystem>();
        Collider[] hits = Physics.OverlapSphere(toppingObj.transform.position, targetingSystem.GetRange());
        foreach (Collider collider in hits)
        {
            if (collider == thisObjectCollider) { continue; }
            if (collider.transform.root.TryGetComponent(out ToppingObjectScript toppingObjectScript))
            {
                if (onlySellTypes != ToppingTypes.Flags.none && !onlySellTypes.HasAny(toppingObjectScript.topping.flags)) { continue; }
                totalSellPrice += toppingObjectScript.topping.SellPrice;
                Shop.shop.SellItemOffCake(toppingObjectScript.topping, toppingObjectScript.transform.root.gameObject);
                thisTopping.TriggersCount += 1;
            }
        }
    }
}
