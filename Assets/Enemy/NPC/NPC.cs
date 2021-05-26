using Assets.Enemy.Finite_State_Machines;
using Attacks.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
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
    public class NPC : MonoBehaviour
    {
        //TODO: Ranged Enemy approaching player when no line of sight
        //TODO: If there is another player in range prioritize the one it can shoot

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
        private LayerMask _coveringLayers;

        private Animator _animator;
        

        //public RillaPunchSettings punchSettings;

        public void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _finiteStateMachine = GetComponent<FiniteStateMachine>();
            _enemyAttacks = GetComponent<EnemyAttacks>();
            enemyTransform = gameObject.transform;
            _animator = GetComponent<Animator>();
        }

        public void Update()
        {
            SetPlayerReferences();
            //SetChaseTarget();
        }

        private void SetPlayerReferences()
        {
            //Debug.Log(GameManager.Instance._rilla.gameObject.name);
            _playerList = GameManager.Instance._attackableCharacters;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }

        public float ClosestPlayerDistance(out Transform targetTransform)
        {
            float playerDistance = 0.0f;
            targetTransform = null;
            for (int i = 0; i < _playerList.Count; i++)
            {
                if (_playerList[i] != null && playerDistance > Vector3.Distance(_playerList[i].position, transform.position) || playerDistance == 0)
                {
                    playerDistance = Vector3.Distance(_playerList[i].position, transform.position);
                    targetTransform = _playerList[i];
                }
            }

            return playerDistance;
        }
        
        public float ClosestPlayerDistance()
        {
            float playerDistance = 0.0f;
            for (int i = 0; i < _playerList.Count; i++)
            {
                if (_playerList[i] != null && playerDistance > Vector3.Distance(_playerList[i].position, transform.position) || playerDistance == 0)
                {
                    playerDistance = Vector3.Distance(_playerList[i].position, transform.position);
                }
            }

            return playerDistance;
        }

        //fixed with help from andreas
        private bool _lookAtTarget;
        private Transform _lookAtTransform;

        private void LateUpdate()
        {
            if (!_lookAtTarget) return;
            Vector3 direction = (_lookAtTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
        }
        public void FaceTarget(Transform target)
        {
            _lookAtTarget = true;
            _lookAtTransform = playerTransform;
        }
        public void RemoveTarget()
        {
            _lookAtTarget = false;
            _lookAtTransform = null;
        }

        public Transform PlayerTransform
        {
            get { return playerTransform; }
            set { playerTransform = value; }
        }

        public Transform ThisTransform
        {
            get { return enemyTransform; }
            set { enemyTransform = value; }
        }

        public FiniteStateMachine GetFiniteStateMachine
        {
            get { return _finiteStateMachine; }
            set { _finiteStateMachine = value; }
        }

        public GameObject GetEnemyObject
        {
            get { return _enemyToSpawn; }
        }

        public List<Transform> GetPlayerList
        {
            get { return _playerList; }
        }

        public EnemyAttacks GetEnemyAttack
        {
            get { return _enemyAttacks; }
        }
    }
}