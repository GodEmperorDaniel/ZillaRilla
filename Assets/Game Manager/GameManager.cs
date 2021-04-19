using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


[Serializable]
public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState>
{
}

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }


    // Fields
    public GameObject[] _systemPrefab;
    public EventGameState _onGameStateChanged;

    private List<GameObject> _instancedSystemPrefabs;
    private string _currentLevelName = string.Empty;
    private GameState _currentGameState = GameState.PREGAME;

    private List<AsyncOperation> _loadOperations;

    
    // Getters/Setters
    public GameState CurrentGameState
    {
        get => _currentGameState;
        private set => _currentGameState = value;
    }
    

    // Unity Methods
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();
        
        //CreateDebugCube();
    }

    
    // Public Methods
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
    public void UnloadLevel(int levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[" + name + "] Unable to unload level " + levelName);
            return;
        }

        ao.completed += OnUnloadOperationComplete;
    }
    public void StartGame()
    {
        LoadLevel("TestScene1");
    }
    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }
    
    
    // Internal Methods
    private void UpdateState(GameState newState)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = newState;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        // Dispatch Messages
        _onGameStateChanged.Invoke(_currentGameState, previousGameState);
    }
    private void InstantiateSystemPrefabs()
    {
        for (int i = 0; i < _systemPrefab.Length; ++i)
        {
            var prefabInstance = Instantiate(_systemPrefab[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
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
                UpdateState(GameState.RUNNING);
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
    
    
    // >DEBUG<
    /*public GameObject dRotatingCube; 
    
    private void CreateDebugCube()
    {
        Instantiate(dRotatingCube);
    }*/
}