using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVFixer : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _thisCamera;
    void Update()
    {
        _thisCamera.fieldOfView = _mainCamera.fieldOfView;
    }
}
