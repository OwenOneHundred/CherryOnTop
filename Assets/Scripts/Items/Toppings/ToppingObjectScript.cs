using System.Collections.Generic;
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
}
