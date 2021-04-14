using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{

#region Enums

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

#endregion

#region Fields

    public GameObject[] systemPrefab;

    private List<GameObject> _instancedSystemPrefabs;
    private string _currentLevelName = string.Empty;
    private GameState _currentGameState = GameState.PREGAME;

    private List<AsyncOperation> _loadOperations;

#endregion

#region Getters/Setters

    public GameState CurrentGameState
    {
        get => _currentGameState;
        private set => _currentGameState = value;
    }

#endregion

#region Unity Methods

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();
        
        InstantiateSystemPrefabs();
    }

#endregion

#region Public Methods

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

#endregion

#region Internal Methods

    private void UpdateState(GameState state)
    {
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                break;
            
            case GameState.RUNNING:
                break;
            
            case GameState.PAUSED:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void InstantiateSystemPrefabs()
    {
        for (int i = 0; i < systemPrefab.Length; ++i)
        {
            var prefabInstance = Instantiate(systemPrefab[i]);
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

#endregion
    
#region Events
    
    private void OnLoadOperationComplete(AsyncOperation asyncOperation)
    {
        if (_loadOperations.Contains(asyncOperation))
        {
            _loadOperations.Remove(asyncOperation);
            
            // dispatch message
            // transition between scenes
        }
        
        Debug.Log("Load Complete.");
    }
    private void OnUnloadOperationComplete(AsyncOperation asyncOperation)
    {
        Debug.Log("Unload Complete.");

    }
    
#endregion
    
}
