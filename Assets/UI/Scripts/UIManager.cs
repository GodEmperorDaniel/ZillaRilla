using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private InGameUI _inGameUI;

    [SerializeField] private HitIconSpawner _hitIconSpawner;
    private Camera _dummyCamera;

    public InGameUI InGameUI => _inGameUI;

    protected override void Awake()
    {
        base.Awake();

        _dummyCamera = Instantiate(new GameObject("Dummy Camera"), transform).AddComponent<Camera>();
        _dummyCamera.gameObject.SetActive(false);
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

    public void SpawnHitIcon(Vector3 position)
    {
        _hitIconSpawner.SpawnHitIcon(position);
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
}