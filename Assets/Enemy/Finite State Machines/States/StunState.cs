using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Enemy.Finite_State_Machines.States
{
    [CreateAssetMenu(fileName = "StunState", menuName = "ZillaRilla/States/Stun", order = 4)]
    public class StunState : AbstractFSMState
    {
        float _totalDuration;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.STUN;
        }
        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("ENTERED STUN STATE");
                _totalDuration = 0f;
            }
            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
               
                _totalDuration += Time.deltaTime;
                Debug.Log("UPDATING STUN STATE: "+ _totalDuration + " Seconds.");

                if (_totalDuration >= _npc._stunTime)
                {
                    _fsm.EnterState(FSMStateType.IDLE);
                }
            }
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("EXITING STUN STATE");
            return true;
        }
    }
}
