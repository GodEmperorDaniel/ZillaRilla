using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableBuilding : MonoBehaviour
{
    private Attackable _attackable;
    private Animation _animation;

    private void Start()
    {
        _attackable = GetComponent<Attackable>();
        _animation = GetComponent<Animation>();
    }

    private void BuildingDestruction()
    {
        Debug.Log("Building Destroyed");
        _animation.Play("BuildingCollapse");
    }

}
