using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private string _objectiveName = "[Default Name]";
    private string _objectiveDescription = "[Default Text]";
    private float _progress = 0f;
    private bool _completed = false;

    [HideInInspector] public bool playerInArea;
    [HideInInspector] public bool enemyInArea;

    /* No use right now because not implemented in UI yet
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
        if (_completed || !playerInArea || enemyInArea) return;

        if (_progress <= 1.0f)
        {
            _progress += 0.001f;
            UIManager.Instance.UpdateProgressionOnUI(_progress);
        }
        else
        {
            _completed = true;
        }
    }
    
    
}
