using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Manager<LevelManager>
{
    [SerializeField] private List<Objective> _objectives;
    private Objective _currentObjective;


    public Objective CurrentObjective
    {
        get => _currentObjective;
        private set
        {
            _currentObjective = value; 
            UpdateObjective();
        }
    }

    private void Start()
    {
        _currentObjective = _objectives[0];
    }

    private void UpdateObjective()
    {
        //UIManager.Instance.UpdateObjectiveOnUI(_currentObjective);
    } 
}
