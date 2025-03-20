using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    public Button backButton;
    public Button forwardsButton;
    public HoverImgChange hoverImgChange;
    void Start()
    {
        hoverImgChange = GetComponent<HoverImgChange>();
        backButton.onClick.AddListener(forwardsButton.GetComponent<SwitchLevelPreview>().OnBackButtonClick);
        
    }
}
