using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : UiDocumentHUD
{
    private MenuItemComponent _menuItemComponent;

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        _menuItemComponent = new MenuItemComponent();
        _menuItemComponent.SetVisualElements(_rootScreen);

        ReplacePlayerName();
        ReplaceCoinsAmount();

        RegisterCallbacks();

        ShowVisualElement(_menuItemComponent.welcomePanel, false);

        this.DoAfter(() => DataManager.Instance.PlayerData != null, () =>
        {
            if (string.IsNullOrEmpty(DataManager.Instance.PlayerData.player_name))
            {
                ShowVisualElement(_menuItemComponent.mainPanel, false);
                ShowVisualElement(_menuItemComponent.welcomePanel, true);
                BlurBackground(true);
            }
        });
    }
    private void RegisterCallbacks()
    {
        _menuItemComponent.quitGameButton?.RegisterCallback<ClickEvent>(QuitGame);
        _menuItemComponent.playButton?.RegisterCallback<ClickEvent>(PlayGame);
        _menuItemComponent.saveNameButton?.RegisterCallback<ClickEvent>(SaveName);
        _menuItemComponent.playerTextfield?.RegisterCallback<KeyDownEvent>(SetPlayerTextfield);
    }

    private void QuitGame(ClickEvent evt)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void ReplacePlayerName()
    {
        string userName = _menuItemComponent.welcomeText.text.Replace("<>", DataManager.Instance.GetPlayerName());
        _menuItemComponent.welcomeText.text = userName;
    }
    private void ReplaceCoinsAmount()
    {
        _menuItemComponent.coinText.text = $"{DataManager.Instance.GetPlayerCoins()}";
    }
    private void PlayGame(ClickEvent evt)
    {
        SceneManager.LoadSceneAsync("Main");
    }
    private void SaveName(ClickEvent evt)
    {
        if (DataManager.Instance != null)
        {
            GameDelegates.OnSaveData?.Invoke(new GameData(_menuItemComponent.playerTextfield.text, 0));
            BlurBackground(false);

            ShowVisualElement(_menuItemComponent.welcomePanel, false);
            ShowVisualElement(_menuItemComponent.mainPanel, true);

            _menuItemComponent.welcomeText.text = $"Welcome {_menuItemComponent.playerTextfield.text}";
        }
    }
    private void SetPlayerTextfield(KeyDownEvent evt)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE
        if (evt.keyCode == KeyCode.Return && DataManager.Instance != null)
        {
            GameDelegates.OnSaveData?.Invoke(new GameData(_menuItemComponent.playerTextfield.text, 0));
            BlurBackground(false);
        }
#endif
    }
}