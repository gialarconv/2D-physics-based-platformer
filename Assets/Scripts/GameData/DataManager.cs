using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance => _instance;

    [SerializeField] private string _fileName;

    private GameData _gameData;
    private string _fullPath;
    //if true data will be encripted with an XOR function
    private bool encrypt = false;

    public GameData PlayerData => _gameData;

    [InspectorButton("Save Data", nameof(SaveTest))]
    public bool _saveButton;
    [InspectorButton("Load Data", nameof(LoadGameValues))]
    public bool _loadButton;
    [InspectorButton("Clear Data", nameof(ClearData))]
    public bool _clearButton;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (_instance == null)
            _instance = this;

        else if (_instance != this)
            Destroy(gameObject);

        //the filename where saved data will be stored
        _fullPath = Application.persistentDataPath + "/" + _fileName;

        _gameData = new GameData();
    }
    private void OnEnable()
    {
        GameDelegates.OnLoadData += LoadGameValues;
        GameDelegates.OnSaveData += SaveGameValues;
    }
    private void OnDisable()
    {
        GameDelegates.OnLoadData -= LoadGameValues;
        GameDelegates.OnSaveData -= SaveGameValues;
    }
    void Start()
    {
        LoadGameValues();
    }

    private void LoadGameValues()
    {
        SaveManager.Instance.Load<GameData>(_fullPath, DataWasLoaded, encrypt);
    }

    private void DataWasLoaded(GameData data, SaveResult result, string message)
    {
        if (result == SaveResult.EmptyData || result == SaveResult.Error)
        {
            data = new GameData();
        }
        if (result == SaveResult.Success)
        {
            _gameData = data;
        }
    }

    private void SaveGameValues(GameData playerData)
    {
        if (string.IsNullOrEmpty(_gameData.player_name))
        {
            _gameData.player_name = playerData.player_name;
        }
        _gameData.coins_amount = playerData.coins_amount;

        SaveManager.Instance.Save(_gameData, _fullPath, DataWasSaved, encrypt);
    }
    private void DataWasSaved(SaveResult result, string message)
    {
        Debug.Log($"Data Was Saved - Result: {result}, message {message}");
        if (result == SaveResult.Error)
        {
            Debug.Log($"Error saving data");
        }
    }

    public string GetPlayerName()
    {
        return _gameData.player_name;
    }
    public int GetPlayerCoins()
    {
        return _gameData.coins_amount;
    }

    public void SaveTest()
    {
        GameData gameData = new GameData("Cheqcoslov", 7);

        SaveGameValues(gameData);
    }
    public void ClearData()
    {
        Debug.Log($"Data was clear");
        SaveManager.Instance.ClearFIle(_fullPath);
    }
}