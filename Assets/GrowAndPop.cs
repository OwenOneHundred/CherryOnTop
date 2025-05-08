using Unity.Mathematics;
using UnityEngine;

public class GrowAndPop : MonoBehaviour
{
    [SerializeField] float growTime = 0.2f;
    float growTimer = 0;
    [SerializeField] float maxSizeMultiplier = 1.5f;
    [SerializeField] protected AudioFile deathSound;
    [SerializeField] GameObject deathPS;
    Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        growTimer += Time.deltaTime;
        if (growTimer > growTime)
        {
            Die();
        }
        else
        {
            transform.localScale = initialScale * Mathf.Lerp(1, maxSizeMultiplier, growTimer / growTime);
        }
    }

    void Die()
    {
        SoundEffectManager.sfxmanager.PlayOneShot(deathSound);
        Destroy(gameObject);
        Destroy(Instantiate(deathPS, transform.position, Quaternion.identity), 1.25f);
    }
}
