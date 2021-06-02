using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//TODO: Add Loading Completed state-functionality int enter, exit and OnLoaded transition
//TODO: Maybe rename OnLoaded since we already have OnLoadedComplete

public class GameManager : Manager<GameManager>
{
    public enum GameState
    {
        BOOT,
        CUTSCENE,
        MAIN_MENU,
        CREDITS,
        LOADING,
        LOADING_COMPLETE,
        IN_GAME,
        PAUSED
    }

    // FIELDS
    //public SceneAsset mainLevel;
    //public SceneAsset cutScene;
    //public SceneAsset creditsScene;

    public string mainLevel;
    public string cutscene;
    public string creditsScene;

    public GameObject[] _systemPrefab;
    public GameObject _cutsceneManagerPrefab;
    private List<GameObject> _instancedSystemPrefabs;
    private List<AsyncOperation> _loadOperations;

    private string _currentLevelName = string.Empty;
    private GameState _currentGameState;
    private Goal _currentObjective;


    public Attackable _zilla;
    public Attackable _rilla;

    public List<Transform>
        _attackableCharacters = new List<Transform>(); //THIS IS USED SO THAT ENEMIES DONT ATTACK PLAYERS WHO ARE DOWNED

    public GameState CurrentGameState => _currentGameState;


    // DEBUG
    // TODO Remove this before final build
    public bool startedFromBoot;

    //private void DebugLevelFix()
    /*{
        if (DebugLevelFixer.Instance == null) return;
        if (startedFromBoot)
        {
            Destroy(FindObjectOfType<DebugLevelFixer>().gameObject);
            return;
        }
    
        mainLevel = DebugLevelFixer.LevelNameBuffer;
    }*/

    private void DebugLevelFix()
    {
        string levelBuffer = SceneManager.GetActiveScene().name;
        if (levelBuffer == "Boot") return;
        mainLevel = levelBuffer;
        print("Active level on Boot: " + mainLevel);

        SceneManager.LoadScene("Boot");

        Destroy(FindObjectOfType<GoalManager>().gameObject);
        print("Boot Loaded");
    }


    // UNITY METHODS
    protected override void Awake()
    {
        base.Awake();
        DebugLevelFix();

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();
    }

