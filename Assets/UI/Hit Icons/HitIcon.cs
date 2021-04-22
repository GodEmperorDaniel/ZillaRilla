using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIcon : MonoBehaviour
{
    [HideInInspector] public Camera _camera;
    private Canvas _canvas;
    private RectTransform _rectTransform;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        //_rectTransform.LookAt(_camera.transform);
    }
}
