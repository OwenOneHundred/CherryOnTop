using UnityEngine;

/// <summary>
/// Handles the behaviour of a Shockwave, which is a GameObject with a spherical hitbox radius which expands at
/// a specified speed. This script should be a component of all Shockwave GameObjects.
/// </summary>
public class ShockwaveBehaviour : MonoBehaviour
{
    // Represents the rate at which the radius of the Shockwave increases.
    [SerializeField]
    public float shockwaveSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Shockwave manager initialized for shockwave with speed " + this.shockwaveSpeed + ".");
        gameObject.GetComponent<SphereCollider>().radius = 0;
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseRadius();
    }

    private void IncreaseRadius() {
        gameObject.GetComponent<SphereCollider>().radius += Time.deltaTime * shockwaveSpeed;
    }
}
