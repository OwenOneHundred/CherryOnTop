using UnityEngine;

public class TabController : MonoBehaviour
{
    public virtual void OnSwitchedTo()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnSwitchedOff()
    {
        gameObject.SetActive(false);
    }
}
