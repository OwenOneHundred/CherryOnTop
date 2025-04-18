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
public class CherryBombData : SpecialtyCherry
{
    public CherryBombData(float weight) : base(weight)
    {

    }
}

[System.Serializable]
public class CherryBlossomData : SpecialtyCherry
{
    public CherryBlossomData(float weight) : base(weight)
    {

    }
}


