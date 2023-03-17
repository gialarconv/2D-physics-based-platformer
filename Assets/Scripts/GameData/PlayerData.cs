using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string player_name;
    public int coins_amount;

    public GameData()
    {

    }
    public GameData(string name, int coins)
    {
        player_name = name;
        coins_amount = coins;
    }
}
