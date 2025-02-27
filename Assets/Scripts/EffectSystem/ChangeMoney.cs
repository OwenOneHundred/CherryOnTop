using UnityEngine;

[CreateAssetMenu(menuName ="Effects/ChangeMoney")]
public class ChangeMoney : EffectSO
{
    [SerializeField] int amountToChangeMoney = 5;

    public override void OnTriggered()
    {
        GameInfo.money += amountToChangeMoney;
    }
}