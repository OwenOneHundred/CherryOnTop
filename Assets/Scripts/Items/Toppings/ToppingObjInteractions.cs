using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToppingObjInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject radiusCirclePrefab;
    GameObject radiusCircle;
    InfoPopup infoPopup;
    private bool hovered = false;
    private bool Hovered
    {
        get { return hovered; }
        set { hovered = value; }
    }
    [SerializeField] Vector3 radiusCircleOffset = default;
    bool selected = false;
    private void Awake()
    {
        infoPopup = Shop.shop.infoPopup;
    }

    public void OnPlacedFromInventory()
    {
        OnClicked();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!Hovered && !infoPopup.hovered)
            {
                OnClickedOff();
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Hovered && !selected)
            {
                OnClicked();
            }
        }
    }

    private void OnClicked()
    {
        if (selected) { return; }
        selected = true;
        SetUpPopupMenu();
        SetUpRadiusCircle();
    }

    public void OnClickedOff()
    {
        if (!selected) { return; }
        selected = false;
        DestroyRadiusCircle();
        ClearInfoPopup();
    }

    private void SetUpPopupMenu()
    {
        infoPopup.SetUp(GetComponentInParent<ToppingObjectScript>().topping, gameObject.transform.root.gameObject);
    }

    private void SetUpRadiusCircle()
    {
        if (transform.parent.GetChild(1).TryGetComponent(out TargetingSystem targetingSystem))
        {
            radiusCircle = Instantiate(radiusCirclePrefab, transform.position + radiusCircleOffset, Quaternion.identity);
            float range = targetingSystem.GetRange();
            radiusCircle.transform.localScale = new Vector3(range, range, range);
        }
    }

    private void DestroyRadiusCircle()
    {
        if (radiusCircle != null)
        {
            Destroy(radiusCircle);
        }
    }

    private void ClearInfoPopup()
    {
        infoPopup.Clear();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hovered = true;
    }

    public void OnDestroy()
    {
        OnClickedOff();
    }
}
