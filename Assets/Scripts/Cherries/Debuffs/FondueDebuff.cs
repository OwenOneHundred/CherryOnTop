using UnityEngine;

[CreateAssetMenu(menuName = "CherryDebuff/Fondue")]
public class FondueDebuff : CherryDebuff
{
    [SerializeField] bool removeOnHit = false;
    [SerializeField] GameObject particleSystemPrefab;
    GameObject psObj;
    public override void EveryFrame()
    {
        
    }

    public override void OnAdded(GameObject cherry)
    {
        this.cherry = cherry;

        psObj = Instantiate(particleSystemPrefab, cherry.transform.position, Quaternion.identity, cherry.transform);
        ParticleSystem.ShapeModule shape = psObj.GetComponent<ParticleSystem>().shape;
        shape.mesh = cherry.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        psObj.transform.localScale = cherry.transform.GetChild(0).lossyScale;
    }

    public override void OnRemoved(GameObject cherry)
    {
        psObj.transform.parent = null;
        psObj.GetComponent<ParticleSystem>().Stop();
        Destroy(psObj, 3);
    }

    public override void OnCherryDamaged(float damage)
    {
        if (removeOnHit) { RemoveSelf(); }
    }
}
