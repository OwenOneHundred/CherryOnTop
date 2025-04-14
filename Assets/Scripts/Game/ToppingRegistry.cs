using System.Collections.Generic;
using System.Linq;
using GameSaves;
using UnityEngine;

public class ToppingRegistry : MonoBehaviour
{
    public static ToppingRegistry toppingRegistry;
    [SerializeField] List<ItemInfo> placedToppings = new();
    public List<Item> allItems = new();
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
        ItemInfo itemInfo = placedToppings.FirstOrDefault(x => x.topping.name == item.name);
        if (itemInfo.obj != null) { Debug.Log("Remove topping: " + itemInfo.obj); } else { Debug.LogWarning("Couldn't find " + item.name + " in toppingregistry."); }
        
        if (itemInfo.obj != null)
        {
            placedToppings.Remove(itemInfo);
        }
    }

    public void DeregisterTopping(GameObject obj)
    {
        ItemInfo itemInfo = placedToppings.FirstOrDefault(x => x.obj.name == obj.name);
        if (itemInfo.obj != null)
        {
            placedToppings.Remove(itemInfo);
        }
    }

    public List<ItemInfo> GetAllPlacedToppings()
    {
        placedToppings.RemoveAll(x => x.obj == null); // this line should be unnecessary but I have spent 4 hours trying
        // to figure out how things are null in this list and I'm just gonna manually remove them who cares
        
        return placedToppings;
    }

    public GameObject GetToppingObj(Topping topping)
    {
        return placedToppings.First(x => x.topping.name == topping.name).obj;
    }

    public List<ItemInfo> GetAllToppingsOfType(ToppingTypes.Flags flags)
    {
        return placedToppings.Where(x => x.topping.flags.HasAny(flags)).ToList();
    } 

    public void SaveAll(SaveData saveData)
    {
        foreach (ItemInfo itemInfo in GetAllPlacedToppings())
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
        foreach (ItemInfo itemInfo in GetAllPlacedToppings())
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
