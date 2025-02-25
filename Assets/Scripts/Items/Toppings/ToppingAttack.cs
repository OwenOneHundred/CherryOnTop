using UnityEngine;

/**
 * An abstract class representing an attack by a Topping. Child classes should override the methods in
 * this class to implement a type of attack on a Cherry.
 */
public abstract class ToppingAttack : ScriptableObject
{
    // Represents the Topping object that the ToppingAttack instance is assigned to.
    public GameObject topping;

    // Represents the number of seconds the Topping should wait between attacks.
    public float cooldown;

    /** 
     * Specifies what the ToppingAttack should do as soon as it is assigned to a Topping. The topping it is assigned to
     * is passed as a parameter when called.
     */
    public abstract void OnStart();

    /** 
     * Specifies what the ToppingAttack should do when it first sees a Cherry enter its attack radius or switches targets.
     * The new target Cherry is passed as a parameter when called. 
     */
    public abstract void OnNewCherryFound(GameObject newTargetedCherry);

    /** 
     * Specifies what the ToppingAttack should do every n seconds, where n is the Topping's cooldown. The targeted Cherry'
     * is passed as a parameter when called.
     */
    public abstract void OnCycle(GameObject targetedCherry);

}
