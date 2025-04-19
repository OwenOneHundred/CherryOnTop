using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An abstract class representing an attack by a Topping. Child classes should override the methods in
/// this class to implement a type of attack on a Cherry.
/// </summary>
public abstract class ToppingAttack : ScriptableObject
{
    // A reference to the Topping object that the ToppingAttack instance is assigned to.
    [System.NonSerialized]
    public GameObject toppingObj;

    // Represents the number of seconds the Topping should wait between attacks.
    public float cooldown;

    // Represents the amount of damage each attack should do to affected Cherries.
    public int damage;

    // The list of debuffs this ToppingAttack might inflict when it attacks a Cherry.
    public List<CherryDebuff> debuffs;

    /// <summary>
    /// Specifies what the ToppingAttack should do as soon as it is assigned to a Topping. The topping it is assigned to
    /// is passed as a parameter when called.
    /// </summary>
    public abstract void OnStart();

    /// <summary> 
    /// Specifies what the ToppingAttack should do when it first sees a Cherry enter its attack radius or switches targets.
    /// The new target Cherry is passed as a parameter when called. 
    /// </summary>
    public abstract void OnNewCherryFound(GameObject newTargetedCherry);

    /// <summary> 
    /// Specifies what the ToppingAttack should do every n seconds, where n is the Topping's cooldown. The targeted Cherry
    /// is passed as a parameter when called.
    /// </summary>
    public abstract void OnCycle(GameObject targetedCherry);

    public virtual void EveryFrame()
    {

    }

    public virtual int GetVisibleDamage()
    {
        return damage;
    }

}
