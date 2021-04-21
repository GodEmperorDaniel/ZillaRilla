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
        [SerializeField]
        public float stunTime;
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

            }
            return EnteredState;

        }

        public override void UpdateState()
        {
            Debug.Log("UPDATING STUN STATE");



            _fsm.EnterState(FSMStateType.IDLE);
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("EXITING STUN STATE");
            return true;
        }
    }
}
