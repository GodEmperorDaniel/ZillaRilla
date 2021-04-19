using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private UIInput _uiInput;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;

    private void Start()
    {
        GameManager.Instance._onGameStateChanged.AddListener(HandleGameStateChange);
    }

    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
    }

    private void ActivateMainMenuUI()
    {
        
    }
    
    private void ActivatePauseMenuUI()
    {
        
    }
    
}
