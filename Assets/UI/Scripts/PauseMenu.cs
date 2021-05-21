using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    /*
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    private void Start()
    {
        _resumeButton.onClick.AddListener(HandleResumeClicked);
    }

    private void HandleResumeClicked()
    {
        GameManager.Instance.TogglePause();
    }*/
    
    public void TogglePause()
    {
        GameManager.Instance.TogglePause();
    }

    public void ExitToMainMenu()
    {
        GameManager.Instance.ExitToMainMenu();
    }

    public void Accept()
    {
        // TODO Accept function
    }

    public void Cancel()
    {
        // TODO Cancel function
    }
}
