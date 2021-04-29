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
            _navMeshAgent.isStopped = true;
            base.ExitState();
            
            //Debug.Log("EXITING Chaseing STATE");
            return true;
        }

        private object SetChaseTarget(Transform player)
        {
            
            if (_npc.Destiantion()<= _npc.lookRadius) {
                _npc.FaceTarget(_npc.PlayerTransform);
                _navMeshAgent.SetDestination(player.position);

                if (_npc.Destiantion() <= _npc.attackRadius)
                {
                    
                    _fsm.EnterState(FSMStateType.ATTACK);
                }
            }
            else
            {
                _fsm.EnterState(FSMStateType.IDLE);
            }
            return null;
        }
    }
}
