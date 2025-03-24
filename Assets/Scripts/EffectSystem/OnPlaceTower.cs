using UnityEngine;
using System.Collections.Generic;
using EventBus;

[CreateAssetMenu(menuName = "Event/OnTowerPlaced")]
public class OnPlaceTower : EventSO <TowerPlacedEvent>
{
}
