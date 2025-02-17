using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Holds all the information for this topping, including:
/// Price, what GameObject to spawn, and the toppingEffects associated
/// This scriptableObject will be instantiated and assigned to the shop representation of toppings and then passed to
/// the toppings' ingame version once they are placed, so this can hold data and handle events in menus.
/// Also holds the type of this topping in an enum (that is not implemented)
/// </summary>
[CreateAssetMenu(menuName ="Toppings/Topping")]
public class Topping : Item
{
    public GameObject towerPrefab;
    public ToppingTypes.Flags flags;
}
