using System.Drawing;
using UnityEngine;

public class CherryTypes : MonoBehaviour
{
    public CherrySize cherrySize = CherrySize.Normal;
    public CherryHitbox cherryHitbox;
    public CherryMovement cherryMovement;
    [SerializeField] MeshRenderer meshRenderer;

    public bool IsMetal
    {
        get { return _isMetal; }
        set
        {
            _isMetal = value;
            if (value)
            {
                meshRenderer.sharedMaterial = metalMaterial;
                cherryHitbox.cherryHealth *= 100;
            }
        }
    }
    bool _isMetal = false;

    [SerializeField] Material metalMaterial;

    public void Awake()
    {
        cherryHitbox = GetComponent<CherryHitbox>();
        cherryMovement = GetComponent<CherryMovement>();
    }

    public enum CherrySize
    {
        Small = 0,
        Normal = 1,
        Large = 2,
        SuperLarge = 3,
        None = 999
    }

    // Sets Cherry Health and Speed based on a cherry's given size
    public void SetCherryHealthAndSpeed()
    {
        switch (cherrySize)
        {
            case CherrySize.Small:
                cherryHitbox.cherryHealth *= 4f;
                cherryMovement.baseSpeed *= 2f;
                transform.localScale *= 0.6f;
                break;
            case CherrySize.Normal:
                cherryHitbox.cherryHealth *= 1.0f;
                cherryMovement.baseSpeed *= 1.0f;
                transform.localScale *= 1f;
                break;
            case CherrySize.Large:
                cherryHitbox.cherryHealth *= 4.5f;
                cherryMovement.baseSpeed *= 0.75f;
                transform.localScale *= 2f;
                break;
            case CherrySize.SuperLarge:
                cherryHitbox.cherryHealth *= 18f;
                cherryMovement.baseSpeed *= 0.45f;
                transform.localScale *= 5f;
                break;
        }
    }

}
