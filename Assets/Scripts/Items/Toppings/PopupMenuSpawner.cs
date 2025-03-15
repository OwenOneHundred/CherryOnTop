using System.Collections;
using UnityEngine;

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
        infoPopup = Instantiate(infoPopupPrefab, canvas.transform);
        RectTransform rect = infoPopup.GetComponent<RectTransform>();
        rect.position = Camera.main.WorldToScreenPoint(transform.position) + (Vector3.up * 400);
        infoPopup.GetComponent<InfoPopup>().SetUp(null, gameObject);
    }
}
