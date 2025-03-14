using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryIconControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Topping assignedTopping;
    bool hovered = false;
    public bool beingPlaced = false;
    [SerializeField] Image image;
    [SerializeField] GameObject outline;
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
        if (Input.GetMouseButtonDown(0) && hovered)
        {
            ToppingPlacer.toppingPlacer.StartPlacingTopping(assignedTopping, this);
            image.color = Color.gray;
            outline.SetActive(true);
        }
        else
        {
            image.color = Color.white;
            outline.SetActive(false);
        }
        outline.SetActive(beingPlaced);
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
