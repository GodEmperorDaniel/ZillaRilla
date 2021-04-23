using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace OldManager
{

    [Serializable]
    public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState>
    {
    }


    public class GameManager : Manager<GameManager>
    {
        public enum GameState
        {
            BOOT,
            MAIN_MENU,
            LOADING,
            IN_GAME,
            PAUSED,
            CUT_SCENE
        }


        // Fields
        public GameObject[] _systemPrefab;
        public EventGameState _onGameStateChanged;

        private List<GameObject> _instancedSystemPrefabs;
        private string _currentLevelName = string.Empty;
        private GameState _currentGameState = GameState.BOOT;

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
            _instancedSystemPrefabs = new List<GameObject>();
            _loadOperations = new List<AsyncOperation>();

            InstantiateSystemPrefabs();
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
            LoadLevel("Main Menu");
            UpdateState(GameState.MAIN_MENU);
        }

        public void StartNewGame()
        {
            UnloadLevel("Main Menu");
            LoadLevel("Test Level 1");
            UpdateState(GameState.IN_GAME);
        }

        public void TogglePause()
        {
            UpdateState(_currentGameState == GameState.IN_GAME ? GameState.PAUSED : GameState.IN_GAME);
        }


        // Internal Methods
        private void UpdateState(GameState newState)
        {
            GameState previousGameState = _currentGameState;
            _currentGameState = newState;

            switch (_currentGameState)
            {
                case GameState.BOOT:
                    Time.timeScale = 1.0f;
                    break;

                case GameState.IN_GAME:
                    Time.timeScale = 1.0f;
                    break;

                case GameState.PAUSED:
                    Time.timeScale = 0.0f;
                    break;

                case GameState.MAIN_MENU:

                    break;

                case GameState.LOADING:

                    break;

                case GameState.CUT_SCENE:

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
                    UpdateState(GameState.IN_GAME);
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
}