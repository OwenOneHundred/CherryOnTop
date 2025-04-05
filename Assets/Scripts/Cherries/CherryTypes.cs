using System.Drawing;
using UnityEngine;

public class CherryTypes : MonoBehaviour
{
    public CherrySize cherrySize = CherrySize.Normal;
    public CherryType cherryType = CherryType.DefaultCherry;
    public float cherryHealth;
    public float cherrySpeed;
    public CherryHitbox cherryHitbox;
    public CherryMovement cherryMovement;

    public void Start()
    {
        cherryHitbox = GetComponent<CherryHitbox>();
        cherryMovement = GetComponent<CherryMovement>();
        cherryHealth = cherryHitbox.cherryHealth;
        cherrySpeed = cherryMovement.baseSpeed;
    }

    public enum CherrySize
    {
        Small = 0,
        Normal = 1,
        Large = 2,
        SuperLarge = 3
    }


    public enum CherryType
    {
        DefaultCherry = 0,
        FrozenCherry = 1,
        CherryBomb = 2,
        CherryBlossom = 3
    }

    // Sets Cherry Health and Speed based on a cherry's given size
    public void SetCherryHealthAndSpeed()
    {
        switch (cherrySize)
        {
            case CherrySize.Small:
                cherryHitbox.cherryHealth *= 0.75f;
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
                cherryHitbox.cherryHealth *= 20f;
                cherryMovement.baseSpeed *= 0.5f;
                transform.localScale *= 6f;
                break;
        }
    }

}
