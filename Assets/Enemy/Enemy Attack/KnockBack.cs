using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBack : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent nma;
    private Animator ani;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        nma = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
    }
    /// <summary>
    /// Used to apply knockback on for example enemies
    /// </summary>
    /// <param name="str"> The force applied to the objects rigidbody </param>
    /// <param name="dir"> The direction in which the force is applied </param>
    public void ApplyKnockBack(Vector3 dir, float str, float knockbackTimer)
    {
        //Vector3 lastDestination = nma.destination;
        nma.enabled = false;
        rb.isKinematic = false;
        //ani.applyRootMotion = false;
        //nma.isStopped = true;
        rb.AddForce(dir * str, ForceMode.Impulse);
        StartCoroutine(SetNavMeshAgentInfo(knockbackTimer));
    }

    private IEnumerator SetNavMeshAgentInfo(float time)
    {
        yield return new WaitForSeconds(time);
        nma.enabled = true;
        rb.isKinematic = true;
        //ani.applyRootMotion = true;
    }
}
