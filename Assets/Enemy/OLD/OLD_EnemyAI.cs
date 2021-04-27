using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

    public class EnemyAI : MonoBehaviour
    {
        [SerializeField]
        private Transform player;

        public float lookRadius = 10f;

        Transform target;
        NavMeshAgent agent;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            target = player;
        }
        
        void Update()
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);
            }
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
    }

