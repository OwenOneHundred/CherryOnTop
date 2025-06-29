using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericSparkleSpawner : MonoBehaviour
{
    [SerializeField] Vector2 sizeRange = new Vector2(0.3f, 1f);
    float sizeMultiplier = 1;
    [SerializeField] Vector2 cooldownRange = new Vector2(0.2f, 2);
    float cooldownMultiplier = 1;
    float timer = 0;
    float randomCooldown = 1;
    [SerializeField] GameObject sparkleObj;
    [SerializeField] RectTransform backgroundArea;
    [SerializeField] Color sparkleColor;
    List<GameObject> sparkles = new();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > randomCooldown)
        {
            SpawnSparkle();
            randomCooldown = GetCooldown();
            timer = 0;
        }
    }

    void SpawnSparkle()
    {
        GameObject newSparkle = Instantiate(sparkleObj, transform);
        sparkles.Add(newSparkle);
        Rect rect = backgroundArea.rect;
        Vector2 positionInBounds = new Vector2(UnityEngine.Random.Range(rect.min.x, rect.max.x),
            UnityEngine.Random.Range(rect.min.y, rect.max.y));
        newSparkle.GetComponent<RectTransform>().anchoredPosition = positionInBounds;
        float size = UnityEngine.Random.Range(sizeRange.x, sizeRange.y) * sizeMultiplier;
        newSparkle.GetComponent<Image>().color = sparkleColor;

        StartCoroutine(GrowAndShrinkSparkle(newSparkle, size));
    }

    IEnumerator GrowAndShrinkSparkle(GameObject sparkle, float size)
    {
        sparkle.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        while (sparkle.transform.localScale.x < size)
        {
            sparkle.transform.localScale *= 1 + (15 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        while (sparkle.transform.localScale.x > 0.01f)
        {
            sparkle.transform.localScale *= 1 - (15 * Time.deltaTime);
            yield return null;
        }

        sparkles.Remove(sparkle);
        Destroy(sparkle);
    }
    
    void OnDisable()
    {
        foreach (GameObject sparkle in sparkles)
        {
            Destroy(sparkle);
        }
        sparkles.Clear();
    } 
    
    float GetCooldown()
    {
        return UnityEngine.Random.Range(cooldownRange.x, cooldownRange.y) * cooldownMultiplier;
    }
}
