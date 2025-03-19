using System.Collections;
using System.Linq;
using EventBus;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class ToppingPlacer : MonoBehaviour
{
    [SerializeField] LayerMask placeableLayers;
    [SerializeField] LayerMask layersThatBlockPlacement;
    [SerializeField] Material red;
    [SerializeField] Material white;
    [SerializeField] GameObject placePreview;
    InventoryIconControl iconControl;
    bool placingTopping = false;

    public static ToppingPlacer toppingPlacer;

    GameObject transparentObject;
    [SerializeField] GameObject toppingPlaceEffect;

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

        while (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, placeableLayers))
            {
                cakePos = hit.point;
                objCenter = cakePos + new Vector3(0, lowestPointOffset, 0);

                transparentObject.SetActive(true);
                transparentObject.transform.position = cakePos;

                placementValidCheck = CheckIfPlacementValid(toppingMeshFilter, objCenter, toppingMeshFilter.sharedMesh);
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
            PlaceTopping(topping, cakePos);
        }
        StopPlacingTopping();
    }

    private bool CheckIfPlacementValid(MeshFilter prefabMeshFilter, Vector3 pos, Mesh mesh)
    {
        Bounds bounds = mesh.bounds;
        Vector3 extents = prefabMeshFilter.transform.rotation * Vector3.Scale(bounds.extents, prefabMeshFilter.transform.lossyScale);
        var result = Physics.OverlapBox(pos + checkAreaVerticalOffset, extents, Quaternion.identity, layersThatBlockPlacement);
        
        return result.Count() == 0;
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

    private void PlaceTopping(Topping topping, Vector3 position)
    {
        GameObject newToppingObj = Instantiate(topping.towerPrefab, position, Quaternion.identity); // spawn obj
        ToppingRegistry.toppingRegistry.RegisterPlacedTopping(Instantiate(topping), newToppingObj); // register
        EventBus<TowerPlacedEvent>.Raise(new TowerPlacedEvent(topping, newToppingObj)); // call placed tower event
        Destroy(Instantiate(toppingPlaceEffect, position, Quaternion.identity), 6); // create particle effect
        Inventory.inventory.RemoveItem(topping); // remove from inventory
    }
}
