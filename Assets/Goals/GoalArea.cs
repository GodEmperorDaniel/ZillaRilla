using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    private List<Collider> _playersInArea;
    private List<Collider> _enemiesInArea;
    private Goal _objective;

    private void Start()
    {
        _playersInArea = new List<Collider>();
        _enemiesInArea = new List<Collider>();
        
        _objective = GetComponentInParent<Goal>();
    }

    private void OnTriggerEnter(Collider other)
    {
        AddAreaObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveAreaObject(other);
    }

    
    private void AddAreaObject(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playersInArea.Add(other);
            Debug.Log("Added " + other.name + " from _playersInArea");
            if (_playersInArea.Count != 0) _objective.playerInArea = true;
        }
        else
        {
            _enemiesInArea.Add(other);
            Debug.Log("Added " + other.name + " from _enemiesInArea");
            if (_enemiesInArea.Count != 0) _objective.enemyInArea = true;
        }
    }
    
    private void RemoveAreaObject(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playersInArea.Remove(other);
            Debug.Log("Removed " + other.name + " from _playersInArea");
            if (_playersInArea.Count == 0) _objective.playerInArea = false;
        }
        else
        {
            _enemiesInArea.Remove(other);
            Debug.Log("Removed " + other.name + " from _enemiesInArea");
            if (_enemiesInArea.Count == 0) _objective.enemyInArea = false;
        }
    }
    
}
