using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    //TODO: Capture Progress Percent/Bar
    //TODO: Button Prompts (Tutorial and such)
    //TODO: News banner
    //TODO: Kill-o-meter for Zilla and Rilla

    public FillableBar zillaHealthBar;
    public FillableBar rillaHealthBar;
    public FillableBar progressBar;
    
    

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
