using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "effectExample/gameeffect")]
public class GameEffectExample : ScriptableObject
{
    [SerializeField] TowerEffectExample effect;
    protected TowerExample tower;

    public void RegisterEffects(TowerExample tower)
    {
        this.tower = tower;
        EventBinding<RoundStartEvent> roundStartBinding = new EventBinding<RoundStartEvent>(HandleRoundStart);
        EventBus<RoundStartEvent>.Register(roundStartBinding);
    }

    public void HandleRoundStart(RoundStartEvent round)
    {
        if (tower != null)
        {
            Debug.Log("Handling tower input on round start");
            effect.HandleTowerInput(tower);
        }
    }
}
