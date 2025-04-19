using GameSaves;
using UnityEngine;

public class CherryBlossom : CherryHitbox
{

    // Update is called once per frame
    void Update()
    {
        if (cherryHealth <= 0) {
            int numSpawned = UnityEngine.Random.Range(4, 7);
            bloom(numSpawned);
        }
    }

    void bloom(int cherriesSpawn) {

        GameObject track = GameObject.FindGameObjectWithTag("Track");
        CherrySpawner spawner = track.GetComponent<CherrySpawner>();
        CherryMovement cherryMovement = GetComponent<CherryMovement>();
        for (int i = 0; i < cherriesSpawn; i++)
        {
            GameObject newCherry = Instantiate(spawner.cherryPrefab, transform.position, Quaternion.identity);
            spawner.cherryManager.RegisterCherry(newCherry.GetComponentInChildren<CherryMovement>());
            CherryMovement newCherryMovement = newCherry.GetComponent<CherryMovement>();
            newCherryMovement.currentTarget = cherryMovement.currentTarget;
            CherryTypes newCherryType = newCherry.GetComponent<CherryTypes>();
            newCherryType.cherrySize = CherryTypes.CherrySize.Small;
            newCherryType.SetCherryHealthAndSpeed();
        }
    }
}
