using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDelegates
{
    #region Data storage
    public static System.Action OnLoadData;
    public static System.Action<GameData> OnSaveData;
    #endregion

    #region Coins Beheviour
    public static System.Action<int> OnAddCoinAmountText;
    public static System.Action<Vector3> OnPlayPooledFX;
    #endregion

    #region Game Timer
    public static System.Action OnInitTimer;
    #endregion

    #region Character Spawner
    public static System.Action OnInitCharacterSpawner;
    #endregion

    #region Game Manager
    public static System.Action<int> OnAddCoinAmount;
    public static System.Action OnAddCoinPercentage;
    public static System.Action<float> OnGamePaused;
    public static System.Action OnGameResumed;
    public static System.Action OnQuitGame;
    public static System.Action OnGameOver;
    #endregion
}
