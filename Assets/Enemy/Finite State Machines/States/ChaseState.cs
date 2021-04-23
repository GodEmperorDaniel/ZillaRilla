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
                SetChaseTarget(_npc._player, _npc.ThisEnemyPosition);
            }
           
            Debug.Log("UPDATING Chaseing STATE");
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("EXITING Chaseing STATE");
            return true;
        }

        private void SetChaseTarget(Transform player, Transform transform)
        {
            if (_npc.Destiantoion <= _npc.lookRadius) {
                _navMeshAgent.SetDestination(player.position);

                //TO DO STOPPING DISTANCE??
                _navMeshAgent.stoppingDistance = 2f;
                if (_navMeshAgent.stoppingDistance == 2f)
                {
                    _fsm.EnterState(FSMStateType.ATTACK);
                }
            }
            else
            {
                _fsm.EnterState(FSMStateType.IDLE);
            }
               
        }
    }
}
