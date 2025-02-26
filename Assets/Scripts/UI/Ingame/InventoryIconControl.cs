using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryIconControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Topping assignedTopping;
    bool hovered = false;
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
            ToppingPlacer.toppingPlacer.StartPlacingTopping(assignedTopping);
        }
    }
}
