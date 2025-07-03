using System.Collections.Generic;
using System.Linq;
using GameSaves;
using UnityEngine;

public class ToppingRegistry : MonoBehaviour
{
    public static ToppingRegistry toppingRegistry;
    private List<ItemInfo> placedToppings = new();
    public List<ItemInfo> PlacedToppings
    {
        get { 
            placedToppings.RemoveAll(x => x.obj == null); // this line should be unnecessary but I have spent 4 hours trying
            // to figure out how things are null in this list and I'm just gonna manually remove them who cares
        
            return placedToppings;
        }
    }
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
        Item.HighestSellPrice = -1;
    }

    public void RegisterPlacedTopping(Topping topping, GameObject gameObject)
    {
        PlacedToppings.Add(new ItemInfo(topping, gameObject));
    }   

    public void DeregisterTopping(Item item)
    {
        ItemInfo itemInfo = PlacedToppings.FirstOrDefault(x => x.topping.name == item.name);
        if (itemInfo.obj != null) { Debug.Log("Remove topping: " + itemInfo.obj); } else { Debug.LogWarning("Couldn't find " + item.name + " in toppingregistry."); }
        
        if (itemInfo.obj != null)
        {
            PlacedToppings.Remove(itemInfo);
        }
    }

    public void DeregisterTopping(GameObject obj)
    {
        ItemInfo itemInfo = PlacedToppings.FirstOrDefault(x => x.obj.name == obj.name);
        if (itemInfo.obj != null)
        {
            PlacedToppings.Remove(itemInfo);
        }
    }

    public GameObject GetToppingObj(Topping topping)
    {
        return PlacedToppings.First(x => x.topping.name == topping.name).obj;
    }

    public List<ItemInfo> GetAllToppingsOfType(ToppingTypes.Flags flags)
    {
        return PlacedToppings.Where(x => x.topping.flags.HasAny(flags)).ToList();
    } 

    public void SaveAll(SaveData saveData)
    {
        foreach (ItemInfo itemInfo in PlacedToppings)
        {
            itemInfo.topping.SaveToppingData(saveData);
        }
        // Removed because:
        // A) Items in inventory do not store save data
        // B) Items saving in inventory attempt to access toppings which are null, causing crash
        /*foreach (Item item in Inventory.inventory.ownedItems)
        {
            item.SaveToppingData(saveData);
        }*/
    }

    public void LoadAllToppingData(SaveData saveData)
    {
        foreach (ItemInfo itemInfo in PlacedToppings)
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
