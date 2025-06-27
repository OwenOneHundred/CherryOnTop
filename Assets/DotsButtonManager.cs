using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotsButtonManager : MonoBehaviour
{
    [SerializeField] GameObject dotsButtonPrefab;
    List<Image> dotsImages = new();
    [SerializeField] Color selectedColor;
    [SerializeField] Color deselectedColor;
    public void SetUp(int count, Action<int> onClickAction)
    {
        for (int i = 0; i < count; i++)
        {
            int index = i;
            GameObject newDot = Instantiate(dotsButtonPrefab, transform);
            dotsImages.Add(newDot.GetComponent<Image>());
            newDot.GetComponent<Button>().onClick.AddListener(() =>
            {
                onClickAction(index);
                SetActiveDot(index);
            });
        }
    }

    public void SetActiveDot(int index)
    {
        foreach (Image image in dotsImages)
        {
            image.color = deselectedColor;
        }

        dotsImages[index].color = selectedColor;
    }
}
