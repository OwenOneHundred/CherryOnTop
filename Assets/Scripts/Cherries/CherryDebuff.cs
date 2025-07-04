using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Abstract parent for debuffs. Children should override the functions below to perform their function.
/// </summary>
public abstract class CherryDebuff : ScriptableObject
{
    public float movementSpeedMultiplier = 1; // this is read by DebuffManager, so debuffs can slow cherries.
    public bool canResultInNegativeTimesNegativeSpeed = false;

    public ToppingTypes.Flags typesThatGetDamageMultiplier;
    public float damageMultiplier = 1; // this is read by DebuffManager, so debuffs can put multipliers on damage.

    public DebuffType debuffType = DebuffType.none;

    public float dps = 0;
    
    [System.NonSerialized] public GameObject cherry; // should be set in OnAdd, 
    // so it can be read in EveryFrame to perform actions on the cherry this debuff is on

    [System.NonSerialized] public CherryDebuff template;
    public float effectDuration = 1;
    public AudioFile onAppliedSFX;

    /// <summary>
    /// Called every frame. This is where effects would deal damage and operate logic.
    /// </summary>
    public abstract void EveryFrame();

    /// <summary>
    /// Called when debuff is applied to a cherry with the cherry gameObject debuff is applied to.
    /// Expected to put VFX on the cherry and set cherry field to the argument cherry gameObject.
    /// </summary>
    /// <param name="cherry">The gameObject this debuff is now applied to</param>
    public abstract void OnAdded(GameObject cherry);

    /// <summary>
    /// Called when debuff is removed. Expected to remove VFX from cherry.
    /// Should not error if cherry gameObject is null when this call happens.
    /// </summary>
    public abstract void OnRemoved(GameObject cherry);

    public virtual void OnCherryDamaged(float damage) { }

    public void RemoveSelf()
    {
        if (cherry == null)
        {
            Debug.LogWarning("Debuff " + name + " tried to remove itself before its cherry was set.");
            return;
        }

        cherry.GetComponentInChildren<DebuffManager>().RemoveDebuffSpecifically(this);
    }

    public static CherryDebuff CreateInstance(CherryDebuff template)
    {
        CherryDebuff instance = Instantiate(template);

        instance.template = template;

        return instance;
    }

    
    [System.Flags] public enum DebuffType
    {
        none = 0,
        fire = 1,
        freeze = 2,
        poison = 4,
        fondue = 8
    }
}
