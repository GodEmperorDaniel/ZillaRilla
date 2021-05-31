using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Goal : MonoBehaviour
{
    private const string cGoalCategory = "Goal";
    
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
        Debug.Log("Goal Completed");
        
        // TODO Create Goal XML script
        // if (newsTitle != "") UIManager.Instance.ActivateBanner(cGoalCategory, newsTitle);
        GetComponentInParent<GoalManager>().GoalCompleted();
    } 
}