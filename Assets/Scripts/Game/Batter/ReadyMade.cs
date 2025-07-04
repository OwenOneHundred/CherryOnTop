using UnityEngine;

[CreateAssetMenu(menuName = "Batter/ReadyMade")]
public class ReadyMade : Batter
{
    public override void OnGameStart()
    {
        IngameUI ingameUI = GameObject.FindAnyObjectByType<IngameUI>();

        ingameUI.SetLockValue(true, true);
        ingameUI.PlayLockDisabled = true;

        ingameUI.SetSpeedUp(true, true);
        ingameUI.SpeedUpButtonLocked = true;
    }
}
