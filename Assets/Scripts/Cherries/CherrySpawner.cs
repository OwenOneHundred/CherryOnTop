using UnityEngine;

public class CherrySpawner : MonoBehaviour
{
    int roundNumber = 1;
    float roundTimer = 0;

    float defaultTimeBetweenCherries = 1;
    float defaultCherriesPerRound = 1;

    readonly float oddNumberCherryCountMultiplier = 1.5f;
    readonly float evenNumberCherrySpacingMultiplier = 0.75f;
    
}
