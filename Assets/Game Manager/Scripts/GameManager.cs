using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Manager<GameManager>
{
    // TODO: Game States with entry and exit methods

    private StateMachine _gameState;
    
    // Fields
    public GameObject[] _systemPrefab;
    private List<GameObject> _instancedSystemPrefabs;
    private List<AsyncOperation> _loadOperations;
    
    
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

        _gameState = new StateMachine();

        InstantiateSystemPrefabs();
        FindPlayerCharacters();
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
    
    public void GameStartUp()
    {
        //LoadLevel("Main Menu");
        //UpdateState(GameState.MAIN_MENU);
    }
    
    public void StartNewGame()
    {
        UnloadLevel("Main Menu");
        LoadLevel("Test Level 1");
        //UpdateState(GameState.IN_GAME);
        //UpdateObjective();
    }

    public void StartTestLevel()
    {
        
    }
    
    public void TogglePause()
    {
        //UpdateState(_currentGameState == GameState.IN_GAME ? GameState.PAUSED : GameState.IN_GAME);
    }


    // Internal Methods
    private void InstantiateSystemPrefabs()
    {
        foreach (var go in _systemPrefab)
        {
            var prefabInstance = Instantiate(go);
            _instancedSystemPrefabs.Add(prefabInstance);
        }

        _instancedSystemPrefabs.Add(FindObjectOfType<GoalManager>().gameObject);
    }

    private void FindPlayerCharacters()
    {
        _zilla = GameObject.Find("ZillaPlayer").GetComponent<Attackable>();
        _rilla = GameObject.Find("RillaPlayer").GetComponent<Attackable>();

        if (_zilla == null) Debug.LogError("[" + name + "] No reference to Zilla");
        if (_rilla == null) Debug.LogError("[" + name + "] No reference to Rilla");
    }

    private void UpdateHealth()
    {
        //UIManager.Instance.UpdateHealthOnUI();
    }

    public void UpdateObjective(Goal objective)
    {
        _currentObjective = objective;
        UIManager.Instance.UpdateObjectiveOnUI(objective.GoalName, objective.GoalDescription);
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