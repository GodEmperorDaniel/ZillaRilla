using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBack : MonoBehaviour
{
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// Used to apply knockback on for example enemies
    /// </summary>
    /// <param name="str"> The force applied to the objects rigidbody </param>
    /// <param name="dir"> The direction in which the force is applied </param>
    public void ApplyKnockBack(Vector3 dir, float str)
    {
        NavMeshAgent nma = GetComponent<NavMeshAgent>();
        //Vector3 lastDestination = nma.destination;
        nma.isStopped = true;
        rb.AddForce(dir * str, ForceMode.Impulse);
        nma.isStopped = false;
    }
}
