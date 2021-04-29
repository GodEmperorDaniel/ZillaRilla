using Assets.Enemy.Finite_State_Machines;
using Attacks.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public enum EnemyType
{
    MELEE,
    RANGE,
    BOSS,
};
namespace Assets.Enemy.NPCCode
{
    [RequireComponent(typeof(NavMeshAgent), typeof(FiniteStateMachine), typeof(Transform))]
    public class NPC: MonoBehaviour
    {
        NavMeshAgent _navMeshAgent;
        FiniteStateMachine _finiteStateMachine;
        EnemyAttacks _enemyAttacks;

        public EnemyType enemyType;

        public List<Transform> PlayerList;
        [SerializeField]
        private float _rotationSpeed;
        private Transform playerTransform;
        public float _stunTime = 3f;
        public float lookRadius = 10f;
        public float attackRadius = 5f;
        public float deSpawnTime = 1;

        public Animator _playerAnimator;

        //public RillaPunchSettings punchSettings;


        public void Awake()
        {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();
            _finiteStateMachine = this.GetComponent<FiniteStateMachine>();
            _enemyAttacks = this.GetComponent<EnemyAttacks>();
        }
        public void Start()
        {
        }
        public void Update()
        {
            //SetChaseTarget();
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
        public Transform PlayerTransform { get { return playerTransform;  } set { playerTransform = value; } }
        public float Destiantion()
        {

            float saveDistance = 0.0f;
            foreach (Transform player in PlayerList)
            {
                if (saveDistance > Vector3.Distance(player.position, transform.position) || saveDistance == 0)
                {
                    saveDistance = Vector3.Distance(player.position, transform.position);
                    PlayerTransform = player;
                }   
            }
            return saveDistance;
        }
        public List<Transform> GetPlayerList {
            get { return PlayerList; }
        }
        public EnemyAttacks getEnemyAttack {
            get { return _enemyAttacks; }
        }
        public void FaceTarget(Transform player)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
        }
    }
}
