using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Goal : MonoBehaviour
{
    [SerializeField] protected string _goalName = "[Default Name]";
    [SerializeField] protected string _goalDescription = "[Default Text]";
    protected bool _completed = false;
    
    

    public string GoalDescription
    {
        get => _goalDescription;
        private set => _goalDescription = value;
    }

    public string GoalName
    {
        get => _goalName;
        private set => _goalName = value;
    }

    public virtual void GoalInitialization()
    {
        Debug.Log("Goal Initialized");
    }
    
    public virtual void GoalCompleted()
    {
        Debug.Log("Goal Completed");
        GetComponentInParent<GoalManager>().GoalCompleted();
    } 
}