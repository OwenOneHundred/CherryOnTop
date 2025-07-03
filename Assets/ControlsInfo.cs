using System.Collections.Generic;
using UnityEngine;

public class ControlsInfo : MonoBehaviour
{
    [SerializeField] private List<ControlsStatePair> controlStatePairs;
    public static ControlsInfo controlsInfo;
    public void Awake()
    {
        if (controlsInfo == null || controlsInfo == this)
        {
            controlsInfo = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(ControlsState controlsState)
    {
        foreach (ControlsStatePair pair in controlStatePairs)
        {
            if (pair.controlsState != controlsState)
            {
                pair.controlsObject.SetActive(false);
            }
            else
            {
                pair.controlsObject.SetActive(true);
            }
        }
    }

    public enum ControlsState
    {
        normal, placingTopping
    }

    [System.Serializable]
    private class ControlsStatePair
    {
        public ControlsState controlsState;
        public GameObject controlsObject;
    }
}
