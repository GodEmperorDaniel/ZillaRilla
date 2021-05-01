using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(WaveSpawner))]
public class WaveSpawnerGoal : Goal
{
    private WaveSpawner _waveSpawner;

    private void Start()
    {
        _waveSpawner = GetComponent<WaveSpawner>();
    }

    private void Update()
    {
        if (!SpawnerIsDone()) return;
        _completed = true;
        GoalCompleted();
    }

    private bool SpawnerIsDone()
    {
        return _waveSpawner.WavesCompleted;
    }
    
    
    
}
