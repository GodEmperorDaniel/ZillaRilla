using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : Manager<GoalManager>
{
    public List<Goal> _goals;
    private int _currentGoalIndex;

    public Goal GetCurrentGoal()
    {
        return _goals[_currentGoalIndex];
    }

    private void Start()
    {
        ResetGoals();
    }

    private void ResetGoals()
    {
        _currentGoalIndex = 0;
        InitializeGoal();
    }

    private void InitializeGoal()
    {
        string goalName = _goals[_currentGoalIndex].GoalName;
        string goalDescription = _goals[_currentGoalIndex].GoalDescription;
        UIManager.Instance.UpdateObjectiveOnUI(goalName, goalDescription);
    }

    private void GoalCompleted()
    {
        // Check if on last Goal in list
        if (_currentGoalIndex < _goals.Count - 1)
        {
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
    }
}
