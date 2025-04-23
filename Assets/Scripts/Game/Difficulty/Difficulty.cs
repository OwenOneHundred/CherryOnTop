using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Difficulty : ScriptableObject
{
    public float cherryAmountScaleValue = 1.12f;
    public float cherrySizeScaleValue = 1.12f;
    public virtual void OnRoundStart() { }
    public int number = 1;
    [SerializeField] List<HardCodedRound> hardCodedRounds;
    public bool TryGetHardCodedRound(int round, out HardCodedRound hardCodedRound)
    {
        hardCodedRound = hardCodedRounds.FirstOrDefault(x => x.round == round);
        return hardCodedRound != null;
    }
}