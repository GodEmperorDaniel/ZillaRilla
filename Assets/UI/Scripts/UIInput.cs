using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour, IStartGameInput
{
    public Command startGameInput;
    
    private UITestInput _testingActions;
    
    public bool IsPressingStartGame { get; private set; }

    private void Awake()
    {
        _testingActions = new UITestInput();
    }

    private void OnEnable()
    {
        Debug.Log("Input Enabled");

        _testingActions.Enable();
        _testingActions.UITest.StartGame.performed += OnStartGamePressed;
    }

    private void OnDisable()
    {
        Debug.Log("Input Disabled");

        _testingActions.UITest.StartGame.performed -= OnStartGamePressed;
        _testingActions.Disable();
    }
    

    private void OnStartGamePressed(InputAction.CallbackContext context)
    {
        Debug.Log("StartGame called");

        var value = context.ReadValue<float>();
        IsPressingStartGame = value >= 0.15;
        if (startGameInput != null && IsPressingStartGame)
        {
            startGameInput.Execute();
        }
    }
}
