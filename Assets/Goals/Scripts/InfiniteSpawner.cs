using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class InfiniteSpawner : MonoBehaviour
{
    private List<GameObject> _enemies;
    private SpawnState _state = SpawnState.COUNTING;
    [SerializeField] private int maxEnemiesAtTheSameTime;
    [SerializeField] private float _spawnCountdown;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private InfiniteWave _wave;

    public enum SpawnState
    {
        WAITING,
        COUNTING
    }

    [Serializable]
    public class InfiniteWave
    {
        public string Name;
        public GameObject enemy;
        public int minTimeBetweenSpawns;
    }

    private void OnEnable()
    {
        _enemies = new List<GameObject>();
    }

    private void Update()
    {
        if (EnemyListFull()) return;

        if (_spawnCountdown <= 0)
        {
            SpawnEnemy(_wave.enemy);
            _spawnCountdown = _wave.minTimeBetweenSpawns;
            _state = SpawnState.COUNTING;
        }
        else
        {
            _spawnCountdown -= Time.deltaTime;
        }
    }

    private bool EnemyListFull()
    {
        UpdateEnemyList();
        if (_enemies.Count < maxEnemiesAtTheSameTime)
        {
            return false;
        }

        return true;
    }

    private void UpdateEnemyList()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] == null)
            {
                _enemies.RemoveAt(i);
            }
        }
    }

    private void SpawnEnemy(GameObject enemy)
    {
        //Debug.Log("Spawning Enemy " + enemy.name);

        if (spawnPoints.Length == 0)
        {
            Transform defaultSpawnPoint = transform;
            _enemies.Add(Instantiate(enemy, defaultSpawnPoint.position, defaultSpawnPoint.rotation, null));

            Debug.LogError("[" + name + "] Default Spawn Point Used");
        }
        else
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            _enemies.Add(Instantiate(enemy, spawnPoint.position, spawnPoint.rotation, null));
        }
    }
}