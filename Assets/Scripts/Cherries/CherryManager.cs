using System.Collections.Generic;
using UnityEngine;

public class CherryManager : MonoBehaviour
{
    public static CherryManager Instance; // Singleton for easy access
    private List<CherryMovement> cherries = new List<CherryMovement>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
       UpdateCherryOrder();
    }

    // Add a cherry to the list
    public void RegisterCherry(CherryMovement cherry)
    {
        if (!cherries.Contains(cherry))
            cherries.Add(cherry);
    }

    // called when a cherry dies
    public void OnCherryKilled(CherryMovement cherryMovement)
    {
        RemoveCherry(cherryMovement);
        RoundManager.roundManager.OnCherryKilled();
    }

    // Remove a cherry when it dies or leaves the scene
    public void RemoveCherry(CherryMovement cherry)
    {
        if (cherries.Contains(cherry))
            cherries.Remove(cherry);
    }

    // Sort cherries by distance traveled (highest first)
    private void UpdateCherryOrder()
    {     
        cherries.Sort((a, b) => b.distanceTraveled.CompareTo(a.distanceTraveled)); 
    }

    // Get the top cherry (highest traveled distance)
    public CherryMovement GetHighestPriorityCherry()
    {
        if (cherries.Count > 0) return cherries[0];
        return null;
    }

    // Get all cherries in order
    public List<CherryMovement> GetOrderedCherries()
    {
        return cherries;
    }
}

