using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIconSpawner : MonoBehaviour
{
    public GameObject _hitPrefab;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        transform.forward = _camera.transform.forward;
    }

    public void SpawnHitIcon(Vector3 position)
    {
        GameObject hitIcon = Instantiate(_hitPrefab, transform);
        hitIcon.GetComponent<Canvas>().worldCamera = _camera;
        hitIcon.GetComponent<HitIcon>()._camera = _camera;
        hitIcon.transform.position = position;
    }
}
