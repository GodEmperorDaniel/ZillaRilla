using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Goal : MonoBehaviour
{
    private const string cNewsCategory = "Goal";
    
    [SerializeField] protected string goalName = "[Default Name]";
    [SerializeField] protected string goalDescription = "[Default Text]";
    [SerializeField] private string newsTitle;

    protected bool _completed = false;


    public string GoalDescription
    {
        get => goalDescription;
        private set => goalDescription = value;
    }

    public string GoalName
    {
        get => goalName;
        private set => goalName = value;
    }

    public virtual void GoalInitialization()
    {
        Debug.Log("Goal Initialized");
    }
    
    public virtual void GoalCompleted()
    {
        // TODO Create Goal XML script

        // Will override current news banner
        if (newsTitle == "")
            UIManager.Instance.ActivateBanner(cNewsCategory, true); 
        else
            UIManager.Instance.ActivateBanner(cNewsCategory, true, newsTitle);
        
        GetComponentInParent<GoalManager>().GoalCompleted();
    } 
}