using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using UnityEditor;
using UnityEngine.SocialPlatforms;

public class SmokeLauncher : MonoBehaviour
{

    private ParticleSystem _smokeParticleSystem;

    private void Start()
    {
        _smokeParticleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // DEBUG
        if (Keyboard.current.numpad0Key.wasPressedThisFrame)
        {
            CreateSmokeParticles();
        }
        CreateSmokeParticles();
    }

    private void CreateSmokeParticles()
    {
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

        Vector3 emitDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
        emitDirection = Vector3.Normalize(emitDirection);

        emitParams.velocity = emitDirection;
        
        
        _smokeParticleSystem.Emit(emitParams, 1);
    }

    private void UpdateSmokeParticles()
    {
        NativeArray<ParticleSystem.Particle> particles = new NativeArray<ParticleSystem.Particle>();
        _smokeParticleSystem.GetParticles(particles);

        foreach (ParticleSystem.Particle particle in particles)
        {
            Vector3 direction = particle.velocity.normalized;
            Vector3 rotation = particle.rotation3D;
        }
    }
    
}
