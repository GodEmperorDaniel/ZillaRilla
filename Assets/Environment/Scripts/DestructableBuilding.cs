using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestructableBuilding : MonoBehaviour
{
    private Attackable _attackable; //to check health?
    private Animation _animation;
    [SerializeField] private ParticleSystem _smokePrefab;
    [SerializeField] private GameObject _rubblePrefab;
    private List<ParticleSystem> _smokeParticleSystems;
    public float smokeLifetime = 5.0f;
    public float collapseDelay = 1.0f;
    
    //[SerializeField] private Mesh _rubbleMesh; //unused prob?

    [HideInInspector] public bool isRubble = false;

    private void Start()
    {
        _smokeParticleSystems = new List<ParticleSystem>();
        
        TryGetComponent(out _attackable);
        TryGetComponent(out _animation);
    }

    //private void Update()
    //{
    //    if (Keyboard.current.numpad1Key.wasPressedThisFrame)
    //    {
    //        StartCoroutine(BuildingDestruction());
    //    }

    //    StartCoroutine(SmokeTimer());
    //}

    private IEnumerator SmokeTimer()
    {
        if (_smokeParticleSystems.Count <= 0) yield return null; //what is this suppose to do? / Jonte

        yield return new WaitForSeconds(smokeLifetime);
        
        foreach (ParticleSystem pSystem in _smokeParticleSystems)
        {
            pSystem.Stop();
        }

    }

    private IEnumerator BuildingDestruction()
    {
        if (isRubble) yield return null;
        
        Debug.Log("Building Destroyed");
        CreateSmokeAndRubble();
        yield return new WaitForSeconds(collapseDelay); 
        
        _animation.Play("BuildingCollapse");
    }

    private void OnAnimationDone()
    {
        Destroy(gameObject);
    }

    private void CreateSmokeAndRubble()
    {
        DestructableBuilding rubble = Instantiate(_rubblePrefab, transform.position, transform.rotation).AddComponent<DestructableBuilding>();
        rubble.GetComponent<DestructableBuilding>().isRubble = true;

        // Sets the rubble as parent to this to make animation use its base position
        transform.SetParent(rubble.transform);

        for (int i = 0; i < 4; i++)
        {
            ParticleSystem pSystem = Instantiate(_smokePrefab, rubble.transform);
            pSystem.transform.Rotate(Vector3.up, i * 90);

            _smokeParticleSystems.Add(pSystem);
        }
        StartCoroutine(SmokeTimer()); //blir lite mindre corutine calls på detta sättet tänker jag? eller missar jag något kanske
    }
}