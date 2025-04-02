using System;
using UnityEngine;

public static class ToppingTypes
{
    // Any topping can include any number of tags. "| type" means "and all objects of this type are also this other type".
    // candle = 1 << 0 | hot, and therefore all candles are hot.
    // numberCandle = 1 << 1 | candle, and therefore all numberCandles are candles.
    // numberCandles are also all hot, because candles are.
    // 
    // Check if a ToppingTypes.Type value includes a flag (this checks for hot) like this: ToppingTypes.Flags.HasFlag(ToppingTypes.Flags.hot)
    // 
    // There can only be 32 tags, because data is stored in an int. We should not need more than that.

    /// <summary>
    /// Tags for toppings.
    /// </summary>
    [Flags]
    [System.Serializable]
    public enum Flags
    {
        none            = 0,
        cold            = 1 << 0,
        hot             = 1 << 1,
            candle          = 1 << 2 | hot,
                numberCandle    = 1 << 3 | candle,
        produce         = 1 << 4,
            vegetable       = 1 << 5 | produce,
            fruit           = 1 << 6 | produce,
        sweet           = 1 << 7,
            candy           = 1 << 8 | sweet,
        figurine        = 1 << 9,
        decoration      = 1 << 10,
        electronics     = 1 << 11,
        animal          = 1 << 12,
            ocean           = 1 << 13 | animal,
            bird            = 1 << 14 | animal,
    }

    public static bool HasAny(this ToppingTypes.Flags value, ToppingTypes.Flags any)
        => (value & any) != 0;
    
    public static float GetWeight(this ToppingTypes.Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.common: return 1f;
            case Rarity.uncommon: return 0.25f;
            case Rarity.rare: return 0.1f;
            default: return 1;
        }
    }
        

    [System.Serializable]
    public enum Rarity
    {
        common, uncommon, rare
    }
}