using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

public class GameScreenController : UiDocumentHUD
{
    private const float DELAY_KEYS = 0.05f;

    private GameItemComponent _gameItemComponent;
    private WaitForSeconds _waitForKeyDelay;

    public GameItemComponent GameItemComponent => _gameItemComponent;


    private void OnEnable()
    {
        GameDelegates.OnAddCoinAmountText += AddCointAmountText;

        GameDelegates.OnGameOver += GameOver;
    }
    private void OnDisable()
    {
        GameDelegates.OnAddCoinAmountText -= AddCointAmountText;

        GameDelegates.OnGameOver -= GameOver;
    }
    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        _gameItemComponent = new GameItemComponent();
        _gameItemComponent.SetVisualElements(_rootScreen);
        RegisterButtonCallbacks();

        ShowVisualElement(_gameItemComponent.pauseScreen, false);
        ShowVisualElement(_gameItemComponent.gameOverScreen, false);

        _gameItemComponent.cointText.text = "0";

        _waitForKeyDelay = new WaitForSeconds(DELAY_KEYS);
    }
    private void AddCointAmountText(int amount)
    {
        _gameItemComponent.cointText.text = $"{amount}";
    }
    #region Visual Behaviours
    private void RegisterButtonCallbacks()
    {
        _gameItemComponent.pauseButton?.RegisterCallback<ClickEvent>(ShowPauseScreen);
        _gameItemComponent.resumeButton?.RegisterCallback<ClickEvent>(ResumeGame);
        _gameItemComponent.quitButton?.RegisterCallback<ClickEvent>(QuitGame);
        _gameItemComponent.gameOverQuitButton?.RegisterCallback<ClickEvent>(QuitGame);
    }

    private void ShowPauseScreen(ClickEvent evt)
    {
        ShowVisualElement(_gameItemComponent.pauseScreen, true);
        ShowVisualElement(_gameItemComponent.gameScreen, false);

        BlurBackground(true);

        GameDelegates.OnGamePaused?.Invoke(1f);
    }
    private void ResumeGame(ClickEvent evt)
    {
        ShowVisualElement(_gameItemComponent.gameScreen, true);
        ShowVisualElement(_gameItemComponent.pauseScreen, false);

        BlurBackground(false);

        GameDelegates.OnGameResumed?.Invoke();
    }
    private void GameOver()
    {
        ShowVisualElement(_gameItemComponent.pauseScreen, false);
        ShowVisualElement(_gameItemComponent.gameScreen, false);
        ShowVisualElement(_gameItemComponent.gameOverScreen, true);

        BlurBackground(true);

        ShowFinalCoins();
    }
    private void ShowFinalCoins()
    {
        _gameItemComponent.finalCoinsAmountText.text = string.Empty;
        StartCoroutine(IEAnimateMessage(GameManager.Instance.CurrentCoinAmount.ToString()));
    }
    private IEnumerator IEAnimateMessage(string message)
    {
        foreach (char c in message.ToCharArray())
        {
            yield return _waitForKeyDelay;
            _gameItemComponent.finalCoinsAmountText.text += c;
        }
    }

    private void QuitGame(ClickEvent evt)
    {
        GameDelegates.OnQuitGame?.Invoke();
    }
    #endregion
}