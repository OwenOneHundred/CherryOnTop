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

    // Note to Tony from Owen: C# supports asterisk comment blocks, but it actually has a built in documentation system.
    // Press / three times to make a summary block, and text entered there is viewable by hovering that keyword. This is standard.
    // I've switched your documentation on the function below to that format.

    /// <summary>
    /// Specifies what the ToppingAttack should do as soon as it is assigned to a Topping. The topping it is assigned to
    /// is passed as a parameter when called.
    /// </summary>
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
