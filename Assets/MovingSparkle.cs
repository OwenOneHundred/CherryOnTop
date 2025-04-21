using System.Collections;
using UnityEngine;

public class MovingSparkle : MonoBehaviour
{
    Vector2 spawnTimeRange = new Vector2(4f, 9f);
    float chosenSpawnTime = 0;
    float timer = 0;
    [SerializeField] GameObject sparklePrefab;
    [SerializeField] RectTransform sparkleMovementPerimeter;
    Canvas canvas;
    [SerializeField] float sparkleSpeed = 4;
    [SerializeField] Vector2 sparkleLifetimeRange;
    void Start()
    {
        canvas = transform.root.GetComponent<Canvas>();
        chosenSpawnTime = UnityEngine.Random.Range(spawnTimeRange.x, spawnTimeRange.y);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > chosenSpawnTime)
        {
            StartCoroutine(SparkleRunner());
            timer = 0;
        }
    }

    IEnumerator SparkleRunner()
    {
        Vector3[] boxCorners = new Vector3[4];
        sparkleMovementPerimeter.GetWorldCorners(boxCorners);

        GameObject sparkle = Instantiate(sparklePrefab, Vector3.zero, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)), canvas.transform);
        int sparkleStartCornerIndex = UnityEngine.Random.Range(0, 3);
        int goalCornerIndex = sparkleStartCornerIndex + 1 > 3 ? 0 : sparkleStartCornerIndex + 1;
        sparkle.transform.position = boxCorners[goalCornerIndex];
        sparkle.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        float goalSparkleSize = 0.75f;

        float sparkleLifetime = UnityEngine.Random.Range(sparkleLifetimeRange.x, sparkleLifetimeRange.y);
        float sparkleTimer = 0;

        bool growing = true;

        while (sparkleTimer < sparkleLifetime)
        {
            if (growing)
            {
                growing = sparkle.transform.localScale.x < goalSparkleSize;
                sparkle.transform.localScale += 10 * Time.deltaTime * new Vector3(1, 1, 1);
            }
            else
            {
                sparkle.transform.localScale -= (0.9f / sparkleLifetime) * Time.deltaTime * new Vector3(1, 1, 1);
            }

            sparkle.transform.position = Vector3.MoveTowards(sparkle.transform.position, boxCorners[goalCornerIndex], sparkleSpeed);
            if (Vector3.Distance(sparkle.transform.position, boxCorners[goalCornerIndex]) < 0.01f)
            {
                goalCornerIndex += 1;
                if (goalCornerIndex > 3)
                {
                    goalCornerIndex = 0;
                }
            }

            sparkleTimer += Time.deltaTime;

            yield return null;
        }

        Destroy(sparkle);

    }
}
