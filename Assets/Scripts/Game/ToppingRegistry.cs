using System.Collections.Generic;
using System.Linq;
using GameSaves;
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

    public void DeregisterTopping(Item item)
    {
        ItemInfo itemInfo = placedToppings.FirstOrDefault(x => x.topping == item);
        if (itemInfo.obj != null)
        {
            placedToppings.Remove(itemInfo);
        }
    }

    public void DeregisterTopping(GameObject obj)
    {
        ItemInfo itemInfo = placedToppings.FirstOrDefault(x => x.obj == obj);
        if (itemInfo.obj != null)
        {
            placedToppings.Remove(itemInfo);
        }
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

    public void SaveAll(SaveData saveData)
    {
        foreach (ItemInfo itemInfo in placedToppings)
        {
            itemInfo.topping.SaveToppingData(saveData);
        }
        foreach (Item item in Inventory.inventory.ownedItems)
        {
            item.SaveToppingData(saveData);
        }
    }

    public void LoadAllToppingData(SaveData saveData)
    {
        foreach (ItemInfo itemInfo in placedToppings)
        {
            itemInfo.topping.LoadToppingData(saveData);
        }
        foreach (Item item in Inventory.inventory.ownedItems)
        {
            item.LoadToppingData(saveData);
        }
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
