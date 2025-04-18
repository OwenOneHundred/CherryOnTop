using UnityEngine;

[System.Serializable]
public class SpecialtyCherry
{
    public float weight = 1;
    public GameObject prefab;

    public SpecialtyCherry(float weight)
    {
        this.weight = weight;
    }
}

[System.Serializable]
public class CherryBlossom : SpecialtyCherry
{
    public CherryBlossom(float weight) : base(weight)
    {

    }
}

[System.Serializable]
public class CherryBomb : SpecialtyCherry
{
    public CherryBomb(float weight) : base(weight)
    {

    }
}


