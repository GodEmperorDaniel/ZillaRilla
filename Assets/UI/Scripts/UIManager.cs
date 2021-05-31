using System;
using System.Collections;
using System.Collections.Generic;
using UI.Main_Menu;
using UI.Scripts;
using UI.Scripts.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private InGameUI _inGameUI;

    [SerializeField] private HitIconSpawner _hitIconSpawner;
    private Camera _dummyCamera;
    private UIInput _uiInput;
    private PlayerInput _playerInput;
    private static PlayOneShot uiSounds;

    public InGameUI InGameUI => _inGameUI;


    public UIInput UIInput
    {
        get => _uiInput;
        private set => _uiInput = value;
    }

    public PlayOneShot UISounds
    {
        get => uiSounds;
        private set => uiSounds = value;
    }

    protected override void Awake()
    {
        base.Awake();

        _dummyCamera = GetComponentInChildren<Camera>();
        _dummyCamera.gameObject.SetActive(false);
        _mainMenu.gameObject.SetActive(false);
        _pauseMenu.gameObject.SetActive(false);
        _uiInput = GetComponent<UIInput>();
        UISounds = GetComponent<PlayOneShot>();
    }


    private void Start()
    {
        //GameManager.Instance._onGameStateChanged.AddListener(HandleGameStateChange);
    }

    /*private void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
    }*/

    public void UpdateObjectiveOnUI(string objectiveName, string objectiveDescription)
    {
        _inGameUI.SetObjectiveOnUI(objectiveName, objectiveDescription);
    }

    public void UpdateProgressionOnUI(float progress)
    {
        _inGameUI.SetProgressOnUI(progress);
    }

    public void UpdateZillaHealthOnUI(float zillaHealth)
    {
        _inGameUI.SetZillaHealthOnUI(zillaHealth);
    }

    public void UpdateRillaHealthOnUI(float rillaHealth)
    {
        _inGameUI.SetRillaHealthOnUI(rillaHealth);
    }

    public void SpawnHitIcon(Vector3 position, int player)
    {
        _hitIconSpawner.SpawnHitIcon(position, player);
    }


    public void EnableUIInput()
    {
        _playerInput.SwitchCurrentActionMap("UI");
    }

    public void DisableUIInput()
    {
        _playerInput.SwitchCurrentActionMap("Blank");
    }

    public void EnableInGameUI()
    {
        _inGameUI.gameObject.SetActive(true);
    }

    public void DisableInGameUI()
    {
        _inGameUI.gameObject.SetActive(false);
    }

    public void EnablePauseUI()
    {
        _pauseMenu.gameObject.SetActive(true);
    }

    public void DisablePauseUI()
    {
        _pauseMenu.gameObject.SetActive(false);
    }

    public void EnableMainMenuUI()
    {
        _mainMenu.gameObject.SetActive(true);
    }

    public void DisableMainMenuUI()
    {
        _mainMenu.gameObject.SetActive(false);
    }

    public void EnableDummyCamera()
    {
        _dummyCamera.gameObject.SetActive(true);
    }

    public void DisableDummyCamera()
    {
        _dummyCamera.gameObject.SetActive(false);
    }

    public void MenuSelection(float navigateDirection)
    {
        GameManager.GameState state = GameManager.Instance.CurrentGameState;
        UISounds.PlaySound("Menu Selection");

        switch (state)
        {
            case GameManager.GameState.BOOT:
                break;
            case GameManager.GameState.CUTSCENE:
                break;
            case GameManager.GameState.MAIN_MENU:
                _mainMenu.MoveArrow(navigateDirection);
                break;
            case GameManager.GameState.LOADING:
                break;
            case GameManager.GameState.IN_GAME:
                break;
            case GameManager.GameState.PAUSED:
                _pauseMenu.MoveArrow(navigateDirection);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void PressAccept()
    {
        GameManager.GameState state = GameManager.Instance.CurrentGameState;

        switch (state)
        {
            case GameManager.GameState.BOOT:
                break;
            case GameManager.GameState.CUTSCENE:
                break;
            case GameManager.GameState.MAIN_MENU:
                Debug.Log("Menu Accept");
                _mainMenu.Accept();
                break;
            case GameManager.GameState.LOADING:
                break;
            case GameManager.GameState.IN_GAME:
                break;
            case GameManager.GameState.PAUSED:
                Debug.Log("Paused Accept");
                _pauseMenu.Accept();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void PressCancel()
    {
        GameManager.GameState state = GameManager.Instance.CurrentGameState;

        switch (state)
        {
            case GameManager.GameState.BOOT:
                break;
            case GameManager.GameState.CUTSCENE:
                break;
            case GameManager.GameState.MAIN_MENU:
                _mainMenu.Cancel();
                break;
            case GameManager.GameState.LOADING:
                break;
            case GameManager.GameState.IN_GAME:
                break;
            case GameManager.GameState.PAUSED:
                _pauseMenu.Cancel();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ActivateBannerRandom(string category)
    {
        _inGameUI.ActivateNewsBannerRandom(category);
    }
    
    public void ActivateBanner(string category, int index)
    {
        _inGameUI.ActivateNewsBanner(category, index);
    }
    
    public void ActivateBanner(string category, string title)
    {
        _inGameUI.ActivateNewsBanner(category, title);
    }
    
}