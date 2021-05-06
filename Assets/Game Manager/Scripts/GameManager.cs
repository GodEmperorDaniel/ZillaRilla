using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameManager : Manager<GameManager>
{
    // TODO: Game States with entry and exit methods

    public enum GameState
    {
        BOOT,
        CUTSCENE,
        MAIN_MENU,
        LOADING,
        IN_GAME,
        PAUSED
    }

    // Fields
    public GameObject[] _systemPrefab;
    private List<GameObject> _instancedSystemPrefabs;
    private List<AsyncOperation> _loadOperations;
    private GameState _gameState;


    private string _currentLevelName = string.Empty;
    private Goal _currentObjective;
    public Attackable _zilla;
    public Attackable _rilla;


    // Getters/Setters


    // Unity Methods
    private void Start()
    {
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();


        InstantiateSystemPrefabs();


        //FindPlayerCharacters(); // Needs to be moved to after level with characters is loaded!
    }


    // Public Methods
    
    
    // Loads scene and the completed event calls the OnLoadComplete
    // method when the load operation is completed.
    // Loading multiple scenes is possible
    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[" + name + "] Unable to load level " + levelName);
            return;
        }

        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);

        _currentLevelName = levelName;
    }

    // Unloads scene and the completed event calls the OnUnloadComplete
    // method when the unload operation is completed
    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[" + name + "] Unable to unload level " + levelName);
            return;
        }

        ao.completed += OnUnloadOperationComplete;
    }

    public void LoadMainMenu()
    {
        LoadLevel("Main Menu");
        UpdateState(GameState.MAIN_MENU);
    }

    public void StartNewGame()
    {
        UnloadLevel("Main Menu");
        LoadLevel("Test Level 1");
        UpdateState(GameState.IN_GAME);
        FindPlayerCharacters();
    }

    public void StartTestLevel()
    {
    }

    public void TogglePause()
    {
        //UpdateState(_currentGameState == GameState.IN_GAME ? GameState.PAUSED : GameState.IN_GAME);
    }

    public void UpdateObjective(Goal objective)
    {
        _currentObjective = objective;
        UIManager.Instance.UpdateObjectiveOnUI(objective.GoalName, objective.GoalDescription);
    }


    // Internal Methods
    private void UpdateState(GameState state)
    {
        ExitCurrentState();
        EnterNewState(state);
    }

    private void ExitCurrentState()
    {
        switch (_gameState)
        {
            case GameState.BOOT:
                break;

            case GameState.CUTSCENE:
                // Disable cutscene UI
                // Disable skip cutscene controls?
                UIManager.Instance.DisableDummyCamera();
                break;

            case GameState.MAIN_MENU:
                UIManager.Instance.DisableMainMenuUI();
                UIManager.Instance.DisableDummyCamera();
                break;

            case GameState.LOADING:
                // Disable loading UI
                UIManager.Instance.DisableDummyCamera();
                break;

            case GameState.IN_GAME:
                UIManager.Instance.DisableInGameUI();
                break;

            case GameState.PAUSED:
                UIManager.Instance.DisablePauseUI();
                Time.timeScale = 1.0f;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void EnterNewState(GameState state)
    {
        _gameState = state;
        
        switch (state)
        {
            case GameState.BOOT:

                break;
            case GameState.CUTSCENE:
                // Enable cutscene UI
                // Enable skip cutscene controls?
                break;

            case GameState.MAIN_MENU:
                UIManager.Instance.EnableMainMenuUI();
                UIManager.Instance.EnableDummyCamera();
                EnableMenuControls();
                break;

            case GameState.LOADING:
                // Enable loading UI
                // Disable all controls
                UIManager.Instance.EnableDummyCamera();
                break;

            case GameState.IN_GAME:
                UIManager.Instance.EnableInGameUI();
                EnableInGameControls();
                break;

            case GameState.PAUSED:
                Time.timeScale = 1.0f;
                UIManager.Instance.EnablePauseUI();
                EnableMenuControls();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void InstantiateSystemPrefabs()
    {
        foreach (var go in _systemPrefab)
        {
            var prefabInstance = Instantiate(go);
            _instancedSystemPrefabs.Add(prefabInstance);
        }

        GoalManager goalManager = FindObjectOfType<GoalManager>();
        if (goalManager != null) _instancedSystemPrefabs.Add(goalManager.gameObject);
    }

    private void FindPlayerCharacters()
    {
        _zilla = GameObject.Find("ZillaPlayer").GetComponent<Attackable>();
        _rilla = GameObject.Find("RillaPlayer").GetComponent<Attackable>();

        if (_zilla == null) Debug.LogError("[" + name + "] No reference to Zilla");
        if (_rilla == null) Debug.LogError("[" + name + "] No reference to Rilla");
    }

    private void EnableMenuControls()
    {
        _zilla.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        _rilla.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
    }

    private void EnableInGameControls()
    {
        _zilla.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        _rilla.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }

    private void UpdateHealth()
    {
        //UIManager.Instance.UpdateHealthOnUI();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < _instancedSystemPrefabs.Count; ++i)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }

        _instancedSystemPrefabs.Clear();
    }


    // Events
    private void OnLoadOperationComplete(AsyncOperation asyncOperation)
    {
        if (_loadOperations.Contains(asyncOperation))
        {
            _loadOperations.Remove(asyncOperation);

            if (_loadOperations.Count == 0)
            {
                //UpdateState(GameState.IN_GAME);
            }


            // dispatch message
            // transition between scenes
        }

        Debug.Log("Load Complete.");
    }

    private void OnUnloadOperationComplete(AsyncOperation asyncOperation)
    {
        Debug.Log("Unload Complete.");
    }
}