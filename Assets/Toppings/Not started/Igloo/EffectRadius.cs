using UnityEngine;

public class EffectRadius : MonoBehaviour
{
    public CherryDebuff cherryDebuff;
    public SphereCollider coll;
    [SerializeField] TargetingSystem targetingSystem;

    void Start()
    {
        cherryDebuff = CherryDebuff.CreateInstance(cherryDebuff);
        coll.radius = targetingSystem.GetRange();
    }

    public void OnTriggerEnter(Collider other)
    {
        coll.radius = targetingSystem.GetRange();
        if (other.transform.root.TryGetComponent<DebuffManager>(out DebuffManager debuffManager))
        {
            debuffManager.AddDebuff(cherryDebuff);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.root.TryGetComponent<DebuffManager>(out DebuffManager debuffManager))
        {
            debuffManager.RemoveDebuff(cherryDebuff);
        }
    }
}
