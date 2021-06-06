using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class AreaGoal : Goal
{
    private List<Collider> _playersInArea;
    private List<Collider> _enemiesInArea;
    private bool playerInArea;
    private bool enemyInArea;
    
    private float _progress = 0f;
    public float _progressAmount = 0.001f;

    
    private void Start()
    {
        _playersInArea = new List<Collider>();
        _enemiesInArea = new List<Collider>();
    }

    private void OnEnable()
    {
        GoalInitialization();
        UpdateProgression();
    }

    private void OnTriggerEnter(Collider other)
    {
        AddAreaObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveAreaObject(other);
    }

    private void Update()
    {
        for (int i = 0; i < _enemiesInArea.Count; i++)
        {
            if (_enemiesInArea[i] == null)
            {
                _enemiesInArea.RemoveAt(i);
            }
        }
        if (_enemiesInArea.Count == 0) enemyInArea = false;
        
            UpdateProgression();
    }

    private void AddAreaObject(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playersInArea.Add(other);
            //Debug.Log("Added " + other.name + " from _playersInArea");
            if (_playersInArea.Count != 0) playerInArea = true;
        }
        else
        {
            _enemiesInArea.Add(other);
            //Debug.Log("Added " + other.name + " from _enemiesInArea");
            if (_enemiesInArea.Count != 0) enemyInArea = true;
        }
    }

    private void RemoveAreaObject(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playersInArea.Remove(other);
            //Debug.Log("Removed " + other.name + " from _playersInArea");
            if (_playersInArea.Count == 0) playerInArea = false;
        }
        else
        {
            _enemiesInArea.Remove(other);
            //Debug.Log("Removed " + other.name + " from _enemiesInArea");
            if (_enemiesInArea.Count == 0) enemyInArea = false;
        }
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
                _completed = true;
                GoalCompleted();
            }
        }

    public override void GoalInitialization()
    {
        base.GoalInitialization();
    }

    public override void GoalCompleted()
    {
        base.GoalCompleted();
        UIManager.Instance.InGameUI.DeactivateProgressBar();
    }
}