using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{


    [SerializeField] private GameScreenController _gameScreenController;
    [SerializeField] private float _timeAmount;

    private bool _timerIsRunning = false;
    private float _timeRemaining;
    private void OnEnable()
    {
        GameDelegates.OnInitTimer += InitTimer;
    }
    private void OnDisable()
    {
        GameDelegates.OnInitTimer -= InitTimer;
    }
    private void Update()
    {
        Timer();
    }

    public void InitTimer()
    {
        this.DoAfter(() => _gameScreenController.GameItemComponent != null, () =>
        {
            _timeRemaining = _timeAmount;
            _timerIsRunning = true;
        });
    }

    public void Timer()
    {
        if (_timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                DiplayTime(_timeRemaining);
            }
            else
            {
                EndTimer();
            }
        }
    }

    private void EndTimer()
    {
        _timeRemaining = 0;
        _timerIsRunning = false;

        GameDelegates.OnGamePaused?.Invoke(1f);
        GameDelegates.OnGameOver?.Invoke();
    }

    public void DiplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        _gameScreenController.GameItemComponent.timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}