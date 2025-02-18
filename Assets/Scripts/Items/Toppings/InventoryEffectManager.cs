using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class InventoryEffectManager : MonoBehaviour
{
    List<InventoryEffect> inventoryEffects = new List<InventoryEffect>();
    public void AddInventoryEffect(InventoryEffect effect)
    {
        inventoryEffects.Add(effect);
    }
    public void RemoveInventoryEffect(InventoryEffect effect)
    {
        inventoryEffects.Remove(effect);
    }

    public int GetLimit<T>() where T : Limit 
    {
        List<T> limitEffects = inventoryEffects.OfType<T>().ToList();
        if (limitEffects.Count != 0)
        {
            return limitEffects.OrderByDescending(item => item.limit).Last().limit;
        }
        else return int.MaxValue;
    }
    
    public int ApplyLimit<T>(int value) where T : Limit 
    {
        List<T> limitEffects = inventoryEffects.OfType<T>().ToList();
        if (limitEffects.Count != 0)
        {
            int limit = limitEffects.OrderByDescending(item => item.limit).Last().limit;
            return value > limit ? limit : value;
        }
        else return value;
    }
}
