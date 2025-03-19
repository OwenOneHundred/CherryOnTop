using UnityEngine;

public class FondueDebuff : CherryDebuff
{
    public override void EveryFrame()
    {
        
    }

    public override void OnAdded(GameObject cherry)
    {
        this.cherry = cherry;
    }

    public override void OnRemoved(GameObject cherry)
    {
        
    }

    public override void OnCherryDamaged(int damage)
    {
        RemoveSelf();
    }
}
