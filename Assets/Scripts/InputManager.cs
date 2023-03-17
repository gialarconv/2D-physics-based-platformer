using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)] //execute first
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance => _instance;

    public delegate void StartTouchEvent();
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent();
    public event EndTouchEvent OnEndTouch;

    private InputSystem _inputSystem;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (_instance == null)
            _instance = this;

        else if (_instance != this)
            Destroy(gameObject);

        _inputSystem = new InputSystem();
    }
    private void OnEnable()
    {
        if (_inputSystem != null)
        {
            _inputSystem.Enable();
        }
    }
    private void OnDisable()
    {
        if (_inputSystem != null)
        {
            _inputSystem.Disable();
        }
    }
    private void Start()
    {
        _inputSystem.Click.ClickBehaviour.started += context => StartTouch(context);
        _inputSystem.Click.ClickBehaviour.canceled += context => EndTouch(context);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
            OnStartTouch();
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
            OnEndTouch();
    }
}