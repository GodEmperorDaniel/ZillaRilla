using Assets.Enemy.Finite_State_Machines;
using Attacks.Enemy;
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

        public Transform _player;
        public float _stunTime = 3f;
        public float lookRadius = 10f;
        public float attackRadius = 5f;

        public Animator _playerAnimator;

        public RillaPunchSettings punchSettings;


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
            SetChaseTarget(_player);
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
        public float Destiantion {
            get { return Vector3.Distance(_player.position, ThisEnemyPosition.position); }
        }
        public Transform ThisEnemyPosition {
            get { return transform; }
        }
        
        private object SetChaseTarget(Transform player)
        {

            if (Destiantion <= lookRadius)
            {
 
                FaceTarget();

            }
            return null;
        }
        public void FaceTarget()
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
