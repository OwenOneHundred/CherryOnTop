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

    public void SetUp(float range, float speed)
    {
        main.startSpeed = speed;
        main.startLifetime = range / speed;
        ps.Play();
    }
}
