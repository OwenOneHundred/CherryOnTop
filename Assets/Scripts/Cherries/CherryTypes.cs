using UnityEngine;

public class CherryTypes : MonoBehaviour
{
    public CherrySize cherrySize;
    public CherryType cherryType;
    public float cherryHealth;
    public float cherrySpeed;


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

    public void SetCherryProperties(CherrySize size, CherryType type)
    {
        cherrySize = size;
        cherryType = type;
        SetCherryHealthAndSpeed();
    }

    // Sets Cherry Health and Speed based on a cherry's given size
    public void SetCherryHealthAndSpeed()
    {
        switch (cherrySize)
        {
            case CherrySize.Small:
                cherryHealth *= 0.75f;
                cherrySpeed *= 1.5f;
                break;
            case CherrySize.Normal:
                cherryHealth *= 1.0f;
                cherrySpeed *= 1.0f;
                break;
            case CherrySize.Large:
                cherryHealth *= 1.5f;
                cherrySpeed *= 0.75f;
                break;
            case CherrySize.SuperLarge:
                cherryHealth *= 2.0f;
                cherrySpeed *= 0.5f;
                break;
        }
    }

}
