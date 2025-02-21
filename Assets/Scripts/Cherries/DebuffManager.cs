using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Has a list of debuffs and manages calling their EveryFrame() functions and removing them
/// when their timers are up.
/// </summary>
public class DebuffManager : MonoBehaviour
{
    List<CherryDebuff> debuffs = new List<CherryDebuff>();

    private void Start()
    {
        
    }

    private void Update()
    {
        for (int i = 0; i < debuffs.Count; i++)
        {
            debuffs[i].EveryFrame();
        }
    }

    /// <summary>
    /// Adds a debuff to a cherry
    /// </summary>
    public void AddDebuff(CherryDebuff debuff)
    {
        debuffs.Add(debuff);
        //debuff.OnAdded();
    }

    /// <summary>
    /// Removes a debuff from a cherry
    /// </summary>
    public void RemoveDebuff(CherryDebuff debuff)
    {
        debuffs.Remove(debuff);
        debuff.OnRemoved();
    }

    /// <summary>
    /// Returns the movement speed multiplier
    /// </summary>
    public float GetMovementSpeedMultiplier()
    {
        float movementSpeedMultiplier = 1.0f;
        for (int i = 0; i < debuffs.Count; i++)
        {
            movementSpeedMultiplier *= debuffs[i].movementSpeedMultiplier;
        }
        return movementSpeedMultiplier; // return the product of all debuff movementSpeedMultipliers
    }

    /// <summary>
    /// Returns the damage multiplier
    /// </summary>
    public float GetDamageMultiplier()
    {
        float damageMultiplier = 1.0f;
        for (int i = 0; i < debuffs.Count; i++)
        {
            damageMultiplier *= debuffs[i].damageMultiplier;
        }
        return damageMultiplier; // return the product of all debuff damageMultipliers
    }
}
