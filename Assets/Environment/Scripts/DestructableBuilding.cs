using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestructableBuilding : MonoBehaviour
{
    private const string cNewsCategory = "Building Destruction";
    
    private Animation _animation;
    [SerializeField] private ParticleSystem _smokePrefab;
    [SerializeField] private GameObject _rubblePrefab;
    private List<ParticleSystem> _smokeParticleSystems;
    public float smokeLifetime = 5.0f;
    public float collapseDelay = 1.0f;

    [HideInInspector] public bool isRubble = false;

    private void Start()
    {
        _smokeParticleSystems = new List<ParticleSystem>();
        
        TryGetComponent(out _animation);
    }

    private IEnumerator SmokeTimer()
    {
        if (_smokeParticleSystems.Count <= 0) yield return null;

        yield return new WaitForSeconds(smokeLifetime);
        
        foreach (ParticleSystem pSystem in _smokeParticleSystems)
        {
            pSystem.Stop();
        }
    }

    private IEnumerator BuildingDestruction() //is used in sendmessage
    {
        if (isRubble) yield return null;

        GetComponent<PlayOneShot>().PlaySound("Collapse");
        CreateSmokeAndRubble();
        yield return new WaitForSeconds(collapseDelay);

        // Activate news prompt
        float activationChance = UIManager.Instance.InGameUI.NewsChanceBuilding;
        UIManager.Instance.ActivateBannerRandom(cNewsCategory, false, activationChance);
        
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
        rubble.transform.localScale = transform.lossyScale;
        // Sets the rubble as parent to this to make animation use its base position
        transform.SetParent(rubble.transform);

        ParticleSystem pSystem = Instantiate(_smokePrefab, rubble.transform);

        _smokeParticleSystems.Add(pSystem);
        StartCoroutine(SmokeTimer());
        //made so only one spawn

    }
}