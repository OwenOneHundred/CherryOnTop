using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Holds all the information for this topping, including:
/// Price, what GameObject to spawn, and the toppingEffects associated
/// This scriptableObject will be instantiated and assigned to the shop representation of toppings and then passed to
/// the toppings' ingame version once they are placed, so this can hold data and handle events in menus.
/// Also holds the type of this topping in an enum flags
/// </summary>
[CreateAssetMenu(menuName = "Items/Topping")]
public class Topping : Item
{
    public GameObject towerPrefab;
    public ToppingTypes.Flags flags;

    public void SetGameObjectOnEffects(GameObject prefab)
    {
        for (int i = 0; i < effectsAndWhen.Count; i++)
        {
            for (int j = 0; j < effectsAndWhen[i].effectSOs.Count; j++)
            {   
                effectsAndWhen[i].effectSOs[j].toppingObj = prefab;
            }
        }
    }
}