    private void Start()
    {
        DeactivateAllUI();
        UIManager.Instance.EnableLoadUI();
        IntroCutScene();
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

#region LevelManagement

    // PUBLIC METHODS
    /*Loads scene and the completed event calls the OnLoadComplete
    method when the load operation is completed.
    Loading multiple scenes is possible*/
    public void LoadLevel(string levelName)
    {
        // Controls need to be disabled when loading a level or the event will trigger multiple times
        DisableAllControls();

        UIManager.Instance.EnableLoadUI(); //added in merge!!
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

    /*Unloads scene and the completed event calls the OnUnloadComplete
    method when the unload operation is completed*/
    public void UnloadLevel(string levelName)
    {
        if (!string.IsNullOrEmpty(levelName))
        {
            // Controls might need to be disabled when unloading a level or the event might trigger multiple times
            DisableAllControls();

            AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
            if (ao == null)
            {
                Debug.LogError("[" + name + "] Unable to unload level " + levelName);
                return;
            }

            ao.completed += OnUnloadOperationComplete;
        }
        else
        {
            Debug.Log("Nothing To Unload");
        }
    }

    private void OnLoadOperationComplete(AsyncOperation asyncOperation)
    {
        if (_loadOperations.Contains(asyncOperation))
        {
            _loadOperations.Remove(asyncOperation);

            if (_loadOperations.Count == 0)
            {
                OnLevelLoaded(_currentGameState);
                UIManager.Instance.DisableLoadUI();
            }

            // Dispatch message
            // Transition between scenes
        }

        Debug.Log("Load Complete.");
    }

    private void OnUnloadOperationComplete(AsyncOperation asyncOperation)
    {
        Debug.Log("Unload Complete.");
    }

#endregion

#region Levels

    // SPECIFIC SCENE LOADERS
    public void IntroCutScene()
    {
        LoadLevel(cutscene);
        UpdateState(GameState.CUTSCENE);
    }

    public void Credits()
    {
        UnloadLevel("Main Menu");
        LoadLevel("Credits");
        UpdateState(GameState.CREDITS);
    }

    public void LoadMainMenu()
    {
        UnloadLevel(_currentLevelName);
        LoadLevel("Main Menu");
        UpdateState(GameState.MAIN_MENU);
    }

    public void StartNewGame()
    {
        UnloadLevel("Main Menu");
        LoadLevel(mainLevel);
        UpdateState(GameState.IN_GAME);
    }

    public void ExitToMainMenu()
    {
        _zilla = null;
        _rilla = null;
        DestroyInGameManagers();
        UnloadLevel(_currentLevelName);
        LoadLevel("Main Menu");
        UpdateState(GameState.MAIN_MENU);
    }

#endregion

    public void UpdateObjective(Goal objective)
    {
        _currentObjective = objective;
        UIManager.Instance.UpdateObjectiveOnUI(objective.GoalName, objective.GoalDescription);
    }

    public void TogglePause()
    {
        UpdateState(CurrentGameState == GameState.IN_GAME ? GameState.PAUSED : GameState.IN_GAME);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void DestroyInGameManagers()
    {
        // TODO Check if exists first
        _instancedSystemPrefabs.Remove(GoalManager.Instance.gameObject);
        _instancedSystemPrefabs.Remove(PlayerManager.Instance.gameObject);

        Destroy(GoalManager.Instance.gameObject);
        Destroy(PlayerManager.Instance.gameObject);
    }


    // INTERNAL METHODS
    private void DeactivateAllUI()
    {
        UIManager.Instance.DisableDummyCamera();
        UIManager.Instance.DisableMainMenuUI();
        UIManager.Instance.DisableInGameUI();
        UIManager.Instance.DisablePauseUI();
    }

    private void UpdateState(GameState state)
    {
        ExitCurrentState();
        EnterNewState(state);
    }

#region StateManagement

    private void ExitCurrentState()
    {
        switch (CurrentGameState)
        {
            case GameState.BOOT:
                break;

            case GameState.CUTSCENE:
                UIManager.Instance.DisableDummyCamera();
                _instancedSystemPrefabs.Remove(CutsceneManager.Instance.gameObject);
                Destroy(CutsceneManager.Instance.gameObject);
                break;

            case GameState.MAIN_MENU:
                UIManager.Instance.DisableMainMenuUI();
                break;
            case GameState.CREDITS:
                break;

            case GameState.LOADING:
                break;

            case GameState.LOADING_COMPLETE:
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

        Debug.Log("Exited state: " + CurrentGameState);
    }

    private void EnterNewState(GameState state)
    {
        _currentGameState = state;

        switch (state)
        {
            case GameState.BOOT:

                break;
            case GameState.CUTSCENE:
                UIManager.Instance.EnableDummyCamera();
                _instancedSystemPrefabs.Add(Instantiate(_cutsceneManagerPrefab));
                break;

            case GameState.MAIN_MENU:
                UIManager.Instance.EnableMainMenuUI();
                UIManager.Instance.EnableDummyCamera();
                break;

            case GameState.CREDITS:
                break;

            case GameState.LOADING:
                // Enable loading UI
                UIManager.Instance.EnableDummyCamera();
                break;

            case GameState.LOADING_COMPLETE:
                break;
            case GameState.IN_GAME:
                UIManager.Instance.EnableInGameUI();
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                UIManager.Instance.EnablePauseUI();
                EnableUIControls();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        Debug.Log("Entered state: " + state);
    }

    private void OnLevelLoaded(GameState state)
    {
        FindPlayerCharacters();

        switch (state)
        {
            case GameState.BOOT:
                break;
            case GameState.CUTSCENE:
                EnableUIControls();
                break;
            case GameState.MAIN_MENU:
                EnableUIControls();
                break;
            case GameState.CREDITS:
                EnableUIControls();
                break;
            case GameState.LOADING:
                break;
            case GameState.IN_GAME:
                UIManager.Instance.DisableDummyCamera();
                FindPlayerCharacters();
                InitializeGoalManager();
                EnableInGameControls();
                break;
            case GameState.PAUSED:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

#endregion

    private void InstantiateSystemPrefabs()
    {
        foreach (GameObject go in _systemPrefab)
        {
            GameObject prefabInstance = Instantiate(go);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    private void FindPlayerCharacters()
    {
        if (GameObject.Find("ZillaPlayer"))
        {
            GameObject.Find("ZillaPlayer").TryGetComponent(out _zilla);
            _attackableCharacters.Add(_zilla.transform);
        }

        if (GameObject.Find("RillaPlayer"))
        {
            GameObject.Find("RillaPlayer").TryGetComponent(out _rilla);
            _attackableCharacters.Add(_rilla.transform);
        }

        // Activate the PLayer Manager
        if (_zilla && _rilla)
            PlayerManager.Instance.gameObject.SetActive(true);
    }

    private void InitializeGoalManager()
    {
        GoalManager goalManager = FindObjectOfType<GoalManager>();
        if (goalManager != null)
            _instancedSystemPrefabs.Add(goalManager.gameObject);
    }

    private void EnableUIControls()
    {
        if (_zilla != null || _rilla != null)
        {
            _zilla.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
            _rilla.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        }
        else
        {
            UIManager.Instance.GetComponent<PlayerInput>().enabled = true;
            UIManager.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        }
    }

    private void EnableInGameControls()
    {
        if (_zilla != null || _rilla != null)
        {
            _zilla.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            _rilla.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        }
        else
        {
            UIManager.Instance.GetComponent<PlayerInput>().enabled = false;
        }
    }

    public void DisableAllControls()
    {
        if (_zilla != null || _rilla != null)
        {
            _rilla.GetComponent<PlayerInput>().enabled = false;
            _zilla.GetComponent<PlayerInput>().enabled = false;
        }
        else
        {
            UIManager.Instance.GetComponent<PlayerInput>().enabled = false;
        }
    }

    public void EnableAllControls()
    {
        if (_zilla != null || _rilla != null)
        {
            _rilla.GetComponent<PlayerInput>().enabled = true;
            _zilla.GetComponent<PlayerInput>().enabled = true;
        }
        else
        {
            UIManager.Instance.GetComponent<PlayerInput>().enabled = true;
        }
    }

    //private void UpdateHealth() its an unused private
    //{
    //    //UIManager.Instance.UpdateHealthOnUI();
    //}

    // EVENT METHODS
    // EVENT METHODS

    ////USED TO UPDATE ENEMYS LIST ON WHICH PLAYERS CAN BE ATTACKED!
    //public List<Transform> GetAttackablePlayers()

    //{

    //    return _attackableCharacters;
    //}
}