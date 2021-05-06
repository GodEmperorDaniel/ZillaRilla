using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject _enemy;
    public int _enemyAmount;

    public float maxSpawnDist;

    private Transform _transform;
    private BoxCollider SpawnArea;
    private int _enemyCount = 0;
    private float xPos;
    private float zPos;
    private void Start()
    {
        _transform = gameObject.transform;
        // Debug.Log("Lenght of Z " + -(_transform.localScale.y / 2));
        //Debug.Log("Lenght of magnitude " + _transform.localScale.magnitude);
        SpawnArea = GetComponent<BoxCollider>();
        StartCoroutine(EnemySpawning());
        SpawnArea.size = new Vector3(maxSpawnDist, 1, maxSpawnDist);
    }
    private void Update()
    {
        
    }

    IEnumerator EnemySpawning()
    {
        while (_enemyCount < _enemyAmount)
        {
            xPos = Random.Range(-maxSpawnDist, maxSpawnDist);
            zPos = Random.Range(-maxSpawnDist, maxSpawnDist);
            Vector3 pos = new Vector3(xPos + _transform.position.x, 0, zPos + _transform.position.z) ;
            //Debug.Log("Enemy position Z: " + zPos);
            Instantiate(_enemy, pos, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            _enemyCount += 1;
        }
    }
}