using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;



    [SerializeField] private GameSpawner _gameSpawner;

    [Range(0f, 100f)]
    [SerializeField] private float _coinChestPercentage;
    [SerializeField] private float _timeToRespawnCoins;
    [SerializeField] private float _timeToRespawnChests;
    [SerializeField] private int _maxChestsToCreate = 10;

    private int _currentCoinAmount;
    private int _randomNormalCointsToRespawn = 10;

    public float TimeToRespawnCoins => _timeToRespawnCoins;
    public float TimeToRespawnChests => _timeToRespawnChests;
    public int MaxChestsToCreate => _maxChestsToCreate;
    public int RandomNormalCointsToRespawn => _randomNormalCointsToRespawn;
    public int CurrentCoinAmount => _currentCoinAmount;

    private void Awake()
    {
        _instance = this;
        // if (_instance == null)
        //     _instance = this;

        // else if (_instance != this)
        //     Destroy(gameObject);
    }
    private void OnEnable()
    {
        GameDelegates.OnAddCoinAmount += AddCoinAmount;
        GameDelegates.OnAddCoinPercentage += AddCoinPercentage;
        GameDelegates.OnGamePaused += GamePaused;
        GameDelegates.OnGameResumed += GameResumed;
        GameDelegates.OnQuitGame += QuitGame;

        GameDelegates.OnGameOver += GameOver;
    }
    private void OnDisable()
    {
        GameDelegates.OnAddCoinAmount -= AddCoinAmount;
        GameDelegates.OnAddCoinPercentage -= AddCoinPercentage;
        GameDelegates.OnGamePaused -= GamePaused;
        GameDelegates.OnGameResumed -= GameResumed;
        GameDelegates.OnQuitGame -= QuitGame;

        GameDelegates.OnGameOver -= GameOver;
    }
    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        //reset timescale
        Time.timeScale = 1f;
        //init game 
        _gameSpawner.InitGameSpawner();
        //init character
        GameDelegates.OnInitCharacterSpawner?.Invoke();
        //init timer
        GameDelegates.OnInitTimer?.Invoke();
    }
    private void AddCoinPercentage()
    {
        float percentageAmount = _currentCoinAmount * (_coinChestPercentage / 100);
        _currentCoinAmount += Mathf.CeilToInt(percentageAmount);
        GameDelegates.OnAddCoinAmountText?.Invoke(_currentCoinAmount);
    }

    private void AddCoinAmount(int amount)
    {
        _currentCoinAmount += amount;
        GameDelegates.OnAddCoinAmountText?.Invoke(_currentCoinAmount);
    }

    private void GamePaused(float delay)
    {
        StopAllCoroutines();
        StartCoroutine(PauseGameTime(delay));
    }

    private void GameResumed()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
    }
    private IEnumerator PauseGameTime(float delay = 2f)
    {
        float pauseTime = Time.time + delay;
        float decrement = (delay > 0) ? Time.deltaTime / delay : Time.deltaTime;

        while (Time.timeScale > 0.1f || Time.time < pauseTime)
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale - decrement, 0f, Time.timeScale - decrement);
            yield return null;
        }

        // ramp the timeScale down to 0
        Time.timeScale = 0f;
    }
    private void QuitGame()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
    private void GameOver()
    {
        if (DataManager.Instance == null)
            return;

        if (DataManager.Instance.GetPlayerCoins() < _currentCoinAmount)
        {
            GameData gameData = new GameData(DataManager.Instance.GetPlayerName(), _currentCoinAmount);
            GameDelegates.OnSaveData?.Invoke(gameData);
        }
        else
        {
            Debug.Log("Doesnt need to save data.");
        }

    }
}