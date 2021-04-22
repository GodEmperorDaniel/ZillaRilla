using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITester : MonoBehaviour
{
    public InGameUI inGameUI;

    [Range(0, 1)] public float zillaHealth;
    [Range(0, 1)] public float rillaHealth;
    [Range(0, 1)] public float progress;

    private void Update()
    {
        inGameUI.SetZillaHealthOnUI(zillaHealth);
        inGameUI.SetRillaHealthOnUI(rillaHealth);
        
        inGameUI.SetProgressOnUI(progress);
    }
}
