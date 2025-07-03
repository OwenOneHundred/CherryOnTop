using UnityEngine;
using UnityEngine.UI;

public class MaskingSlider : MonoBehaviour
{
    [SerializeField] Image fillArea;
    [SerializeField] Slider slider;

    void Start()
    {
        fillArea.fillMethod = Image.FillMethod.Horizontal;
        OnValueChanged();
    }

    public void OnValueChanged()
    {
        fillArea.fillAmount = slider.value;
        Debug.Log(fillArea.fillAmount);
    }
}
