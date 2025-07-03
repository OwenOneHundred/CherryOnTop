using UnityEngine;

public class GlassesLaserShrink : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float shrinkSpeed = 5;
    public void SetUp(GameObject toppingObj, GameObject targetedCherry)
    {
        lineRenderer.positionCount = 11;

        Vector3 startPos = toppingObj.transform.root.Find("Lens").position;
        for (int i = 0; i < 10; i += 1)
        {
            lineRenderer.SetPosition(i, Vector3.Lerp(startPos, targetedCherry.transform.position, i / 10));
        }
        lineRenderer.SetPosition(10, targetedCherry.transform.position);
    }

    void Update()
    {
        if (lineRenderer.widthMultiplier > 0)
        {
            lineRenderer.widthMultiplier -= Time.deltaTime * shrinkSpeed;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
