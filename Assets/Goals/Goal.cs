using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Goal : MonoBehaviour
{
    [SerializeField] private string _goalName = "[Default Name]";
    [SerializeField] private string _goalDescription = "[Default Text]";
    [SerializeField] private Goal _nextGoal;
    private bool _completed = false;
    private float _progress = 0f;
    public float _progressAmount = 0.001f;


    [HideInInspector] public bool playerInArea;
    [HideInInspector] public bool enemyInArea;

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

    private void OnEnable()
    {
        UpdateProgression();
    }

    private void Update()
    {
        UpdateProgression();
    }

    private void UpdateProgression()
    {
        if (_completed || playerInArea && enemyInArea || !playerInArea && !enemyInArea) return;
        
        if (enemyInArea && !playerInArea && _progress >= 0.0f)
        {
            _progress -= _progressAmount;
            if (_progress < 0.0f) _progress = 0.0f;
        }
        else if (playerInArea && !enemyInArea && _progress <= 1.0f)
        {
            _progress += _progressAmount;
        }
        
        UIManager.Instance.UpdateProgressionOnUI(_progress);
        
        if (_progress >= 1.0f)
        {
            GoalCompleted();
            _completed = true;
        }
    }
    
    //WE NEED TO CHANGE THIS SO THAT WE CAN COMPLETE GOALS WITHOUT ZONES
    public void GoalCompleted()
    {
        GameManager.Instance.UpdateObjective(_nextGoal);
    } 
}