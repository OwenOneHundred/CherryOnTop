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
        buttonImg.alphaHitTestMinimumThreshold = 1;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        buttonImg.sprite = hoverSprite;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImg.sprite = deselectSprite;
    }
}
