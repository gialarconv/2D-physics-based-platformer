using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuItemComponent
{
    private const string QUIT_GAME_BUTTON = "quit_game_Key";
    private const string PLAY_BUTTON = "play_Key";
    private const string SAVE_NAME_BUTTON = "save_name_Key";
    private const string COIN_TEXT = "coin_Key";
    private const string WELCOME_TEXT = "welcome_Key";
    private const string PLAYER_NAME_FIELD = "player_text_field";
    private const string MAIN_PANEL = "main-panel";
    private const string WELCOME_PANEL = "welcome-panel";

    public VisualElement mainPanel;
    public VisualElement welcomePanel;

    public Button quitGameButton;
    public Button playButton;
    public Button saveNameButton;

    public Label coinText;
    public Label welcomeText;

    public TextField playerTextfield;

    public void SetVisualElements(VisualElement visualElement)
    {
        mainPanel = visualElement.Q(MAIN_PANEL);
        welcomePanel = visualElement.Q(WELCOME_PANEL);

        quitGameButton = visualElement.Q<Button>(QUIT_GAME_BUTTON);
        playButton = visualElement.Q<Button>(PLAY_BUTTON);
        saveNameButton = visualElement.Q<Button>(SAVE_NAME_BUTTON);

        coinText = visualElement.Q<Label>(COIN_TEXT);
        welcomeText = visualElement.Q<Label>(WELCOME_TEXT);

        playerTextfield = visualElement.Q<TextField>(PLAYER_NAME_FIELD);
    }
}