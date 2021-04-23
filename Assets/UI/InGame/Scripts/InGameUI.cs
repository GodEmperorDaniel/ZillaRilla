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

    [SerializeField] private FillableBar zillaHealthBar;
    [SerializeField] private FillableBar rillaHealthBar;
    [SerializeField] private FillableBar progressBar;
    [SerializeField] private GameObject _currentObjective;


    public void ActivateHealthBars()
    {
        zillaHealthBar.gameObject.SetActive(true);
        rillaHealthBar.gameObject.SetActive(true);
    }
    
    public void DeactivateHealthBars()
    {
        zillaHealthBar.gameObject.SetActive(false);
        rillaHealthBar.gameObject.SetActive(false);
    }

    public void SetZillaHealthOnUI(float health)
    {
        zillaHealthBar.fillAmount = health;
    }

    public void SetRillaHealthOnUI(float health)
    {
        rillaHealthBar.fillAmount = health;
    }

    
    public void DeactivateProgressBar()
    {
        progressBar.gameObject.SetActive(false);
    }

    public void ActivateProgressBar()
    {
        progressBar.gameObject.SetActive(true);
    }

    public void SetProgressOnUI(float progress)
    {
        progressBar.fillAmount = progress;
    }


    public void SetObjectiveOnUI(string objectiveName, string objectiveDescription)
    {
        _currentObjective.GetComponent<Text>().text = objectiveDescription;
    }
    
}