using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Enemy.Finite_State_Machines.States
{
    [CreateAssetMenu(fileName = "ChaseState", menuName = "ZillaRilla/States/Chase", order = 2)]
    public class ChaseState : AbstractFSMState
    {
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.CHASING;
        }
        public override bool EnterState()
        {
            base.EnterState();
            //Debug.Log("ENTERED Chaseing STATE");

            EnteredState = true;

            return EnteredState;
        }
        public override void UpdateState()
        {
            if (EnteredState)
            {
                SetChaseTarget(_npc.PlayerTransform);
            }
           
            //Debug.Log("UPDATING Chaseing STATE");
        }

        public override bool ExitState()
        {
            if(_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.isStopped = true;
            base.ExitState();
            _npc.RemoveTarget();
            //Debug.Log("EXITING Chaseing STATE");
            return true;
        }

        private object SetChaseTarget(Transform player)
        {
            if (_npc.ClosestPlayerDistance(out Transform target) <= _npc.lookRadius)
            {
                _npc.PlayerTransform = target;
                _npc.FaceTarget(_npc.PlayerTransform);
                if(_navMeshAgent.isActiveAndEnabled)
                    _navMeshAgent.SetDestination(player.position);

                // Test if player is in attack range and in view
                if (_npc.ClosestPlayerDistance() <= _npc.attackRadius && _npc.enemyType != EnemyType.RANGE)
                {
                    _fsm.EnterState(FSMStateType.ATTACK);
                }
                else if (_npc.ClosestPlayerDistance() <= _npc.attackRadius && _npc.GetEnemyAttack.IsPlayerInView(player, _npc))
                {
                    _fsm.EnterState(FSMStateType.ATTACK);
                }
            }
            else
            {
                //_navMeshAgent.SetDestination(_npc.ThisTransform.position);
                _fsm.EnterState(FSMStateType.IDLE);
            }
            return null;
        }
    }
}
