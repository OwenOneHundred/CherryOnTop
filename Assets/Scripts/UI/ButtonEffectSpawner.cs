using UnityEngine;

public class ButtonEffectSpawner : MonoBehaviour
{
    [SerializeField] GameObject effectPrefab;
    [SerializeField] float lifetime = 4;
    [SerializeField] float scaleMultiplier = 1;
    public void SpawnEffect()
    {
        Vector3 worldPoint = GetComponent<RectTransform>().position;
        worldPoint += Camera.main.transform.forward * 0.01f;
        GameObject newEffect = Instantiate(effectPrefab, worldPoint, Quaternion.identity, null);
        Destroy(newEffect, lifetime);
        newEffect.transform.localScale *= scaleMultiplier;
    }
}
