using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryIconControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Topping assignedTopping;
    bool hovered = false;
    public bool beingPlaced = false;
    [SerializeField] Image image;
    [SerializeField] GameObject outline;
    [SerializeField] GameObject infoPopupPrefab;
    GameObject infoPopup;
    Canvas canvas;

    private void Start()
    {
        canvas = GameObject.FindAnyObjectByType<Canvas>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        infoPopup = Instantiate(infoPopupPrefab, canvas.transform);
        RectTransform rect = infoPopup.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(196, 259, 0);
        infoPopup.GetComponent<InfoPopup>().SetUpForInventoryItem(assignedTopping);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && hovered)
        {
            ToppingPlacer.toppingPlacer.StartPlacingTopping(assignedTopping, this);
            StartPlacing();
        }
    }

    private void StartPlacing()
    {
        beingPlaced = true;
        image.color = Color.gray;
        outline.SetActive(true);
    }

    public void StopPlacing()
    {
        beingPlaced = false;
        image.color = Color.white;
        outline.SetActive(false);
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
