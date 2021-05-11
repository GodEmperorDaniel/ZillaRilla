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
    SPAWNER,
    BOSS,
};
namespace Assets.Enemy.NPCCode
{
    [RequireComponent(typeof(NavMeshAgent), typeof(FiniteStateMachine), typeof(Transform))]
    public class NPC: MonoBehaviour
    {
        NavMeshAgent _navMeshAgent;
        private FiniteStateMachine _finiteStateMachine;
        EnemyAttacks _enemyAttacks;
        public EnemyType enemyType;
        public GameObject _enemyToSpawn;
        public List<Transform> _playerList = new List<Transform>(2);
        [SerializeField] private float _rotationSpeed;
        private Transform playerTransform;
        private Transform enemyTransform;
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
            enemyTransform = this.gameObject.transform;
        }
        public void Update()
        {
            if (_playerList[0] == null)
            { 
                SetPlayerReferences();
            }
            //SetChaseTarget();
        }
        private void SetPlayerReferences()
        {
            //Debug.Log(GameManager.Instance._rilla.gameObject.name);
            _playerList[0] = (GameManager.Instance._rilla.gameObject.transform);
            _playerList[1] = (GameManager.Instance._zilla.gameObject.transform);
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
        public Transform PlayerTransform { get { return playerTransform;  } set { playerTransform = value; } }

        public Transform ThisTransform { get { return enemyTransform; } set { enemyTransform = value;  } }
        public FiniteStateMachine GetFiniteStateMachine { get { return _finiteStateMachine; } set { _finiteStateMachine = value; } }

        public GameObject GetEnemyObject { get { return _enemyToSpawn; } }
        public float Destiantion()
        {

            float saveDistance = 0.0f;
            foreach (Transform player in _playerList)
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
            get { return _playerList; }
        }
        public EnemyAttacks GetEnemyAttack {
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
