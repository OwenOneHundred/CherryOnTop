using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName ="Toppings/ToppingEffect")]
public class ToppingEffect : ScriptableObject
{
    [Header("When")]
    public List<When> when;

    [Header("For")]
    public ToppingForSet forSet;

    [Header("If")]
    public ToppingIf ifObject;

    [Header("Perform")]
    public Perform perform;

    public void Init()
    {
        foreach (When w in when)
        {
            switch (w)
            {
                case When.cherryDies: WhenEvents.cherryDies += OnTriggered; break;
                case When.cherryDamaged: WhenEvents.cherryDamaged += OnTriggered; break;
                case When.onBuyAnyTopping: WhenEvents.onBuyAnyTopping += OnTriggered; break;
                case When.onBuyThisTopping: WhenEvents.onBuyThisTopping += OnTriggered; break;
                case When.onReroll: WhenEvents.onReroll += OnTriggered; break;
                case When.onSellAnyTopping: WhenEvents.onSellAnyTopping += OnTriggered; break;
                case When.onSellThisTopping: WhenEvents.onSellThisTopping += OnTriggered; break;
                case When.roundEnds: WhenEvents.roundEnds += OnTriggered; break;
                case When.roundStarts: WhenEvents.roundStarts += OnTriggered; break;
                default: break;
            }
        }
    }

    public void OnTriggered()
    {
        if (ifObject == null || ifObject.Evaluate())
        {
            perform.Execute(forSet.GetSet());
        }
    }

    [System.Serializable]
    public enum HowManyTimes
    {
        once, forEach
    }

    [System.Serializable]
    public enum When
    {
        none,
        cherryDies,
        cherryDamaged,
        roundEnds,
        roundStarts,
        onBuyAnyTopping,
        onSellAnyTopping,
        onBuyThisTopping,
        onSellThisTopping,
        onReroll
    }
}
