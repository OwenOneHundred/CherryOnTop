using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Effects/MakeToppingFree")]
public class MakeToppingFree : EffectSO
{
    [SerializeField] string toppingName;

    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        List<Item> shopItems = GameObject.FindAnyObjectByType<Shop>().currentItems;
        foreach (Item item in shopItems)
        {
            if (item.name == toppingName)
            {
                ToppingActivatedGlow toppingGlow = GetToppingActivatedGlow();
                if (toppingGlow != null) {
                    toppingGlow.StartNewFireEffect("MakeToppingFree", Color.blue, 3);
                }
                item.price = 0;
            }
        }
    }
}
