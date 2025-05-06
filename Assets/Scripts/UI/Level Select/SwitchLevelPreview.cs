using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class SwitchLevelPreview : MonoBehaviour
{
    [SerializeField] private bool forwards;
    public UnityEngine.UI.Button moveButton;
    public List<LevelPreview> levelPreviews;
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
    public Sprite backButtonImg;
    public HoverImgChange forwardHover;
    public HoverImgChange backHover;

    void Awake()
    {
        forwardHover = GetComponent<HoverImgChange>();
        backHover = backButton.GetComponent<HoverImgChange>();
        moveButton.onClick.AddListener(OnForwardsButtonClick);
        backButton.SetActive(false);
    }

    private void Start()
    {
        LoadLevelBox(0, true);
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
        if (moving) { return; }
        if (levelIndex + 1 == levelPreviews.Count) { return; }
        levelIndex = Mathf.Clamp((levelIndex + 1), 0, levelPreviews.Count - 1);
        
        slideLeft = true;
        LoadLevelBox((int) loadedLevel.transform.position.x + boxLoadDistance);
        StartCoroutine(SlideLevelSelectBox(loadedLevel));
        if (levelIndex == levelPreviews.Count - 1) { DisableAllComponentsExceptThis(true); }
        else { DisableAllComponentsExceptThis(false); }
        if (levelIndex == 0) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
    }

    public void OnBackButtonClick()
    {
        if (moving) { return; }
        if (levelIndex == 0) { return; }
        levelIndex = Mathf.Clamp((levelIndex - 1), 0, levelPreviews.Count - 1);
        
        slideLeft = false;
        LoadLevelBox((int)loadedLevel.transform.position.x - boxLoadDistance);
        SlideBox(loadedLevel);
        if (levelIndex == 0) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
        if (levelIndex == levelPreviews.Count - 1) { DisableAllComponentsExceptThis(true); }
        else { DisableAllComponentsExceptThis(false); }
    }

    void LoadLevelBox(int xpos, bool isFirstLevel = false) {
        if (levelPreviews[levelIndex] == null) { 
            return; 
        }
        LevelPreview currentPreview = levelPreviews[levelIndex];
        GameObject newLevelObj = Instantiate(levelPreviews[levelIndex].levelPrefab, new Vector3(xpos, 0, 0), Quaternion.identity, canvasTransform);
        newLevelObj.GetComponent<LevelPreviewManager>().Setup(currentPreview.levelImage, currentPreview.sceneNameIngame, currentPreview.sceneNameInEditor, levelIndex);
        newLevelObj.transform.SetSiblingIndex(1);
        if (!isFirstLevel) { previousLoadedLevel = loadedLevel; }
        loadedLevel = newLevelObj;
        newLevelObj.transform.localPosition = new Vector3(xpos, 0, 0); // Set local position relative to the parent
    }

    void SlideBox(GameObject box)
    {
        StartCoroutine(SlideLevelSelectBox(box));
    }

    IEnumerator SlideLevelSelectBox(GameObject box)
    {
        moving = true;
        yield return new WaitForSeconds(slideTime);
        //Snaps the level to the center if it is not already there
        if (box != null) { 
            StopMovingAndSnapToCenter();
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
