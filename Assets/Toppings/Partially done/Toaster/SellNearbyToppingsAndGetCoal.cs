using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/SellNearbyToppingsGetCoal")]
public class SellNearbyToppingsAndGetCoal : EffectSO
{
    [SerializeField] int coalForEach = 3;
    [SerializeField] Topping coal;
    public override void OnTriggered(IEvent eventObject)
    {
        Collider thisObjectCollider = toppingObj.transform.root.GetComponentInChildren<Collider>();
        int totalSellPrice = 0;
        TargetingSystem targetingSystem = toppingObj.transform.root.GetComponentInChildren<TargetingSystem>();
        Collider[] hits = Physics.OverlapSphere(toppingObj.transform.position, targetingSystem.GetRange());
        foreach (Collider collider in hits)
        {
            if (collider == thisObjectCollider) { continue; }
            if (collider.transform.root.TryGetComponent(out ToppingObjectScript toppingObjectScript))
            {
                totalSellPrice += toppingObjectScript.topping.SellPrice;
                Shop.shop.SellItemOffCake(toppingObjectScript.topping, toppingObjectScript.transform.root.gameObject);
                toppingObjectScript.topping.TriggersCount += 1;
            }
        }
        int totalCoal = totalSellPrice / coalForEach;
        for (int i = 0; i < totalCoal; i += 1)
        {
            Inventory.inventory.AddItem(coal);
        }
    }
}
