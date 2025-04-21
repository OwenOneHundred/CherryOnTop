using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Has a list of debuffs and manages calling their EveryFrame() functions and removing them
/// when their timers are up.
/// </summary>
public class DebuffManager : MonoBehaviour
{
    List<CherryDebuff> debuffs = new List<CherryDebuff>();

    private void Update()
    {
        for (int i = 0; i < debuffs.Count; i++)
        {
            CherryDebuff currentDebuff = debuffs[i];
            currentDebuff.EveryFrame();

            currentDebuff.effectDuration -= Time.deltaTime;
            if (currentDebuff.effectDuration <= 0)
            {
                currentDebuff.RemoveSelf();
            }
        }
    }

    /// <summary>
    /// Adds a debuff to a cherry
    /// </summary>
    public void AddDebuff(CherryDebuff debuffTemplate, bool allowDuplicates = false)
    {
        if (!allowDuplicates && debuffs.Any(x => x.template == debuffTemplate)) { return; }

        CherryDebuff debuffCopy = CherryDebuff.CreateInstance(debuffTemplate);

        DebuffModifierManager.Instance.ApplyDebuffModifiersToDebuff(debuffCopy);
        
        debuffs.Add(debuffCopy);
        debuffCopy.cherry = gameObject;
        debuffCopy.OnAdded(gameObject);
    }

    public void AddDebuffs(List<CherryDebuff> cherryDebuffs)
    {
        foreach (CherryDebuff cherryDebuff in cherryDebuffs)
        {
            AddDebuff(cherryDebuff);
        }
    }

    public bool HasDebuffType(CherryDebuff.DebuffType debuffType)
    {
        return (debuffs.FirstOrDefault(x => x.debuffType == debuffType) != null);
    }

    /// <summary>
    /// Removes a debuff from a cherry
    /// </summary>
    public void RemoveDebuff(CherryDebuff debuff)
    {
        debuffs.Remove(debuff);
        debuff.OnRemoved(gameObject);
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
    /// Returns damage multiplier for an attacker.
    /// </summary>
    /// <param name="attacker">The attacking topping. Null if damage is not dealt by a topping (IE: debuff).</param>
    /// <returns></returns>
    public float GetDamageMultiplier(Topping attacker)
    {
        if (attacker == null) { return 1; }
        float damageMultiplier = 1.0f;
        for (int i = 0; i < debuffs.Count; i++)
        {
            if (debuffs[i].typesThatGetDamageMultiplier.HasAny(attacker.flags))
            {
                damageMultiplier *= debuffs[i].damageMultiplier;
            }
        }
        return damageMultiplier; // return the product of all debuff damageMultipliers
    }

    public void OnDamaged(float damage)
    {
        foreach (CherryDebuff debuff in debuffs)
        {
            debuff.OnCherryDamaged(damage);
        }
    }
}
