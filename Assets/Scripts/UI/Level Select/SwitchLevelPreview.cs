using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SwitchLevelPreview : MonoBehaviour
{
    [SerializeField] private bool forwards;
    public UnityEngine.UI.Button moveButton;
    public GameObject[] level;
    [SerializeField] private GameObject loadedLevel;
    public GameObject backButton;
    public GameObject forwardsButton;
    [SerializeField] private int levelIndex = 0;
    [SerializeField] private Transform canvasTransform;
    public int boxLoadDistance;
    public float slideTime;
    private bool moving = false;


    void Start()
    {
        moveButton.onClick.AddListener(OnForwardsButtonClick);
        InstantiateBaseBox();
        backButton.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(moving)
        {
            loadedLevel.transform.localPosition = loadedLevel.transform.localPosition - new Vector3(boxLoadDistance / (60 / slideTime), 0, 0);
            Debug.Log("sliding");
        }


    }

    void OnForwardsButtonClick()
    {
        Destroy(loadedLevel);
        levelIndex = (levelIndex + 1);
        LoadLevelBox(boxLoadDistance);
        //SlideBox(loadedLevel);
        StartCoroutine(SlideLevelSelectBox(loadedLevel));
        if (levelIndex == level.Length - 1) { DisableAllComponentsExceptThis(); }
        else { gameObject.SetActive(true); }
        if (levelIndex == 0) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
    }

    public void OnBackButtonClick()
    {
        Destroy(loadedLevel);
        levelIndex = (levelIndex - 1);
        LoadLevelBox(-boxLoadDistance);
        SlideBox(loadedLevel);
        if (levelIndex == 0) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
        if (levelIndex == level.Length - 1) { DisableAllComponentsExceptThis(); }
        else { forwardsButton.SetActive(true); }

    }

    void LoadLevelBox(int xpos) {

        GameObject newLevel = Instantiate(level[levelIndex], new Vector3(xpos, 0, 0), Quaternion.identity, canvasTransform);
        loadedLevel = newLevel;
        newLevel.transform.localPosition = new Vector3(xpos, 0, 0); // Set local position relative to the parent
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

        //box.transform.localPosition = new Vector3(0, 0, 0);
        StartCoroutine(SlideLevelSelectBox(box));

    }

    IEnumerator SlideLevelSelectBox(GameObject box)
    {
        moving = true;
        yield return new WaitForSeconds(slideTime);
        moving = false;
        //Snaps the level to the center if it is not already there
        box.transform.localPosition = new Vector3(0, 0, 0);
    }

    void DisableAllComponentsExceptThis()
    {
        Component[] components = GetComponents<Component>();

        // Log the type of each component
        foreach (Component component in components)
        {
            Debug.Log("Component: " + component.GetType());
            if (component is UnityEngine.UI.Image image)
            {
                image.enabled = false;
            }
            else if (component is UnityEngine.UI.Button button)
            {
                button.enabled = false;
            }
           
        }
    }

}
