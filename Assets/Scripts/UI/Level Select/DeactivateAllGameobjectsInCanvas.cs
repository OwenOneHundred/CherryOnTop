using UnityEngine;

public class DeactivateAllGameobjectsInCanvas : MonoBehaviour
{
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
