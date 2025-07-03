using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToppingObjInteractions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject radiusCirclePrefab;
    [SerializeField] LayerMask cakeLayer;
    MeshRenderer meshRenderer;
    GameObject radiusCircle;
    InfoPopup infoPopup;
    private bool hovered = false;
    private bool Hovered
    {
        get { return hovered; }
        set { hovered = value; }
    }
    bool selected = false;
    private void Awake()
    {
        infoPopup = Shop.shop.infoPopup;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnPlacedFromInventory()
    {
        OnClicked(false);
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

    private void OnClicked(bool playSound = true)
    {
        if (selected) { return; }
        selected = true;
        SetUpPopupMenu(playSound);
        SetUpRadiusCircle();
    }

    public void OnClickedOff()
    {
        if (!selected) { return; }
        selected = false;
        DestroyRadiusCircle();
        ClearInfoPopup();
    }

    private void SetUpPopupMenu(bool playSound = true)
    {
        infoPopup.SetUp(GetComponentInParent<ToppingObjectScript>().topping, gameObject.transform.root.gameObject, playSound);
    }

    private void SetUpRadiusCircle()
    {
        if (transform.parent.GetChild(1).TryGetComponent(out TargetingSystem targetingSystem))
        {
            //RaycastHit hit;
            //Physics.Raycast(transform.position, Vector3.down, out hit, 10, cakeLayer);
            //float distanceToFloor = hit.distance;
            Vector3 position = meshRenderer.bounds.center - new Vector3(0, meshRenderer.bounds.size.y / 2, 0);
            radiusCircle = Instantiate(radiusCirclePrefab, position, Quaternion.identity);
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
