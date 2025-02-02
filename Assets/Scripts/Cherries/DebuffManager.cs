using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Has a list of debuffs and manages calling their EveryFrame() functions and removing them
/// when their timers are up.
/// </summary>
public class DebuffManager : MonoBehaviour
{
    List<CherryDebuff> debuffs = new List<CherryDebuff>();

    public void AddDebuff()
    {

    }

    public void RemoveDebuff(CherryDebuff debuff)
    {

    }

    public float GetMovementSpeedMultiplier()
    {
        return 1; // return the product of all debuff movementSpeedMultipliers
    }

    public float GetDamageMultiplier()
    {
        return 1; // return the product of all debuff damageMultipliers
    }
}
