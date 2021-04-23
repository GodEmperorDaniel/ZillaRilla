using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;

    protected override void Awake()
    {
        base.Awake();
        
        _mainMenu.gameObject.SetActive(false);
        _pauseMenu.gameObject.SetActive(false);
    }

    private void Start()
    {
        //GameManager.Instance._onGameStateChanged.AddListener(HandleGameStateChange);
    }

    /*private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
    }*/

    private void ActivateMainMenuUI()
    {
        
    }
    
    private void ActivatePauseMenuUI()
    {
        
    }
    
}
