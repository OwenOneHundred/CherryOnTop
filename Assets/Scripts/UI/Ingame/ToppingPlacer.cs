using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventBus;
using UnityEngine;

public class ToppingPlacer : MonoBehaviour
{
    [SerializeField] LayerMask placeableLayers;
    [SerializeField] LayerMask layersThatBlockPlacement;
    [SerializeField] Material red;
    [SerializeField] Material white;
    [SerializeField] GameObject placePreview;
    [SerializeField] AudioFile placeSound;
    [SerializeField] AudioFile dragOutSound;
    readonly float inventoryXPos = 1460f;

    InventoryIconControl iconControl;
    bool placingTopping = false;

    public static ToppingPlacer toppingPlacer;

    GameObject transparentObject;
    [SerializeField] GameObject toppingPlaceEffect;

    List<List<Vector3>> trackPoints = new();

    readonly Vector3 checkAreaVerticalOffset = new Vector3(0, 0.02f, 0);
    public bool PlacingTopping
    {
        get { return placingTopping; }
        private set { placingTopping = value; }
    }

    void Awake()
    {
        if (toppingPlacer == null || toppingPlacer == this)
        {
            toppingPlacer = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        transparentObject = Instantiate(placePreview);
        transparentObject.SetActive(false);

        StoreAllTrackPositions();
    }

    private void StoreAllTrackPositions()
    {
        foreach (Transform track in GameObject.FindGameObjectWithTag("Track").transform)
        {
            LineRenderer lr = track.GetComponent<LineRenderer>();
            var linePoints = new Vector3[lr.positionCount];
            lr.GetPositions(linePoints);
            trackPoints.Add(new List<Vector3>(linePoints));
        }
    }

    public void StartPlacingTopping(Topping topping, InventoryIconControl iic)
    {
        PlacingTopping = true;
        iconControl = iic;
        iic.beingPlaced = true;

        StartCoroutine(StartPlace(topping));
    }

    private IEnumerator StartPlace(Topping topping)
    {
        Vector3 cakePos = Vector3.zero;
        Vector3 objCenter;

        if (transparentObject == null) { transparentObject = Instantiate(placePreview); }

        // get all the components
        MeshFilter transparentMeshFilter = transparentObject.GetComponent<MeshFilter>();
        MeshFilter toppingMeshFilter = topping.towerPrefab.GetComponentInChildren<MeshFilter>();
        MeshRenderer meshRenderer = transparentObject.GetComponent<MeshRenderer>();

        // set up transparent mesh
        transparentMeshFilter.mesh = toppingMeshFilter.sharedMesh; // set transparent mesh to topping mesh
        transparentObject.transform.localScale = toppingMeshFilter.transform.lossyScale; // set transparent obj scale
        transparentObject.transform.rotation = toppingMeshFilter.transform.rotation; // set transparent obj rotation

        transparentObject.SetActive(false);

        float lowestPointOffset =
            GetLowestPointOffset(
                toppingMeshFilter.sharedMesh.bounds,
                toppingMeshFilter.transform.rotation * Vector3.down,
                toppingMeshFilter.transform.lossyScale.y
            ); // find distance between topping center and the point on the topping closest to downward after rotation

        bool placementValidCheck = false;

        bool mouseIsInSidebar;
        bool mouseLeftSidebar = false;

        while (Input.GetMouseButton(0))
        {
            mouseIsInSidebar = Input.mousePosition.x > inventoryXPos;

            if (!mouseLeftSidebar && !mouseIsInSidebar) { SoundEffectManager.sfxmanager.PlayOneShot(dragOutSound); }
            
            if (mouseLeftSidebar && mouseIsInSidebar) { StopPlacingTopping(); yield break; }

            if (!mouseIsInSidebar) { mouseLeftSidebar = true; } 

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, placeableLayers))
            {
                cakePos = hit.point;
                objCenter = cakePos + new Vector3(0, lowestPointOffset, 0);

                transparentObject.SetActive(true);
                transparentObject.transform.position = objCenter;

                placementValidCheck = CheckIfPlacementValid(toppingMeshFilter, objCenter, toppingMeshFilter.sharedMesh, cakePos);
                meshRenderer.material = placementValidCheck ? white : red;
            }
            else
            {
                transparentObject.SetActive(false);
            }
            yield return null;
        }

