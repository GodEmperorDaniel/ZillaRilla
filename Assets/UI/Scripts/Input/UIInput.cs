using System;
using System.Collections;
using System.Collections.Generic;
using UI.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour, IStartGameInput, ILogState, IPauseGame
{
    public Command startGameInput;
    public Command logStateInput;
    public Command pauseGameInput;

    private UITestInput _testingActions;

    public bool IsPressingStartGame { get; private set; }
    public bool IsPressingLogState { get; private set; }
    public bool IsPressingPauseGame { get; private set; }


    private void Awake()
    {
        _testingActions = new UITestInput();
    }

    private void OnEnable()
    {
        //Debug.Log("Input Enabled");

        _testingActions.Enable();
        _testingActions.UITest.StartGame.performed += OnStartGamePressed;
        _testingActions.UITest.LogState.performed += OnLogStatePressed;
        _testingActions.UITest.PauseGame.performed += OnPauseGamePressed;
    }

    private void OnDisable()
    {
        //Debug.Log("Input Disabled");

        _testingActions.UITest.StartGame.performed -= OnStartGamePressed;
        _testingActions.UITest.LogState.performed -= OnLogStatePressed;
        _testingActions.UITest.PauseGame.performed -= OnPauseGamePressed;
        _testingActions.Disable();
    }


    private void OnStartGamePressed(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        IsPressingStartGame = value >= 0.15;
        if (startGameInput != null && IsPressingStartGame)
        {
            startGameInput.Execute();
        }
    }
    
    private void OnLogStatePressed(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        IsPressingLogState = value >= 0.15;
        if (logStateInput != null && IsPressingLogState)
        {
            logStateInput.Execute();
        }
    }

    private void OnPauseGamePressed(InputAction.CallbackContext context)
    {
        Debug.Log("Pause Pressed");
        
        var value = context.ReadValue<float>();
        IsPressingPauseGame = value >= 0.15;
        if (pauseGameInput != null && IsPressingPauseGame)
        {
            pauseGameInput.Execute();
        }
    }
}
