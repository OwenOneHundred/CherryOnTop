using UnityEngine;

public static class GameInfo
{
    public static int money;
    
    public static RoundState roundState;

    public enum RoundState
    {
        none, cherries, shop 
    }
}