        if (placementValidCheck)
        {
            PlaceTopping(topping, cakePos + new Vector3(0, lowestPointOffset, 0), topping.towerPrefab.transform.rotation, true);
        }
        StopPlacingTopping();
    }

    private bool CheckIfPlacementValid(MeshFilter prefabMeshFilter, Vector3 pos, Mesh mesh, Vector3 cakePos)
    {
        Bounds bounds = mesh.bounds;

        Vector3 worldExtents = Vector3.Scale(bounds.extents, prefabMeshFilter.transform.lossyScale);

        var result = Physics.OverlapBox(pos + checkAreaVerticalOffset, worldExtents * 0.8f, prefabMeshFilter.transform.rotation, layersThatBlockPlacement);
        

        bool notOverlappingAnything = result.Count() == 0;

        //bool tooCloseToTrack = CheckIfTooCloseToTrack(cakePos); this doesn't work, idk why
        bool tooCloseToTrack = false;

        return notOverlappingAnything && (!tooCloseToTrack);
    }

    private bool CheckIfTooCloseToTrack(Vector3 cakePos, float acceptableDistance = 0.525f)
    {
        foreach (List<Vector3> trackPositions in trackPoints)
        {
            for (int i = 0; i < trackPositions.Count - 1; i++)
            {
                Vector3 startPos = trackPositions[i];
                Vector3 endPos = trackPositions[i + 1];
                

                Vector3 closestOnPoints = ClosestPointOnLineSegment(startPos, endPos, cakePos);
                float distance = Vector3.Distance(cakePos, closestOnPoints);

                //Debug.Log("startpos: " + startPos + " endpos: " + endPos + " closestPoint: " + closestOnPoints + " distance: " + distance + " obj pos: " + cakePos);

                if (distance < acceptableDistance)
                {
                    return true;
                }
            }
        }
        return false;
    }

    Vector3 ClosestPointOnLineSegment(Vector3 A, Vector3 B, Vector3 target)
    {
        Vector3 fromAtoB = B - A;
        Vector3 fromAtoTarget = target - A;
        float t = Mathf.Clamp01(Vector2.Dot(fromAtoTarget, fromAtoB) / fromAtoB.sqrMagnitude);
        return A + t * fromAtoB;
    }

    private float GetLowestPointOffset(Bounds bounds, Vector3 groundDirection, float scale)
    {
        Vector3 farthestPoint = bounds.ClosestPoint(groundDirection.normalized * 10000);

        float distanceFromCenter = Vector3.Distance(bounds.center, farthestPoint);

        float scaledDistanceFromCenter = distanceFromCenter * scale;

        return scaledDistanceFromCenter;
    }

    private void StopPlacingTopping()
    {
        iconControl.beingPlaced = false;
        iconControl = null;
        transparentObject.SetActive(false);
    }

    public void PlaceTopping(Topping topping, Vector3 position, Quaternion rotation, bool playSound = false)
    {
        GameObject newToppingObj = Instantiate(topping.towerPrefab, position, rotation); // spawn obj

        ToppingRegistry.toppingRegistry.RegisterPlacedTopping(topping, newToppingObj); // register

        newToppingObj.GetComponent<ToppingObjectScript>().topping = topping; // set topping on object to be read later
        
        EventBus<TowerPlacedEvent>.Raise(new TowerPlacedEvent(topping, newToppingObj)); // call placed tower event
        Destroy(Instantiate(toppingPlaceEffect, position, Quaternion.identity), 6); // create particle effect

        if (playSound) { SoundEffectManager.sfxmanager.PlayOneShot(placeSound); }

        topping.RegisterEffects();
        topping.SetGameObjectOnEffects(newToppingObj);

        Inventory.inventory.RemoveItem(topping); // remove from inventory
    }
}
