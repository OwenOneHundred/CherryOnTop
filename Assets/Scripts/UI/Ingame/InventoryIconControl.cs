using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryIconControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Topping assignedTopping;
    bool hovered = false;
    bool selected = false;
    public bool beingPlaced = false;
    [SerializeField] Image image;
    [SerializeField] GameObject outline;
    InfoPopup infoPopup;

    private void Start()
    {
        infoPopup = GameObject.FindGameObjectWithTag("InfoPanel").GetComponent<InfoPopup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!hovered && !infoPopup.hovered)
            {
                OnClickedOff();
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hovered)
            {
                OnClicked();
            }
        }
    }

    private void StartPlacing()
    {
        beingPlaced = true;
        image.color = Color.gray;
    }

    public void StopPlacing() // called by topping placer
    {
        beingPlaced = false;
        image.color = Color.white;
    }

    public void OnClicked()
    {
        selected = true;
        ToppingPlacer.toppingPlacer.StartPlacingTopping(assignedTopping, this);
        StartPlacing();
        infoPopup.SetUpForInventoryItem(assignedTopping);
        outline.SetActive(true);
    }

    public void OnClickedOff()
    {
        if (!selected) { return; }
        infoPopup.Clear();
        selected = false;
        outline.SetActive(false);
    }

    public void OnDestroy()
    {
        OnClickedOff();
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
