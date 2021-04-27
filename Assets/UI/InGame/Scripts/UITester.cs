using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITester : MonoBehaviour
{
    [Range(0, 1)] public float zillaHealth;
    [Range(0, 1)] public float rillaHealth;
    [Range(0, 1)] public float progress;

    private void Update()
    {
        UIManager.Instance.InGameUI.SetZillaHealthOnUI(zillaHealth);
        UIManager.Instance.InGameUI.SetRillaHealthOnUI(rillaHealth);
        UIManager.Instance.InGameUI.SetProgressOnUI(progress);
    }
}
