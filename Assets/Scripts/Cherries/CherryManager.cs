using System.Collections.Generic;
using UnityEngine;

public class CherryManager : MonoBehaviour
{
    public static CherryManager Instance; // Singleton for easy access
    private List<GameObject> cherries = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
       // UpdateCherryOrder();
    }

    // Add a cherry to the list
    public void RegisterCherry(GameObject cherry)
    {
        if (!cherries.Contains(cherry))
            cherries.Add(cherry);
    }

    // Remove a cherry when it dies or leaves the scene
    public void RemoveCherry(GameObject cherry)
    {
        if (cherries.Contains(cherry))
            cherries.Remove(cherry);
    }

    // Sort cherries by distance traveled (highest first)
    /*
    private void UpdateCherryOrder()
    {
        cherries.Sort((a, b) => b.TotalDistanceTraveled.CompareTo(a.TotalDistanceTraveled));
    }
    */

    // Get the top cherry (highest traveled distance)
    public GameObject GetHighestPriorityCherry()
    {
        if (cherries.Count > 0) return cherries[0];
        return null;
    }

    // Get all cherries in order
    public List<GameObject> GetOrderedCherries()
    {
        return cherries;
    }
}

