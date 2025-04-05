using System.Collections.Generic;
using UnityEngine;

public class CherryManager : MonoBehaviour
{
    public static CherryManager Instance; // Singleton for easy access
    [SerializeField] private List<CherryMovement> cherries = new List<CherryMovement>();

    float totalTrackLength = 42.21752f;
    [SerializeField] bool calculateTrackLength = false;
    AudioManager audioManager;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (calculateTrackLength) { totalTrackLength = CalculateTrackLength(); }
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
       UpdateCherryOrder();
       UpdateAudioCutoff();
    }

    // Add a cherry to the list
    public void RegisterCherry(CherryMovement cherry)
    {
        if (!cherries.Contains(cherry))
        {
            cherries.Add(cherry);
        }
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
        cherries.Remove(cherry);
    }

    // Sort cherries by distance traveled (highest first)
    private void UpdateCherryOrder()
    {     
        cherries.Sort((a, b) => b.distanceTraveled.CompareTo(a.distanceTraveled)); 
    }

    private void UpdateAudioCutoff()
    {
        float farthestCherryCoveredTrackPercentage;

        if (cherries.Count == 0) { farthestCherryCoveredTrackPercentage = 0; }
        else { farthestCherryCoveredTrackPercentage = cherries[0].distanceTraveled / totalTrackLength; }

        if (farthestCherryCoveredTrackPercentage < 0.9f) { farthestCherryCoveredTrackPercentage = 0;}

        audioManager.SetLowpass(Mathf.Clamp01(farthestCherryCoveredTrackPercentage - 0.9f) * 2.6f);
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

    private float CalculateTrackLength()
    {
        float totalDistance = 0;
        
        foreach (LineRenderer lineRenderer in GameObject.FindGameObjectWithTag("Track").GetComponentsInChildren<LineRenderer>())
        {
            int positionsAmount = lineRenderer.positionCount;
            var linePositions = new Vector3[positionsAmount];
            lineRenderer.GetPositions(linePositions);
            for (int i = 1; i < positionsAmount; i++)
            {
                totalDistance += Vector3.Distance(linePositions[i], linePositions[i - 1]);
            }
        }

        return totalDistance;
    }
}

