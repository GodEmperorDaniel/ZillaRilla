using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    //TODO: Button Prompts (Tutorial and such)
    //TODO: News banner
    //TODO: Kill-o-meter for Zilla and Rilla

    [SerializeField] private FillableBar _zillaHealthBar;
    [SerializeField] private FillableBar _rillaHealthBar;
    [SerializeField] private FillableBar _progressBar;
    [SerializeField] private GameObject _currentObjective;


    public void ActivateHealthBars()
    {
        _zillaHealthBar.gameObject.SetActive(true);
        _rillaHealthBar.gameObject.SetActive(true);
    }
    
    public void DeactivateHealthBars()
    {
        _zillaHealthBar.gameObject.SetActive(false);
        _rillaHealthBar.gameObject.SetActive(false);
    }

    public void SetZillaHealthOnUI(float health)
    {
        _zillaHealthBar.fillAmount = health;
    }

    public void SetRillaHealthOnUI(float health)
    {
        _rillaHealthBar.fillAmount = health;
    }

    
    public void DeactivateProgressBar()
    {
        _progressBar.gameObject.SetActive(false);
    }

    public void ActivateProgressBar()
    {
        _progressBar.gameObject.SetActive(true);
    }

    public void SetProgressOnUI(float progress)
    {
        _progressBar.fillAmount = Round(progress, 2);
    }
    
    public void SetObjectiveOnUI(string objectiveName, string objectiveDescription)
    {
        _currentObjective.GetComponent<Text>().text = objectiveDescription;
    }


    // Should be moved to a Math Utility class
    // Rounds float to the amount of digits
    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
    
}