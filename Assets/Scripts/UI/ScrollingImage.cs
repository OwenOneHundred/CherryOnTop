using UnityEngine;
using UnityEngine.UI;

public class ScrollingImage : MonoBehaviour
{
    Material material;
    Vector2 offset;
    public Vector2 offsetScrollSpeed = new Vector2(0.2f, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<Image>().material;
    }

    void Update()
    {
        material.mainTextureOffset += offsetScrollSpeed * Time.deltaTime;
    }
}
