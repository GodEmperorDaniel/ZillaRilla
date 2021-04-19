using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private PauseMenu _pauseMenu;

    private void Start()
    {
        GameManager.Instance._onGameStateChanged.AddListener(HandleGameStateChange);
    }

    private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
    }
}
