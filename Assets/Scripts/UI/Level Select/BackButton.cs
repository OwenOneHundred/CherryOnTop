using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    public Button backButton;
    public Button forwardsButton;
    void Start()
    {
        backButton.onClick.AddListener(forwardsButton.GetComponent<SwitchLevelPreview>().OnBackButtonClick);
    }
}
