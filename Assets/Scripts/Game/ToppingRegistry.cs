using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToppingRegistry : MonoBehaviour
{
    public static ToppingRegistry toppingRegistry;
    [SerializeField] List<ItemInfo> placedToppings;
    private void Awake()
    {
        if (toppingRegistry == null)
        {
            toppingRegistry = this;
        }
        else if (toppingRegistry != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void RegisterPlacedTopping(Topping topping, GameObject gameObject)
    {
        placedToppings.Add(new ItemInfo(topping, gameObject));
    }   

    public List<ItemInfo> GetAllPlacedToppings()
    {
        return placedToppings;
    }

    public struct ItemInfo
    {
        public Topping topping;
        public GameObject obj;

        public ItemInfo(Topping topping, GameObject obj)
        {
            this.topping = topping;
            this.obj = obj;
        }
    }
}
