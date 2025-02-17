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

        GameObject newLevel = Instantiate(level[levelIndex], new Vector3(xpos, 0, 0), Quaternion.identity, canvasTransform);
        loadedLevel = newLevel;
        newLevel.transform.localPosition = new Vector3(xpos, 0, 0); // Set local position relative to the parent

        // Debugging: Log the hierarchy of the instantiated newLevel
        Debug.Log("NewLevel instantiated with children:");
        foreach (Transform child in newLevel.transform)
        {
            Debug.Log("Child: " + child.name);
        }

        // Find the "Panel" child within the newLevel
        Transform panelTransform = newLevel.transform.Find("Panel");
        if (panelTransform != null)
        {
            // Instantiate additional buttons as children of the "Panel"
            Instantiate(sceneChangeButton, Vector3.zero, Quaternion.identity, panelTransform);
            Instantiate(debugObject, Vector3.zero, Quaternion.identity, panelTransform);

            // Debugging: Log the hierarchy after adding buttons
            Debug.Log("After adding buttons to Panel:");
            foreach (Transform child in panelTransform)
            {
                Debug.Log("Child: " + child.name);
            }
        }
        else
        {
            Debug.LogError("Panel not found in the newLevel prefab.");
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

    void DisableAllButtons()
    {
        backButton.SetActive(false);
        forwardsButton.SetActive(false);
    }

    void EnableAllButtons()
    {
        backButton.SetActive(true);
        forwardsButton.SetActive(true);
    }

}
