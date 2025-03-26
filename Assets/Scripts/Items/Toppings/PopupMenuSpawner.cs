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
        rect.position = Camera.main.WorldToScreenPoint(transform.position) + (Vector3.up * 400);
        infoPopup.GetComponent<InfoPopup>().SetUp(GetComponentInParent<ToppingObjectScript>().topping);
    }
}
