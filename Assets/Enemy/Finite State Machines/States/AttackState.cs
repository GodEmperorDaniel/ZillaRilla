using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Attacks;

namespace Assets.Enemy.Finite_State_Machines.States
{
    public class AttackState : AbstractFSMState
    {
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.ATTACK;
        }
        public override bool EnterState()
        {
            base.EnterState();
            Debug.Log("ENTERED ATTACK STATE");

            EnteredState = true;

            return EnteredState;
        }
        public override void UpdateState()
        {
            if (EnteredState)
            {
                Debug.Log("UPDATING ATTACK STATE");
            }

            
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("EXITING ATTACK STATE");
            return true;
        }
    }
}
