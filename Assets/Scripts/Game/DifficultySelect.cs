using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    [SerializeField] List<Image> images;
    public Difficulty difficulty;

    void Start()
    {
        difficulty = new Easy(1.16f);
    }

    public void PressEasy()
    {
        images[1].color = Color.gray;
        images[2].color = Color.gray;
        difficulty = new Easy(1.14f);
    }

    public void PressMedium()
    {
        images[1].color = Color.white;
        images[2].color = Color.gray;
        difficulty = new Medium(1.2f);
    }

    public void PressHard()
    {
        images[1].color = Color.white;
        images[2].color = Color.white;
        difficulty = new Hard(1.28f);
    }

    public class Difficulty
    {
        public float value = 1.16f;
        public virtual void OnRoundStart()
        {

        }
        public int number = 1;
        public Difficulty(float value)
        {
            this.value = value;
        }
    }

    public class Easy : Difficulty
    {
        public Easy(float value) : base(value)
        {
            number = 1;
        }
    }
    public class Medium : Difficulty
    {
        public Medium(float value) : base(value)
        {
            number = 2;
        }

        public override void OnRoundStart()
        {
            Shop.shop.totalItems = 5;
            //Inventory.inventory.initialMoney = 12;
        }
    }
    public class Hard : Difficulty
    {
        public Hard(float value) : base(value)
        {
            number = 3;
        }

        public override void OnRoundStart()
        {
            Shop.shop.totalItems = 4;
            Shop.shop.columns = 2;
            Shop.shop.rows = 2;
            //Inventory.inventory.initialMoney = 10;
            Shop.shop.Rerolls += 1;
        }
    }
}


