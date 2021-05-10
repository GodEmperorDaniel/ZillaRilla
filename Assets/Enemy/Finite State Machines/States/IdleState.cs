using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Enemy.Finite_State_Machines.States
{
    [CreateAssetMenu(fileName ="IdleState", menuName ="ZillaRilla/States/Idle", order =1)]
    public class IdleState : AbstractFSMState
    {
        public override void OnEnable()
        {
            
            base.OnEnable();
            StateType = FSMStateType.IDLE;
        }
        public override bool EnterState()
        {
            _navMeshAgent.isStopped = false;
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                //Debug.Log("ENTERED IDLE STATE");
                
            }
            return EnteredState;
           
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                _fsm.EnterState(FSMStateType.WANDER);
            }
        }

        public override bool ExitState()
        {
            base.ExitState();

            //Debug.Log("EXITING IDLE STATE");
            return true;
        }
    }
}
