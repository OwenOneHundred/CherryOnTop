using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/ContinuousAttackZone")]
public class CDAttack : ToppingAttack
{
    [Header("Damage is measured in DPS.")]
    [SerializeField] LayerMask cherryLayer;
    [SerializeField] float range = 3;
    [SerializeField] float pierce = 5;
    readonly float timeBetweenAttacks = 0.1f;
    float timer = 0;
    bool cherryInZone = false;
    GameObject vfxObj;
    AudioSource audioSource;
    [SerializeField] bool doOnHitEffects = true;

    public override void OnCycle(GameObject targetedCherry)
    {

    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
        
    }

    public override void OnStart()
    {
        vfxObj = toppingFirePointObj.transform.root.GetChild(2).gameObject;
        audioSource = toppingFirePointObj.transform.root.GetComponent<AudioSource>();
    }

    public override void EveryFrame()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenAttacks)
        {
            Collider[] colliders = Physics.OverlapSphere(toppingFirePointObj.transform.position, range, cherryLayer);

            int count = colliders.Length;

            if (count == 0 && cherryInZone) { OnAllCherriesExit(); }
            else if (count > 0 && !cherryInZone) { OnFirstCherryEnters(); }

            cherryInZone = count > 0;

            int budgetEnum = 0;
            foreach (Collider collider in colliders)
            {
                collider.transform.parent.GetComponent<CherryHitbox>().TakeDamage(
                    damage * timeBetweenAttacks,
                    toppingFirePointObj.transform.root.GetComponent<ToppingObjectScript>().topping,
                    doOnHitEffects ? collider.transform.position - toppingFirePointObj.transform.position : Vector2.zero);
                budgetEnum += 1;
                if (budgetEnum >= pierce)
                {
                    break;
                }
            }
            timer = 0;
        }
        
    }

    protected void OnFirstCherryEnters()
    {
        if (vfxObj != null && audioSource != null)
        {
            audioSource.Play();
            vfxObj.SetActive(true);
        }
    }

    protected void OnAllCherriesExit()
    {
        if (vfxObj != null && audioSource != null)
        {
            audioSource.Stop();
            vfxObj.SetActive(false);
        }
    }
}
