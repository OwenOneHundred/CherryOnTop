using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SwitchLevelPreview : MonoBehaviour
{
    [SerializeField] private bool forwards;
    public UnityEngine.UI.Button moveButton;
    public LevelPreview[] levelPreviews;
    private GameObject[] level;
    [SerializeField] private GameObject loadedLevel;
    [SerializeField] private GameObject previousLoadedLevel;
    public GameObject backButton;
    public GameObject forwardsButton;
    [SerializeField] private int levelIndex = 0;
    [SerializeField] private Transform canvasTransform;
    public int boxLoadDistance;
    private bool slideLeft = true;
    public float slideTime;
    private bool moving = false;
    private int stopRadius = 10;
    public Sprite backButtonImg;
    public HoverImgChange forwardHover;
    public HoverImgChange backHover;
    float heldIntervalToMove = 0.5f;

    void Start()
    {
        forwardHover = GetComponent<HoverImgChange>();
        backHover = backButton.GetComponent<HoverImgChange>();
        level = new GameObject[levelPreviews.Length];
        moveButton.onClick.AddListener(OnForwardsButtonClick);
        
        
        backButton.SetActive(false);
        for (int i = 0; i < levelPreviews.Length; i++)
        {
            Spawn(levelPreviews[i]);
            
            level[i] = levelPreviews[i].levelPrefab;
            
        }
        InstantiateBaseBox();
    }

    public void Spawn(LevelPreview preview)
    {
        preview.levelPrefab = Instantiate(preview.emptyLevelPrefab);
        GameObject panel = preview.levelPrefab.transform.GetChild(0).gameObject;
        panel.GetComponent<UnityEngine.UI.Image>().sprite = preview.levelImage;
        GameObject sceneChangeButton = preview.levelPrefab.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        sceneChangeButton.GetComponent<PlayButton>().sceneName = preview.sceneNameInEditor;
    }

    private void Update()
    {
        float leftKeyHeldTimer = 0;
        float rightKeyHeldTimer = 0;
        
        if (Input.GetKeyDown(KeyCode.RightArrow) && moving == false)
        {
            rightKeyHeldTimer += Time.deltaTime;
            forwardHover.OnPointerEnter(null);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) && moving == false)
        {
            rightKeyHeldTimer = 0;
            forwardHover.OnPointerExit(null);
            OnForwardsButtonClick();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && moving == false)
        {
            leftKeyHeldTimer += Time.deltaTime;
            backHover.OnPointerEnter(null);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) && moving == false)
        {
            leftKeyHeldTimer = 0;
            backHover.OnPointerExit(null);
            OnBackButtonClick();
        }
        if (moving)
        {
            if (slideLeft) { 
                loadedLevel.transform.localPosition -= new Vector3(boxLoadDistance / slideTime * Time.deltaTime, 0, 0);
                previousLoadedLevel.transform.localPosition -= new Vector3((boxLoadDistance / slideTime * Time.deltaTime) * 1.5f, 0, 0);
                if (loadedLevel.transform.localPosition.x <= 0) {
                    StopMovingAndSnapToCenter();
                }
            }
            else { 
                loadedLevel.transform.localPosition += new Vector3(boxLoadDistance / slideTime * Time.deltaTime, 0, 0);
                previousLoadedLevel.transform.localPosition += new Vector3((boxLoadDistance / slideTime * Time.deltaTime) * 1.5f, 0, 0);
                if (loadedLevel.transform.localPosition.x >= 0)
                {
                    StopMovingAndSnapToCenter();
                }
            }
            
        }
    }
    private void StopMovingAndSnapToCenter()
    {
        moving = false;
        loadedLevel.transform.localPosition = new Vector3(0, 0, 0);
        Destroy(previousLoadedLevel);

    }

    void OnForwardsButtonClick()
    {
        if (levelIndex + 1 == level.Length) { return; }
        levelIndex = Mathf.Clamp((levelIndex + 1), 0, levelPreviews.Length - 1);
        
        slideLeft = true;
        LoadLevelBox((int) loadedLevel.transform.position.x + boxLoadDistance);
        StartCoroutine(SlideLevelSelectBox(loadedLevel));
        if (levelIndex == level.Length - 1) { DisableAllComponentsExceptThis(true); }
        else { DisableAllComponentsExceptThis(false); }
        if (levelIndex == 0) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
    }

    public void OnBackButtonClick()
    {
        if (levelIndex == 0) { return; }
        levelIndex = Mathf.Clamp((levelIndex - 1), 0, levelPreviews.Length - 1);
        
        slideLeft = false;
        LoadLevelBox((int)loadedLevel.transform.position.x - boxLoadDistance);
        SlideBox(loadedLevel);
        if (levelIndex == 0) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
        if (levelIndex == level.Length - 1) { DisableAllComponentsExceptThis(true); }
        else { DisableAllComponentsExceptThis(false); }
        backButton.GetComponent<UnityEngine.UI.Image>().sprite = backButtonImg;

    }

    void LoadLevelBox(int xpos) {
        if (level[levelIndex] == null) { 
            return; 
        }
        GameObject newLevel = Instantiate(level[levelIndex], new Vector3(xpos, 0, 0), Quaternion.identity, canvasTransform);
        previousLoadedLevel = loadedLevel;
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
        if (box != null) { 
            box.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    void DisableAllComponentsExceptThis(bool disable)
    {
        Component[] components = GetComponents<Component>();

        // Log the type of each component
        foreach (Component component in components)
        {
           
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
