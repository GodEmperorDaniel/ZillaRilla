using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


[Serializable]
public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    }

    [Serializable]
    public class Wave
    {
        public string Name;
        public GameObject enemy;
        public int count;
        public float spawnRate;
    }

    public List<Wave> waves;
    private int _activeWave;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float _waveCountdown;

    private List<GameObject> _enemies;

    private SpawnState _state = SpawnState.COUNTING;

    public bool WavesCompleted { get; private set; } = false;

    private void OnEnable()
    {
        _enemies = new List<GameObject>();
        _waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if (_state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                //Debug.Log("Still Enemies in scene!");
                return;
            }
        }

        if (_waveCountdown <= 0)
        {
            if (_state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[_activeWave]));
            }
        }
        else
        {
            _waveCountdown -= Time.deltaTime;
        }
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        _state = SpawnState.COUNTING;
        _waveCountdown = timeBetweenWaves;

        if (_activeWave + 1 > waves.Count - 1)
        {
            WavesCompleted = true;
            Debug.Log("ALL WAVES COMPLETE!");
        }

        _activeWave++;
    }

    private bool EnemyIsAlive()
    {
        UpdateEnemyList();
        if (_enemies.Count == 0)
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

    private IEnumerator SpawnWave(Wave wave)
    {
        //Debug.Log("Spawning Wave: " + wave.Name);
        _state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        _state = SpawnState.WAITING;

        yield break;
    }

    private void SpawnEnemy(GameObject enemy)
    {
        //Debug.Log("Spawning Enemy " + enemy.name);

        if (spawnPoints.Length == 0)
        {
            Transform defaultSpawnPoint = transform;
            _enemies.Add(Instantiate(enemy, defaultSpawnPoint.position, defaultSpawnPoint.rotation, defaultSpawnPoint));

            Debug.LogError("[" + name + "] Default Spawn Point Used");
        }
        else
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            _enemies.Add(Instantiate(enemy, spawnPoint.position, spawnPoint.rotation, transform));
        }
    }
}