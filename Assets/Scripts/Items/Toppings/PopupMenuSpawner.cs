using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupMenuSpawner : MonoBehaviour
{
    [SerializeField] GameObject infoPopupPrefab;
    GameObject infoPopup;
    Canvas canvas;
    private void Start()
    {
        canvas = GameObject.FindAnyObjectByType<Canvas>();
    }

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        infoPopup = Instantiate(infoPopupPrefab, canvas.transform);
        RectTransform rect = infoPopup.GetComponent<RectTransform>();
        bool aboveScreenCenter = Camera.main.WorldToScreenPoint(transform.position).y > (Screen.height / 2f);
        rect.position = Camera.main.WorldToScreenPoint(transform.position) + (aboveScreenCenter ? (Vector3.down * 350) : (Vector3.up * 350));
        infoPopup.GetComponent<InfoPopup>().SetUp(GetComponentInParent<ToppingObjectScript>().topping, gameObject.transform.root.gameObject);
    }
}
