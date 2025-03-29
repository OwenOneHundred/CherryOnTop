using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToppingRegistry : MonoBehaviour
{
    public static ToppingRegistry toppingRegistry;
    [SerializeField] List<ItemInfo> placedToppings = new();
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

    public GameObject GetToppingObj(Topping topping)
    {
        return placedToppings.First(x => x.topping == topping).obj;
    }

    public List<ItemInfo> GetAllToppingsOfType(ToppingTypes.Flags flags)
    {
        return placedToppings.Where(x => x.topping.flags.HasAny(flags)).ToList();
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
