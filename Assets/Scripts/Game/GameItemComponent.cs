using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameItemComponent
{
    private const string PAUSE_BUTTON = "pause_Key";
    private const string RESUME_BUTTON = "resume_Key";
    private const string QUIT_BUTTON = "quit_Key";
    private const string GAME_OVER_QUIT_BUTTON = "gameOver_quit_Key";

    private const string COIN_TEXT = "coin_Key";
    private const string TIMER_TEXT = "timer_key";
    private const string FINAL_COIN_AMOUNT = "final_coins_key";

    private const string GAME_SCREEN = "game-panel";
    private const string PAUSE_SCREEN = "pause-panel";
    private const string GAME_OVER_SCREEN = "gameOver-panel";

    public Button pauseButton;
    public Button resumeButton;
    public Button quitButton;
    public Button gameOverQuitButton;

    public Label cointText;
    public Label timerText;
    public Label finalCoinsAmountText;

    public VisualElement gameScreen;
    public VisualElement pauseScreen;
    public VisualElement gameOverScreen;

    public void SetVisualElements(VisualElement visualElement)
    {
        pauseButton = visualElement.Q<Button>(PAUSE_BUTTON);
        resumeButton = visualElement.Q<Button>(RESUME_BUTTON);
        quitButton = visualElement.Q<Button>(QUIT_BUTTON);
        gameOverQuitButton = visualElement.Q<Button>(GAME_OVER_QUIT_BUTTON);

        cointText = visualElement.Q<Label>(COIN_TEXT);
        timerText = visualElement.Q<Label>(TIMER_TEXT);
        finalCoinsAmountText = visualElement.Q<Label>(FINAL_COIN_AMOUNT);

        gameScreen = visualElement.Q(GAME_SCREEN);
        pauseScreen = visualElement.Q(PAUSE_SCREEN);
        gameOverScreen = visualElement.Q(GAME_OVER_SCREEN);
    }
}