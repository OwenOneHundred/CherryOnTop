using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

//Changes the image on hover to the given sprite
public class HoverImgChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEngine.UI.Image buttonImg;
    public Sprite hoverSprite;
    private Sprite deselectSprite;

    void Start()
    {
        deselectSprite = buttonImg.sprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("selected!!!!!");
        buttonImg.sprite = hoverSprite;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImg.sprite = deselectSprite;
    }
}
