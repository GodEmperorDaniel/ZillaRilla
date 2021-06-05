using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Main_Menu;
using UI.Scripts;
using UI.Scripts.Input;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private VictoryOrLoseMenu _victoryMenu;
    [SerializeField] private VictoryOrLoseMenu _loseMenu;
    [SerializeField] private InGameUI _inGameUI;
    [SerializeField] private LoadScreen _loadUI;
    [SerializeField] private LoadScreen _controllerScreen;    

    [SerializeField] private HitIconSpawner _hitIconSpawner;
    private Camera _dummyCamera;
    private UIInput _uiInput;
    private PlayerInput _playerInput;
    private static PlayOneShot uiSounds;


    public MainMenu MainMenu => _mainMenu;
    public PauseMenu PauseMenu => _pauseMenu;
    public InGameUI InGameUI => _inGameUI;
    public LoadScreen LoadScreen => _loadUI;
    public Camera DummyCamera => _dummyCamera;

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
        DummyCamera.gameObject.SetActive(false);
        _mainMenu.gameObject.SetActive(false);
        _pauseMenu.gameObject.SetActive(false);
        _uiInput = GetComponent<UIInput>();
        UISounds = GetComponent<PlayOneShot>();
    }

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

    public void EnableVictoryUI()
    {
        _victoryMenu.gameObject.SetActive(true);
    }

    public void DisableVictoryUI()
    {
        _victoryMenu.gameObject.SetActive(false);
    }

    public void EnableLoseUI()
    {
        _loseMenu.gameObject.SetActive(true);
    }

    public void DisableLoseUI()
    {
        _loseMenu.gameObject.SetActive(false);
    }

    public void EnableMainMenuUI()
    {
        _mainMenu.gameObject.SetActive(true);
    }

    public void DisableMainMenuUI()
    {
        _mainMenu.gameObject.SetActive(false);
    }

    public void EnableLoadUI()
    {
        _loadUI.gameObject.SetActive(true);
    }

    public void DisableLoadUI()
    {
        _loadUI.gameObject.SetActive(false);
    }

    public void EnableDummyCamera()
    {
        DummyCamera.gameObject.SetActive(true);
    }

    public void DisableDummyCamera()
    {
        DummyCamera.gameObject.SetActive(false);
    }

    public void EnableControllerScreen()
    {
        _controllerScreen.gameObject.SetActive(true);
    }

    private void DisableControllerScreen()
    {
        Time.timeScale = 1;
        GameManager.Instance.EnableInGameControls();
        _controllerScreen.gameObject.SetActive(false);
    }

    public void MenuSelection(float navigateDirection)
    {
        GameManager.GameState state = GameManager.Instance.CurrentGameState;

        switch (state)
        {
            case GameManager.GameState.BOOT:
                break;
            case GameManager.GameState.CUTSCENE:
                break;
            case GameManager.GameState.MAIN_MENU:
                _mainMenu.MoveArrow(navigateDirection);
                break;
            case GameManager.GameState.CREDITS:

                break;
            case GameManager.GameState.LOADING:
                break;
            case GameManager.GameState.IN_GAME:
                break;
            case GameManager.GameState.PAUSED:
                _pauseMenu.MoveArrow(navigateDirection);
                break;
            case GameManager.GameState.VICTORY:
                _victoryMenu.MoveArrow(navigateDirection);
                break;
            case GameManager.GameState.LOSE:
                _loseMenu.MoveArrow(navigateDirection);
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
                CutsceneManager.Instance.ClipDonePlaying(null);
                break;
            case GameManager.GameState.CREDITS:
                CreditsManager.Instance.SpeedUp();
                break;
            case GameManager.GameState.MAIN_MENU:
                _mainMenu.Accept();
                break;
            case GameManager.GameState.LOADING:
                break;
            case GameManager.GameState.IN_GAME:
                DisableControllerScreen();
                break;
            case GameManager.GameState.PAUSED:
                _pauseMenu.Accept();
                break;
            case GameManager.GameState.VICTORY:
                _victoryMenu.Accept();
                break;
            case GameManager.GameState.LOSE:
                _loseMenu.Accept();
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
            case GameManager.GameState.VICTORY:
                break;
            case GameManager.GameState.LOSE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ActivateBannerRandom(string category, bool forced)
    {
        _inGameUI.ActivateNewsBannerRandom(category, forced);
    }

    public void ActivateBannerRandom(string category, bool forced, float chance)
    {
        _inGameUI.ActivateNewsBannerRandom(category, forced, chance);
    }

    public void ActivateBanner(string category, bool forced)
    {
        _inGameUI.ActivateNewsBanner(category, forced);
    }

    public void ActivateBanner(string category, bool forced, int index)
    {
        _inGameUI.ActivateNewsBanner(category, forced, index);
    }

    public void ActivateBanner(string category, bool forced, string title)
    {
        _inGameUI.ActivateNewsBanner(category, forced, title);
    }
}