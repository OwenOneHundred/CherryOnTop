using UnityEngine;

/// <summary>
/// Abstract type of ToppingAttack which deals damage to a Cherry directly without the use of an intermediate object
/// such as a projectile or shockwave.
/// </summary>
public abstract class DirectAttack : ToppingAttack
{
    /// <summary>
    /// Deal damage to the targeted Cherry directly.
    /// </summary>
    /// <param name="targetedCherry"></param>
    public abstract void DealDamage(GameObject targetedCherry);

}
