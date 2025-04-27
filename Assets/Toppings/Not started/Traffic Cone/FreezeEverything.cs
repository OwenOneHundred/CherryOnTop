using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Debuff Everything")]
public class FreezeEverything : EffectSO
{
    [SerializeField] CherryDebuff debuff;
    public override void OnTriggered(IEvent eventObject)
    {
        foreach (CherryMovement cherryMovement in CherryManager.Instance.GetOrderedCherries())
        {
            cherryMovement.transform.root.GetComponent<DebuffManager>().AddDebuff(debuff);
        }
    }
}
