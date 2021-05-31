using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIconSpawner : MonoBehaviour
{
    private Camera _camera;
    private Camera _UICamera;
    
    public List<GameObject> _zillaHitPrefab;
    public List<GameObject> _rillaHitPrefab;
    public List<GameObject> _enemyHitPrefab;
    public float iconLifeTime;
    public float iconScaling;
    public float iconMaxScaling;
    public Vector3 iconMovement;

    private void Start()
    {
        _camera = Camera.main;
        _UICamera = _camera.transform.GetChild(0).GetComponent<Camera>();
    }

    private void Update()
    {
        if (_camera != null)
        {
            transform.forward = _camera.transform.forward;
        }
    }

    public void SpawnHitIcon(Vector3 position, int playerIndex)
    {
        GameObject hitIcon;
        switch (playerIndex)
        {
            case 0:
                hitIcon = Instantiate(_zillaHitPrefab[RandomGenerator(_zillaHitPrefab.Count - 1)]);
                break;
            case 1:
                hitIcon = Instantiate(_rillaHitPrefab[RandomGenerator(_rillaHitPrefab.Count - 1)]);
                break;
            case 3:
                hitIcon = Instantiate(_enemyHitPrefab[RandomGenerator(_enemyHitPrefab.Count - 1)]);
                break;
            default:
                Debug.LogWarning("Miss in hitIcon Spawner!");
                hitIcon = Instantiate(_zillaHitPrefab[RandomGenerator(_zillaHitPrefab.Count - 1)]);
                break;
        }
       
        hitIcon.GetComponent<Canvas>().worldCamera = _UICamera;
        hitIcon.GetComponent<HitIcon>()._lifeTime = iconLifeTime;
        hitIcon.GetComponent<HitIcon>()._scaling = iconScaling;
        hitIcon.GetComponent<HitIcon>()._maxScaling = iconMaxScaling;
        hitIcon.GetComponent<HitIcon>()._movement = iconMovement;
        hitIcon.transform.position = position;
    }
    private int RandomGenerator(int maxExlusive)
    {
        return UnityEngine.Random.Range(0, maxExlusive);
    }
}
