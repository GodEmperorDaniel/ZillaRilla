using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitTester : MonoBehaviour
{
    public HitIconSpawner hitIconSpawner;

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            hitIconSpawner.SpawnHitIcon(transform.position, 10);
        }
    }
}
