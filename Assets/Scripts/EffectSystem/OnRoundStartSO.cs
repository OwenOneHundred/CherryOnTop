using UnityEngine;
using EventBus;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Event/OnRoundStart")]
public class OnRoundStartSO : EventSO
{
    List<EventBinding<RoundStartEvent>> events = new();
    public override void RegisterEffect(EffectSO effectSO)
    {
        EventBinding<RoundStartEvent> roundStartBinding =
            new EventBinding<RoundStartEvent>((e) => effectSO.OnTriggered(e));
        EventBus<RoundStartEvent>.Register(roundStartBinding);
    }

    public override void DeregisterAllEffects()
    {
        foreach (EventBinding<RoundStartEvent> roundStartEvent in events)
        {
            EventBus<RoundStartEvent>.Deregister(roundStartEvent);
        }
    }
}
