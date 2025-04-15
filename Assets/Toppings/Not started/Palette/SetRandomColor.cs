using System.Collections.Generic;
using UnityEngine;

public class SetRandomColor : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", RandomColor());
    }

    private Color RandomColor()
    {
        List<Color> colors = new List<Color>() { Color.red, Color.blue, Color.yellow, Color.green, Color.magenta};
        return colors[UnityEngine.Random.Range(0, colors.Count)];
    }
}
