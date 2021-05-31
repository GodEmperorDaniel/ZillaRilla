using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Enemy.Finite_State_Machines.States
{
    [CreateAssetMenu(fileName = "WanderState", menuName = "ZillaRilla/States/Wander", order = 7)]
    class WanderState: AbstractFSMState
    {
        [SerializeField] private float _nrEnemiesToSpawn = 5; //QUICK FIX
        private float _spawnedEnemies; //QUICK FIX
        private readonly float wanderRange = 30f;
        private Vector3 wanderTarget;
        private NavMeshHit navHit;
        private Transform myTransform;
        private NavMeshAgent myNavMeshAgent;
        private float checkRate;
        private float nextCheck;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.WANDER;
        }
        public override bool EnterState()
        {
            myNavMeshAgent = _navMeshAgent;
            checkRate = Random.Range(0.3f, 0.4f);
            myTransform = _npc.ThisTransform;

            EnteredState = base.EnterState();

            //if (EnteredState)
            //{
            //    Debug.Log("ENTERED WANDER STATE");
            //}
            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                CheckForTarget();
                if (Time.time > nextCheck)
                {
                    nextCheck = Time.time + checkRate;
                    CheckIfIShouldWander();

                    //_fsm.EnterState(FSMStateType.SPAWNING);
                }
                //Debug.Log("UPDATING WANDER STATE");
            }
        }

        public override bool ExitState()
        {
            base.ExitState();

            //Debug.Log("EXITING WANDER STATE");
            return true;
        }

        private void CheckForTarget()
        {
            if (_npc.ClosestPlayerDistance(out Transform target) <= _npc.lookRadius)
            {
                _npc.PlayerTransform = target;
                if (_npc.enemyType == EnemyType.SPAWNER)
                {
                    _fsm.EnterState(FSMStateType.FLEE);
                }
                else
                {
                    _fsm.EnterState(FSMStateType.CHASING);
                }
            }
        }
        private void CheckIfIShouldWander()
        {
            if (RandomWanderTarget(myTransform.position, wanderRange, out wanderTarget))
            {
                if (myNavMeshAgent.isActiveAndEnabled)
                    myNavMeshAgent.SetDestination(wanderTarget);
                if (_npc.enemyType == EnemyType.SPAWNER && _spawnedEnemies != _nrEnemiesToSpawn)
                { 
                    _fsm.EnterState(FSMStateType.SPAWNING);
                    _spawnedEnemies++;
                }
            }
        }
        private bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out navHit, 1.0f, NavMesh.AllAreas))
            {
                result = navHit.position;
                return true;
            }
            else
            {
                result = center;
                return false;
            }
        }

    }
}
