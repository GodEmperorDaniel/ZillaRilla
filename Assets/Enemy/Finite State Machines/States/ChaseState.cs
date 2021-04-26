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
            Debug.Log("ENTERED Chaseing STATE");

            EnteredState = true;

            return EnteredState;
        }
        public override void UpdateState()
        {
            if (EnteredState)
            {
                SetChaseTarget(_npc._player);
            }
           
            Debug.Log("UPDATING Chaseing STATE");
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("EXITING Chaseing STATE");
            return true;
        }

        private object SetChaseTarget(Transform player)
        {
            
            if (_npc.Destiantion <= _npc.lookRadius) {
                _navMeshAgent.SetDestination(player.position);
                //FaceTarget();
                //if (_npc.Destiantion <= _navMeshAgent.stoppingDistance)
                //{
                //    FaceTarget();
                //}
                //TO DO STOPPING DISTANCE??
                if (_npc.Destiantion <= _npc.attackRadius)
                {
                    _navMeshAgent.isStopped = true;
                    _fsm.EnterState(FSMStateType.ATTACK);
                }
            }
            else
            {
                _fsm.EnterState(FSMStateType.IDLE);
            }
            return null;
        }
        //void FaceTarget()
        //{

        //    Vector3 direction = (_npc._player.position - _npc.ThisEnemyPosition.position).normalized;
        //    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //    _npc.ThisEnemyPosition.rotation = Quaternion.Slerp(_npc.ThisEnemyPosition.rotation, lookRotation,Time.deltaTime * 5f);
        //}
    }
}
