using GameSaves;
using UnityEngine;
using System.Collections;
using EventBus;
using UnityEngine.Rendering;

public class CherryBlossom : CherryHitbox
{

    public GameObject spawnPrefab;
    public Mesh bloomMesh;
    public Material bloomMaterial;
    public float initialHealth;


    public void Start()
    {
        initialHealth = cherryHealth;
    }

    protected override void Die()
    {
        if (dead) { return; }

        if (cherryHealth <= 0)
        {
            cherryHealth = initialHealth / 2;
            dead = true;
            int numSpawned = UnityEngine.Random.Range(4, 7);
            StartCoroutine(bloom(numSpawned));
        }
    }

    IEnumerator bloom(int cherriesSpawn) {
        Debug.Log("Cherry Health: " + cherryHealth);
        Debug.Log("Im blooming it: " + cherriesSpawn);
        GetComponentInChildren<MeshFilter>().sharedMesh = bloomMesh;
        GetComponentInChildren<MeshRenderer>().sharedMaterial = bloomMaterial;
        transform.Rotate(90f, 0, 0);
        GetComponentInChildren<Collider>().enabled = false;
        GameObject track = GameObject.FindGameObjectWithTag("Track");
        CherrySpawner spawner = track.GetComponent<CherrySpawner>();
        CherryMovement cherryMovement = GetComponent<CherryMovement>();
        CherryTypes cherryType = GetComponent<CherryTypes>();
        cherryMovement.baseSpeed = 0;
        for (int i = 0; i < cherriesSpawn; i++)
        {
            Debug.Log("Loop: " + i);
            GameObject newCherry = Instantiate(spawnPrefab, transform.position, Quaternion.identity);
            spawner.cherryManager.RegisterCherry(newCherry.GetComponentInChildren<CherryMovement>());
            CherryMovement newCherryMovement = newCherry.GetComponent<CherryMovement>();
            newCherryMovement.currentTarget = cherryMovement.currentTarget;
            newCherryMovement.currentPosition = cherryMovement.currentPosition;
            newCherryMovement.currentTrack = cherryMovement.currentTrack;
            newCherryMovement.distanceTraveled = cherryMovement.distanceTraveled;
            CherryTypes newCherryType = newCherry.GetComponent<CherryTypes>();
            newCherryType.cherrySize = cherryType.cherrySize - 1;
            if (newCherryType.cherrySize < 0) 
            {
                newCherryType.cherrySize = 0;
            }
            newCherryType.SetCherryHealthAndSpeed();
            yield return new WaitForSeconds(.3f);
        }
        CherryManager.Instance.OnCherryKilled(cherryMovement);
        EventBus<CherryDiesEvent>.Raise(new CherryDiesEvent(gameObject));
        Destroy(gameObject);
    }


}
