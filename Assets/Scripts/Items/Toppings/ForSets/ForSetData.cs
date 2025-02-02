using System.Collections.Generic;
using UnityEngine;

public class ForSetData
{
    public ForSetData(int number)
    {
        this.number = number;
    }
    public int number;
    public List<GameObject> gameObjects;

    public int GetCount()
    {
        int objsCount = 0;
        if (gameObjects != null)
        {
            objsCount = gameObjects.Count;
        }

        return objsCount > number ? objsCount : number;
    }
}
