using UnityEngine;
using EventBus;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Event/OnBuyItem")]
public class OnBuyItem : EventSO
{
    List<EventBinding<BuyEvent>> events = new();
    public override void RegisterEffect(EffectSO effectSO)
    {
        EventBinding<BuyEvent> buyBinding = new EventBinding<BuyEvent>(effectSO.OnTriggered);
        EventBus<BuyEvent>.Register(buyBinding);
    }

    public override void DeregisterAllEffects()
    {
        foreach (EventBinding<BuyEvent> buyEvent in events)
        {
            EventBus<BuyEvent>.Deregister(buyEvent);
        }
    }
}
