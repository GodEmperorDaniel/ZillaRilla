using Assets.Enemy.Finite_State_Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Enemy.NPCCode
{
    [RequireComponent(typeof(NavMeshAgent), typeof(FiniteStateMachine), typeof(Transform))]
    public class NPC: MonoBehaviour
    {
        NavMeshAgent _navMeshAgent;
        FiniteStateMachine _finiteStateMachine;

        [SerializeField]
        public Transform _player;

        public float lookRadius = 10f;
        

        public void Awake()
        {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();
            _finiteStateMachine = this.GetComponent<FiniteStateMachine>();
        }
        public void Start()
        {
            
        }
        public void Update()
        {
            
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
        public float Destiantoion {
            get { return Vector3.Distance(_player.position, ThisEnemyPosition.position); }
        }
        public Transform ThisEnemyPosition {
            get { return transform; }
        }

    }
}
