using UnityEngine;

public class ShockwaveParticleSystem : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.MainModule main;
    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
    }

    public void SetUp(float range)
    {
        main.startLifetime = range / 3f;
        ps.Play();
    }
}
