using System.Collections;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SwitchLevelPreview : MonoBehaviour
{
    [SerializeField] private bool forwards;
    public UnityEngine.UI.Button moveButton;
    public LevelPreview[] levelPreviews;
    private GameObject[] level;
    [SerializeField] private GameObject loadedLevel;
    public GameObject backButton;
    public GameObject forwardsButton;
    [SerializeField] private int levelIndex = 0;
    [SerializeField] private Transform canvasTransform;
    public int boxLoadDistance;
    private bool slideLeft = true;
    public float slideTime;
    private bool moving = false;
    private int stopRadius = 10;


    void Start()
    {
        level = new GameObject[levelPreviews.Length];
        moveButton.onClick.AddListener(OnForwardsButtonClick);
        InstantiateBaseBox();
        backButton.SetActive(false);
        for (int i = 0; i < levelPreviews.Length; i++)
        {
            Spawn(levelPreviews[i]);
            level[i] = levelPreviews[i].emptyLevelPrefab;
        }
    }

    public void Spawn(LevelPreview preview)
    {
        GameObject panel = preview.emptyLevelPrefab.transform.GetChild(0).gameObject;
        panel.GetComponent<UnityEngine.UI.Image>().sprite = preview.levelImage;
        GameObject sceneChangeButton = preview.emptyLevelPrefab.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        sceneChangeButton.GetComponent<PlayButton>().sceneName = preview.sceneNameInEditor;
    }

    private void FixedUpdate()
    {
        /*
        if(moving)
        {

            if (slideLeft) { loadedLevel.transform.localPosition -= new Vector3(boxLoadDistance / (40 / slideTime), 0, 0); Debug.Log("sliding left"); }
            else { loadedLevel.transform.localPosition += new Vector3(boxLoadDistance / (40 / slideTime), 0, 0); Debug.Log("sliding right"); }
            
        }
        */

    }
    private void Update()
    {
        if (moving)
        {
            if (slideLeft) { loadedLevel.transform.localPosition -= new Vector3(boxLoadDistance / slideTime * Time.deltaTime, 0, 0); Debug.Log(loadedLevel.transform.localPosition); }
            else { loadedLevel.transform.localPosition += new Vector3(boxLoadDistance / slideTime * Time.deltaTime, 0, 0); }
            if ( loadedLevel.transform.localPosition.x >= -stopRadius && loadedLevel.transform.localPosition.x <= stopRadius) { 
                moving = false;
                loadedLevel.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
    private void MoveBox()
    {
            

    }

    void OnForwardsButtonClick()
    {
        slideLeft = true;
        Destroy(loadedLevel);
        levelIndex = (levelIndex + 1);
        LoadLevelBox(boxLoadDistance);
        //SlideBox(loadedLevel);
        StartCoroutine(SlideLevelSelectBox(loadedLevel));
        if (levelIndex == level.Length - 1) { DisableAllComponentsExceptThis(true); }
        else { DisableAllComponentsExceptThis(false); }
        if (levelIndex == 0) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
    }

    public void OnBackButtonClick()
    {
        slideLeft = false;
        Destroy(loadedLevel);
        levelIndex = (levelIndex - 1);
        LoadLevelBox(-boxLoadDistance);
        SlideBox(loadedLevel);
        if (levelIndex == 0) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
        if (levelIndex == level.Length - 1) { DisableAllComponentsExceptThis(true); }
        else { DisableAllComponentsExceptThis(false); }

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

        //box.transform.localPosition = new Vector3(0, 0, 0);
        StartCoroutine(SlideLevelSelectBox(box));

    }

    IEnumerator SlideLevelSelectBox(GameObject box)
    {
        moving = true;
        yield return new WaitForSeconds(slideTime);
        //Snaps the level to the center if it is not already there
        box.transform.localPosition = new Vector3(0, 0, 0);
    }

    void DisableAllComponentsExceptThis(bool disable)
    {
        Component[] components = GetComponents<Component>();

        // Log the type of each component
        foreach (Component component in components)
        {
            Debug.Log("Component: " + component.GetType());
            if (component is UnityEngine.UI.Image image)
            {
                image.enabled = !disable;
            }
            else if (component is UnityEngine.UI.Button button)
            {
                button.enabled = !disable;
            }
           
        }
    }

}
