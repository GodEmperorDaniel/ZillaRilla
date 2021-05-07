using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEggSpawner : MonoBehaviour
{
    public GameObject _enemy;
    public float timeForSpawning;
    public float despawnEggTimer;

    private void Start()
    {
        StartCoroutine(EnemySpawning(timeForSpawning));
        Destroy(this.gameObject, despawnEggTimer);
    }

    IEnumerator EnemySpawning(float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(_enemy, transform.position, Quaternion.identity);
    }
}