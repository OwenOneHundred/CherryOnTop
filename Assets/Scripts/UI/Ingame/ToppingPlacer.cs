using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventBus;
using UnityEditor;
using UnityEngine;
using System;

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

    CameraControl cameraControl;
    readonly Vector3 arbitraryArtificialLift = new Vector3(0, 0.09f, 0);
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

        cameraControl = Camera.main.transform.root.GetComponent<CameraControl>();
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
        Transform circleTransform = transparentObject.transform.GetChild(0);
        LineRenderer circleLineRenderer = circleTransform.GetComponent<LineRenderer>();

        // set up circle
        TargetingSystem targetingSystem = topping.towerPrefab.GetComponentInChildren<TargetingSystem>();
        if (targetingSystem != null)
        {
            float range = targetingSystem.GetRange();
            circleTransform.transform.localScale = new Vector3(range, 1, range) / toppingMeshFilter.transform.lossyScale.x;
            circleTransform.gameObject.SetActive(true);
        }
        else
        {
            circleTransform.gameObject.SetActive(false);
        }

        // set up transparent mesh
        transparentMeshFilter.mesh = toppingMeshFilter.sharedMesh; // set transparent mesh to topping mesh
        transparentObject.transform.localScale = toppingMeshFilter.transform.lossyScale; // set transparent obj scale
        transparentObject.transform.rotation = toppingMeshFilter.transform.rotation; // set transparent obj rotation

        // re-rotate circle to flat
        circleTransform.rotation = Quaternion.identity;

        transparentObject.SetActive(false);

        float lowestPointOffset = GetLowestPointOffset(toppingMeshFilter.sharedMesh.bounds, toppingMeshFilter.transform, Vector3.down);

        bool placementValidCheck = false;

        bool mouseIsInSidebar;
        bool mouseLeftSidebar = false;

        var whiteArray = MakeArrayOfMaterial(topping.towerPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterials.Length, white);
        var redArray = MakeArrayOfMaterial(topping.towerPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterials.Length, red);

        while (Input.GetMouseButton(0))
        {
            mouseIsInSidebar = Input.mousePosition.x > inventoryXPos;

            if (!mouseLeftSidebar && !mouseIsInSidebar) { SoundEffectManager.sfxmanager.PlayOneShot(dragOutSound); }

            if (mouseLeftSidebar && mouseIsInSidebar)
            {
                StopPlacingTopping();
                yield break;
            }

            if (!mouseIsInSidebar) { mouseLeftSidebar = true; }

            Vector3 cameraPositionWithShake = cameraControl.transform.position;
            cameraControl.transform.position = Vector3.zero; // Holy hack lmao part 1
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            cameraControl.transform.position = cameraPositionWithShake; // Holy hack lmao part 2
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, placeableLayers))
            {
                cakePos = hit.point;
                objCenter = CalculatePreviewPosition(cakePos, toppingMeshFilter.sharedMesh, lowestPointOffset, transparentMeshFilter.transform, toppingMeshFilter.transform);

                transparentObject.SetActive(true);
                transparentObject.transform.position = objCenter;

                placementValidCheck = CheckIfPlacementValid(toppingMeshFilter, GetCenterOfPreviewMesh(transparentMeshFilter), toppingMeshFilter.sharedMesh, cakePos);
                meshRenderer.sharedMaterials = placementValidCheck ? whiteArray : redArray;
                circleLineRenderer.startColor = placementValidCheck ? Color.white : Color.red;
                circleLineRenderer.endColor = placementValidCheck ? Color.white : Color.red;
            }
            else
            {
                transparentObject.SetActive(false);
            }
            yield return null;
        }

        if (placementValidCheck)
        {
            yield return StartCoroutine(PlaceTopping(topping, transparentMeshFilter.transform.position, topping.towerPrefab.transform.rotation, true));
        }
        StopPlacingTopping();
    }

    private Material[] MakeArrayOfMaterial(int num, Material material)
    {
        Material[] materials = new Material[num];
        for (int i = 0; i < num; i++)
        {
            materials[i] = material;
        }
        return materials;
    }

    private Vector3 CalculatePreviewPosition(Vector3 cakePos, Mesh mesh, float lowestPointOffset, Transform transparentObjectMesh, Transform toppingMeshFilterTransform)
    {
        Vector3 localCenterOffset = mesh.bounds.center;
        Vector3 worldCenterOffset = transparentObjectMesh.rotation * Vector3.Scale(localCenterOffset, transparentObjectMesh.lossyScale);

        Vector3 objCenter = cakePos - worldCenterOffset + new Vector3(0, lowestPointOffset, 0) + arbitraryArtificialLift;

        return objCenter;
    }

    private Vector3 GetCenterOfPreviewMesh(MeshFilter transparentObjectMesh)
    {
        Vector3 localCenter = transparentObjectMesh.sharedMesh.bounds.center;
        Vector3 result = transparentObjectMesh.transform.TransformPoint(localCenter);
        return result;
    }

    private bool CheckIfPlacementValid(MeshFilter prefabMeshFilter, Vector3 pos, Mesh mesh, Vector3 cakePos)
    {
        Bounds bounds = mesh.bounds;

        Vector3 worldExtents = Vector3.Scale(bounds.extents, prefabMeshFilter.transform.lossyScale);

        var result = Physics.OverlapBox(pos, worldExtents * 0.875f, prefabMeshFilter.transform.rotation, layersThatBlockPlacement);

        bool notOverlappingAnything = result.Count() == 0;

        //bool tooCloseToTrack = CheckIfOnTrack(cakePos);
        bool tooCloseToTrack = false;

        return notOverlappingAnything && (!tooCloseToTrack);
    }

    private bool CheckIfOnTrack(Vector3 cakePos, float acceptableDistance = 0.26f)
    {
        return TrackFunctions.trackFunctions.GetAllLineSegmentsThatIntersectSphere(cakePos, acceptableDistance).Count != 0;
    }

    private float GetLowestPointOffset(Bounds bounds, Transform meshTransform, Vector3 groundDirection)
    {
        Vector3 worldExtents = Vector3.Scale(bounds.extents, meshTransform.lossyScale);
        float toGround = Mathf.Abs(Vector3.Dot(worldExtents, groundDirection));
        return toGround;
    }

    private void StopPlacingTopping()
    {
        transparentObject.SetActive(false);

        if (iconControl == null) { return; } // if the item is placed, the icon is destroyed by the inventory,
        // so the "StopPlacing" call errors.
        iconControl.StopPlacing();
        iconControl = null;
    }

    public IEnumerator PlaceTopping(Topping topping, Vector3 position, Quaternion rotation, bool playSound = false)
    {
        GameObject newToppingObj = Instantiate(topping.towerPrefab, position, rotation); // spawn obj

        ToppingRegistry.toppingRegistry.RegisterPlacedTopping(topping, newToppingObj); // register

        newToppingObj.GetComponent<ToppingObjectScript>().topping = topping; // set topping on object to be read later
        
        EventBus<TowerPlacedEvent>.Raise(new TowerPlacedEvent(topping, newToppingObj)); // call placed tower event
        
        topping.SetGameObjectOnEffects(newToppingObj);
        topping.RegisterEffects(newToppingObj);

        Inventory.inventory.RemoveItemByID(topping.ID); // remove from inventory

        newToppingObj.GetComponentInChildren<ToppingObjInteractions>().OnPlacedFromInventory();

        yield return StartCoroutine(PlaceToppingAnimation(playSound, position, newToppingObj));
    }

    private IEnumerator PlaceToppingAnimation(bool playSound, Vector3 position, GameObject newToppingObj)
    {
        float fallDistance = 1.25f;
        float fallSpeed = 15f;
        Vector3 topPosition = position + new Vector3(0, fallDistance, 0);
        newToppingObj.transform.position = topPosition;
        while (newToppingObj.transform.position.y > position.y)
        {
            newToppingObj.transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
            yield return null;
        }
        newToppingObj.transform.position = position;

        Destroy(Instantiate(toppingPlaceEffect, position, Quaternion.identity), 5); // create particle effect

        if (playSound) { SoundEffectManager.sfxmanager.PlayOneShot(placeSound); }
    }

    public void PlaceToppingViaLoad(Topping topping, Vector3 position, Quaternion rotation)
    {
        GameObject newToppingObj = Instantiate(topping.towerPrefab, position, rotation); // spawn obj

        ToppingRegistry.toppingRegistry.RegisterPlacedTopping(topping, newToppingObj); // register

        newToppingObj.GetComponent<ToppingObjectScript>().topping = topping; // set topping on object to be read later
        newToppingObj.transform.root.GetComponentInChildren<ToppingObjInteractions>().OnClickedOff();

        topping.RegisterEffects(newToppingObj);
        topping.SetGameObjectOnEffects(newToppingObj);
    }
}
