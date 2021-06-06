using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : Manager<GoalManager>
{
    private List<Goal> _goals;
    private int _currentGoalIndex;

    public Goal GetCurrentGoal()
    {
        return _goals[_currentGoalIndex];
    }

    private void Start()
    {
        _goals = new List<Goal>();
        
        FindAllGoals();
        
        ResetGoals();
    }

    private void FindAllGoals()
    {
        foreach (Transform child in transform)
        {
            Goal goal = child.GetComponent<Goal>();
            if (goal != null)
            {
                _goals.Add(goal);
            }
        }
    }

    public void ResetGoals()
    {
        foreach (Goal goal in _goals)
        {
            goal.gameObject.SetActive(false);
        }
        _currentGoalIndex = 0;
        InitializeGoal();
    }

    private void InitializeGoal()
    {
        _goals[_currentGoalIndex].gameObject.SetActive(true);
        string goalName = _goals[_currentGoalIndex].GoalName;
        string goalDescription = _goals[_currentGoalIndex].GoalDescription;
        UIManager.Instance.UpdateObjectiveOnUI(goalName, goalDescription);
    }

    public void GoalCompleted()
    {
        // Check if on last Goal in list
        if (_currentGoalIndex < _goals.Count - 1)
        {
            // Deactivate completed goal and initialize next goal 
            _goals[_currentGoalIndex].gameObject.SetActive(false);
            _currentGoalIndex++;
            InitializeGoal();
        }
        else
        {
            AllGoalsCompleted();
        }
    }

    private void AllGoalsCompleted()
    {
        // Signal GameManager that all goals are completed
        GameManager.Instance.VictoryState();
    }
}
