using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [SerializeField] GameObject lockObject;
    [SerializeField] Button tabButton;
    bool _locked = false;
    public bool Locked
    {
        get { return _locked; }
        protected set
        {
            if (value == _locked) { return; }

            lockObject.SetActive(value);
            tabButton.interactable = !value;

            _locked = value;
        }
    }

    public virtual void OnSwitchedTo()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnSwitchedOff()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnLevelChangedWhileActiveTab()
    {

    }

    public virtual void EvaluateIfShouldBeLocked(int levelIndex)
    {
        Locked = false;
    }
}
