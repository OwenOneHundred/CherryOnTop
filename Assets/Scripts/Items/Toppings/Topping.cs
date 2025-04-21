using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using GameSaves;


/// <summary>
/// Holds all the information for this topping, including:
/// Price, what GameObject to spawn, and the toppingEffects associated
/// This scriptableObject will be instantiated and assigned to the shop representation of toppings and then passed to
/// the toppings' ingame version once they are placed, so this can hold data and handle events in menus.
/// Also holds the type of this topping in an enum flags
/// </summary>
[CreateAssetMenu(menuName = "Items/Topping")]
public class Topping : Item
{
    public GameObject towerPrefab;
    public ToppingTypes.Flags flags;

    public void SetGameObjectOnEffects(GameObject obj)
    {
        for (int i = 0; i < effectsAndWhen.Count; i++)
        {
            for (int j = 0; j < effectsAndWhen[i].effectSOs.Count; j++)
            {   
                effectsAndWhen[i].effectSOs[j].toppingObj = obj;
            }
        }

        for (int i = 0; i < onHitCherry.Count; i++)
        {
            onHitCherry[i].toppingObj = obj;
        }

        for (int i = 0; i < onKillCherry.Count; i++)
        {   
            onKillCherry[i].toppingObj = obj;
        }
    }
    public int cakePoints = 10;

    [System.NonSerialized] public int killsThisRound = 0;
    [System.NonSerialized] public int damagedCherriesThisRound = 0;
    [SerializeField] List<EffectSO> onHitCherry = new List<EffectSO>();
    [SerializeField] List<EffectSO> onKillCherry = new List<EffectSO>();

    public void OnHitCherry(CherryHitbox cherry)
    {
        damagedCherriesThisRound += 1;
        foreach (EffectSO effectSO in onHitCherry)
        {
            effectSO.OnTriggered(null);
        }
    }

    public void OnKillCherry(CherryHitbox cherry)
    {
        killsThisRound += 1;
        foreach (EffectSO effectSO in onKillCherry)
        {
            effectSO.OnTriggered(null);
        }
    }

    public void OnRoundEnd()
    {
        killsThisRound = 0;
        damagedCherriesThisRound = 0;
    }
}
