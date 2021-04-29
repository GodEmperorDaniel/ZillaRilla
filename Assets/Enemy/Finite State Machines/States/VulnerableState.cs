using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Enemy.Finite_State_Machines.States
{
    [CreateAssetMenu(fileName = "VulnerableState", menuName = "ZillaRilla/States/Vulnerable", order = 5)]
    class VulnerableState: AbstractFSMState
    {

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.VULNERABLE;
        }
        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("ENTERED VULNERABLE STATE");
            }
            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                Debug.Log("UPDATING VULNERABLE STATE");

            }
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("EXITING VULNERABLE STATE");
            return true;
        }
    }
}
