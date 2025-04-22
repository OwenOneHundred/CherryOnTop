using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    [SerializeField] Image cup;
    [System.NonSerialized] public Difficulty difficulty;
    List<Difficulty> difficulties = new();
    public List<Sprite> cupSprites;
    [SerializeField] AudioFile cupFillSound;
    [SerializeField] TMPro.TextMeshProUGUI difficultyName;

    void Start()
    {
        difficulties.Add(new Easy(1.12f));
        difficulties.Add(new Medium(1.18f));
        difficulties.Add(new Hard(1.24f));
        difficulties.Add(new Impossible(1.265f));
        difficulty = difficulties[0];
    }

    int difficultyIndex = 0;

    public void Click()
    {
        difficultyIndex += 1;
        if (difficultyIndex >= difficulties.Count)
        {
            difficultyIndex = 0;
        }
        else
        {
            SoundEffectManager.sfxmanager.PlayOneShotWithPitch(cupFillSound, 1 + (0.25f * (difficultyIndex - 1)));
        }
        difficulty = difficulties[difficultyIndex];
        difficultyName.text = difficulties[difficultyIndex].name;

        cup.sprite = cupSprites[difficultyIndex];
    }

    [System.Serializable]
    public class Difficulty
    {
        public float value = 1.12f;
        public string name;
        public virtual void OnRoundStart()
        {

        }
        public int number = 1;
        public Difficulty(float value)
        {
            this.value = value;
        }
    }
    [System.Serializable]
    public class Easy : Difficulty
    {
        public Easy(float value) : base(value)
        {
            number = 1;
            name = "Easy";
        }
    }
    [System.Serializable]
    public class Medium : Difficulty
    {
        public Medium(float value) : base(value)
        {
            number = 2;
            name = "Medium";
        }

        public override void OnRoundStart()
        {
            Shop.shop.totalItems = 5;
            Inventory.inventory.initialMoney = 15;
        }
    }
    [System.Serializable]
    public class Hard : Difficulty
    {
        public Hard(float value) : base(value)
        {
            number = 3;
            name = "Hard";
        }

        public override void OnRoundStart()
        {
            Shop.shop.totalItems = 4;
            Shop.shop.columns = 2;
            Shop.shop.rows = 2;
            Inventory.inventory.initialMoney = 14;
            Shop.shop.Rerolls += 1;
        }
    }
    [System.Serializable]
    public class Impossible : Difficulty
    {
        public Impossible(float value) : base(value)
        {
            number = 4;
            name = "Impossible";
        }

        public override void OnRoundStart()
        {
            Shop.shop.totalItems = 3;
            Shop.shop.columns = 2;
            Shop.shop.rows = 2;
            Inventory.inventory.initialMoney = 12;
            Shop.shop.Rerolls += 1;
            RoundManager.roundManager.moneyOnRoundEnd -= 1;
        }
    }
}


