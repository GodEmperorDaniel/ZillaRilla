using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private string _objectiveName = "[Default Name]";
    private string _objectiveDescription = "[Default Text]";
    private float _progress = 0f;

    [HideInInspector] public bool playerInArea;
    [HideInInspector] public bool enemyInArea;

    /*
     public string ObjectiveName
    {
        get => _objectiveName;
        private set => _objectiveName = value;
    }
    */
    
    public string ObjectiveDescription
    {
        get => _objectiveDescription;
        private set => _objectiveDescription = value;
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        UIManager.Instance.UpdateObjectiveOnUI(_objectiveName, _objectiveDescription);
        UpdateProgression();
    }

    private void Update()
    {
        UpdateProgression();
    }

    private void UpdateProgression()
    {
        if (!playerInArea || enemyInArea) return;

        _progress += 0.001f;
        UIManager.Instance.UpdateProgressionOnUI(_progress);
    }
}
