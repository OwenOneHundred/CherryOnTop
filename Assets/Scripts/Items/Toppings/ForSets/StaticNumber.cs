using UnityEngine;

[CreateAssetMenu(menuName ="Toppings/ForSet/StaticNumber")]
public class StaticNumber : ToppingForSet
{
    [SerializeField] int number;

    public override ForSetData GetSet()
    {
        return new ForSetData(number);
    }
}
