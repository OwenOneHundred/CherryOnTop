using UnityEngine;

public class LevelSelectSettings : MonoBehaviour
{
    int activationCount = 0;

    private void OnEnable()
    {
        if (activationCount > 0) { 
            Deactivate(false);
            Debug.Log("Onenable called");
        }
        activationCount++;
    }

    public void Deactivate(bool deactivate)
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (deactivate)
        {
            foreach (Transform child in canvas.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform child in canvas.transform)
            {
                child.gameObject.SetActive(true);
            }

        }

    }
}
