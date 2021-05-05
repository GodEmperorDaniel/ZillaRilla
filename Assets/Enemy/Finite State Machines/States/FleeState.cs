using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Enemy.Finite_State_Machines.States
{
    [CreateAssetMenu(fileName = "FleeState", menuName = "ZillaRilla/States/Flee", order = 6)]
    class FleeState : AbstractFSMState
    {
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.FLEE;
        }
        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("ENTERED FLEE STATE");
            }
            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                Debug.Log("UPDATING FLEE STATE");
                SetFleeFromTarget(_npc.PlayerTransform);
            }
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("EXITING FLEE STATE");
            return true;
        }

        private object SetFleeFromTarget(Transform player)
        {
            if (_npc.Destiantion() <= _npc.lookRadius)
            {
                Vector3 dirToPLayer = _npc.transform.position - player.position;

                Vector3 newPos = _npc.transform.position + dirToPLayer;

                _npc.FaceTarget(_npc.PlayerTransform);

                _navMeshAgent.SetDestination(newPos);
            }
            else
            {
                _fsm.EnterState(FSMStateType.IDLE);
            }
            return null;
        }
    }
}
