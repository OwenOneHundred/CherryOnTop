using System.Collections.Generic;
using UnityEngine;

public class DebuffModifierManager : MonoBehaviour
{
    public static DebuffModifierManager Instance;
    void Awake()
    {
        if (Instance == null || Instance == this) { Instance = this; }
        else { Destroy(gameObject); return; }
    }

    private List<DebuffModifier> debuffModifiers = new();

    public void ApplyDebuffModifiersToDebuff(CherryDebuff cherryDebuff)
    {
        foreach (DebuffModifier debuffModifier in debuffModifiers)
        {
            debuffModifier.ApplyModifiersToDebuff(cherryDebuff);
        }
    }

    public void AddDebuffModifier(DebuffModifier modifier) 
    {
        debuffModifiers.Add(modifier);
    }

    public bool RemoveDebuffModifier(DebuffModifier modifier)
    {
        return debuffModifiers.Remove(modifier);
    }
}
