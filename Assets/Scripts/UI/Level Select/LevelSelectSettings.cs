using UnityEngine;

public class LevelSelectSettings : MonoBehaviour
{
    public DeactivateAllGameobjectsInCanvas deactiveScript;
    int activationCount = 0;

    private void OnEnable()
    {
        if (activationCount > 0) { 
            deactiveScript.Deactivate(false);
            Debug.Log("Onenable called");
        }
        activationCount++;
    }
}
