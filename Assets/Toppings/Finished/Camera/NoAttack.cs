using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/NoAttack")]
public class NoAttack : ToppingAttack
{
    public override void OnCycle(GameObject targetedCherry)
    {
        
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
        
    }

    public override void OnStart()
    {
        
    }
}
