using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIcon : MonoBehaviour
{
    [HideInInspector] public float _lifeTime;
    [HideInInspector] public float _scaling;
    [HideInInspector] public float _maxScaling;
    [HideInInspector] public Vector3 _movement;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (_rectTransform.localScale.x < _maxScaling)
        {
            _rectTransform.localScale += Vector3.one * _scaling;  
        }
        
        transform.position += _movement;
    }
}
