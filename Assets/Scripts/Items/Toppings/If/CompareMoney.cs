using UnityEngine;

[CreateAssetMenu(menuName ="Toppings/If/CompareMoney")]
public class CompareMoney : ToppingIf
{
    [Header("If money is ___ ___, operate")]
    [SerializeField] private Symbol symbol;
    [SerializeField] int value;
    
    public override bool Evaluate()
    {
        if (symbol == Symbol.greater)
        {
            return GameInfo.money > value;
        }
        else if (symbol == Symbol.less)
        {
            return GameInfo.money < value;
        }
        else
        {
            return GameInfo.money == value;
        }
    }

    [System.Serializable]
    private enum Symbol
    {
        greater, less, equals
    }
}
