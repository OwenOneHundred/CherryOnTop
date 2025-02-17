using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class SwitchLevelPreview : MonoBehaviour
{
    [SerializeField] private bool forwards;
    public Button moveButton;
    public GameObject[] level;
    [SerializeField] private GameObject loadedLevel;
    public GameObject backButton;
    public GameObject forwardsButton;
    public GameObject sceneChangeButton;
    public GameObject debugObject;
    [SerializeField] private int levelIndex = 0;
    [SerializeField] private Transform canvasTransform;


    void Start()
    {
        moveButton.onClick.AddListener(OnForwardsButtonClick);
        InstantiateBaseBox();
        backButton.SetActive(false);
    }

    void OnForwardsButtonClick()
    {
        Destroy(loadedLevel);
        levelIndex = (levelIndex + 1);
        LoadLevelBox(500);
        SlideBox(loadedLevel);

        if (levelIndex == level.Length - 1) { gameObject.SetActive(false); }
        else { gameObject.SetActive(true); }
        if (levelIndex == 0) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
    }

    public void OnBackButtonClick()
    {
        Destroy(loadedLevel);
        levelIndex = (levelIndex - 1);
        LoadLevelBox(-500);
        SlideBox(loadedLevel);

        if (levelIndex == 0) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
        if (levelIndex == level.Length - 1) { forwardsButton.SetActive(false); }
        else { forwardsButton.SetActive(true); }

    }

    void LoadLevelBox(int xpos) {
        
        GameObject newLevel = Instantiate(level[levelIndex], new Vector3(xpos, 0, 0), Quaternion.identity, transform.parent.transform);
        loadedLevel = newLevel;
        newLevel.transform.localPosition = new Vector3(xpos, 0, 0); // Set local position relative to the parent

        //Instantiate(debugObject, Vector3.zero, Quaternion.identity, newLevel.transform.Find("Panel"));
        // Debugging: Log the hierarchy of the instantiated newLevel
        Debug.Log("NewLevel instantiated with children:");
        foreach (Transform child in newLevel.transform)
        {
            Debug.Log("Child: " + child.name);
        }
    }

    void InstantiateBaseBox()
    {
        GameObject baseBox = Instantiate(level[0], Vector3.zero, Quaternion.identity, canvasTransform);
        loadedLevel = baseBox;
        baseBox.transform.localPosition = Vector3.zero; // Set local position relative to the parent

        

    }
    void SlideBox(GameObject box)
    {
        //Todo: make this start a coroutine to slide the box in from the side
        box.transform.localPosition = new Vector3(0, 0, 0);

    }
    
}
