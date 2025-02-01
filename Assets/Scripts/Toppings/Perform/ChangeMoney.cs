using UnityEngine;

[CreateAssetMenu(menuName ="Toppings/Perform/ChangeMoney")]
public class ChangeMoney : Perform
{
    [SerializeField] int amountToChangeMoney = 5;

    public override void Execute(ForSetData forSetData)
    {
        int count = forSetData.GetCount();
        for (int i = 0; i < count; i++)
        {
            GameInfo.money += amountToChangeMoney;
        }
    }
}