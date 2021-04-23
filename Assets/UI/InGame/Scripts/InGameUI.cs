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

    private Text currentObjective;


    public void SetObjective(Objective objective)
    {
        currentObjective.text = objective.ObjectiveDescription;
    }
    
    public void ActivateProgressBar()
    {
        progressBar.gameObject.SetActive(true);
    }

    public void ActivateHealthBars()
    {
        zillaHealthBar.gameObject.SetActive(true);
        rillaHealthBar.gameObject.SetActive(true);
    }

    public void DeactivateProgressBar()
    {
        progressBar.gameObject.SetActive(false);
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

    public void SetProgressOnUI(float progress)
    {
        progressBar.fillAmount = progress;
    }
}