using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public List<ItemEffect> effects;
    public int price = 5;
    public Sprite shopSprite;
}
