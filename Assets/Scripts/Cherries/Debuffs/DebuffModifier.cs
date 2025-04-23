using UnityEngine;

[CreateAssetMenu(menuName ="DebuffModifier")]
public class DebuffModifier : ScriptableObject
{
    public CherryDebuff.DebuffType modifiedType = CherryDebuff.DebuffType.none;
    public float movementSpeedMultiplier = 1;
    public float movementSpeedChange = 0;
    public float DPSmulitplier = 1;
    public float durationMultiplier = 1;
    public float damageMultiplier = 1;

    public void ApplyModifiersToDebuff(CherryDebuff debuff)
    {
        if (!HasAny(debuff.debuffType, modifiedType)) { return; }
        debuff.movementSpeedMultiplier *= movementSpeedMultiplier;
        debuff.movementSpeedMultiplier += movementSpeedChange;
        debuff.effectDuration *= durationMultiplier;
        debuff.damageMultiplier *= damageMultiplier;
        debuff.dps *= DPSmulitplier;
    }

    static bool HasAny(CherryDebuff.DebuffType value, CherryDebuff.DebuffType any)
        => (value & any) != 0;
}
