using UnityEngine;

public class ChallengesTab : TabController
{
    [SerializeField] BatterSelect batterSelect;
    public override void OnSwitchedTo()
    {
        gameObject.SetActive(true);

        batterSelect.UpdateBatterCompletions();
    }

    public override void OnLevelChangedWhileActiveTab()
    {
        batterSelect.UpdateBatterCompletions();
    }

    public override void EvaluateIfShouldBeLocked(int levelIndex)
    {
        Debug.Log("Locked = " + !AchievementsTracker.Instance.HasCompletedLevel(levelIndex, 4, -1));
        Locked = !AchievementsTracker.Instance.HasCompletedLevel(levelIndex, 4, -1);
    }
}
