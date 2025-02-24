using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "effectExample/towerEffect")]
public class TowerEffectExample : ScriptableObject
{
    [SerializeField] protected int attackPowerIncrease = 2;

    public void HandleTowerInput(TowerExample tower)
    {
        tower.attackPower += attackPowerIncrease;
    }
}
