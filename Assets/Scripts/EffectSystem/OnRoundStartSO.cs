using UnityEngine;
using EventBus;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Event/OnRoundStart")]
public class OnRoundStartSO : EventSO <RoundStartEvent>
{
}
