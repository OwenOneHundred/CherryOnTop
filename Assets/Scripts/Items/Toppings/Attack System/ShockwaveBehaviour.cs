using UnityEngine;

/// <summary>
/// Handles the behaviour of a Shockwave, which is a GameObject with a spherical hitbox radius which expands at
/// a specified speed. This script should be a component of all Shockwave GameObjects.
/// </summary>
public class ShockwaveBehaviour : MonoBehaviour
{
    [System.NonSerialized] public float range;
    private float lifetime;
    readonly float speed = 2.9f;

    void Start()
    {
        transform.GetChild(0).GetComponent<ShockwaveParticleSystem>().SetUp(range);

        lifetime = (range / speed);
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseRadius();
    }

    private void IncreaseRadius()
    {
        gameObject.GetComponent<SphereCollider>().radius += speed * Time.deltaTime;
    }
}
