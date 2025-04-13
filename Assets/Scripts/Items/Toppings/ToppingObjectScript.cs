using System.Collections.Generic;
using GameSaves;
using UnityEngine;

public class ToppingObjectScript : MonoBehaviour
{
    [SerializeField] List<EffectSO> onPlaced = new List<EffectSO>();
    [SerializeField] List<EffectSO> onSell = new List<EffectSO>();
    [System.NonSerialized] public Topping topping;
    void Start()
    {
        foreach (EffectSO effectSO in onPlaced)
        {
            effectSO.OnTriggered(null);
        }
    }

    void OnDestroy()
    {
        foreach (EffectSO effectSO in onSell)
        {
            effectSO.OnTriggered(null);
        }
    }

    public void SaveOnAllEffects(SaveData saveData)
    {
        topping.SaveToppingData(saveData);

        foreach (EffectSO effectSO in onSell)
        {
            effectSO.Save(saveData);
        }
        foreach (EffectSO effectSO in onPlaced)
        {
            effectSO.Save(saveData);
        }
    }

    public void LoadOnAllEffects(SaveData saveData)
    {
        topping.LoadToppingData(saveData);

        foreach (EffectSO effectSO in onSell)
        {
            effectSO.Load(saveData);
        }
        foreach (EffectSO effectSO in onSell)
        {
            effectSO.Load(saveData);
        }
    }
}
