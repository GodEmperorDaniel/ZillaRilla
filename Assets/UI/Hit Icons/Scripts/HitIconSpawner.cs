using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIconSpawner : MonoBehaviour
{
    private Camera _camera;
    
    public GameObject _hitPrefab;
    public float iconLifeTime;
    public float iconScaling;
    public float iconMaxScaling;
    public Vector3 iconMovement;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_camera != null)
        {
            transform.forward = _camera.transform.forward;
        }
        
    }

    public void SpawnHitIcon(Vector3 position)
    {
        GameObject hitIcon = Instantiate(_hitPrefab, transform);
        hitIcon.GetComponent<Canvas>().worldCamera = _camera;
        hitIcon.GetComponent<HitIcon>()._lifeTime = iconLifeTime;
        hitIcon.GetComponent<HitIcon>()._scaling = iconScaling;
        hitIcon.GetComponent<HitIcon>()._maxScaling = iconMaxScaling;
        hitIcon.GetComponent<HitIcon>()._movement = iconMovement;
        hitIcon.transform.position = position;
    }
}
