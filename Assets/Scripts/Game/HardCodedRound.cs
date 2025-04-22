using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HardCodedRound
{
    public List<SpecialtyCherryAndChance> specialtyCherries;
    public List<SizeAndChance> sizes;
    public int round;
    public float timeBetweenCherriesSeconds = 0.2f;
    public int cherryCount;

    public SpecialtyCherry GetCherryPrefab()
    {
        if (specialtyCherries.Count == 0)
        {
            return null;
        }

        return specialtyCherries[GeneralUtil.RandomWeighted(specialtyCherries.Select(x => x.chance).ToList())].specialtyCherry;
    }

    public CherryTypes.CherrySize GetCherrySize()
    {
        return sizes[GeneralUtil.RandomWeighted(sizes.Select(x => x.chance).ToList())].size;
    }

    [System.Serializable]
    public class SpecialtyCherryAndChance
    {
        public SpecialtyCherry specialtyCherry;
        public float chance;
    }

    [System.Serializable]
    public class SizeAndChance
    {
        public CherryTypes.CherrySize size;
        public float chance;
    }
}
