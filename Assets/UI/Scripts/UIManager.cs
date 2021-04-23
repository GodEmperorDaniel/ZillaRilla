using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Object = System.Object;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private InGameUI _inGameUI;

    public InGameUI InGameUI => _inGameUI;

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
    
    public void UpdateObjectiveOnUI(Objective objective)
    {
        _inGameUI.SetObjective(objective);
    }
    
    

    private void ActivateMainMenuUI()
    {
        
    }
    
    private void ActivatePauseMenuUI()
    {
        
    }
    
    private void ActivateInGameUI()
    {
        
    }
    
}
