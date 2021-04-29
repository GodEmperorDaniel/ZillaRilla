using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Enemy.Finite_State_Machines.States
{
    [CreateAssetMenu(fileName = "DeathState", menuName = "ZillaRilla/States/Death", order = 3)]
    public class DeathState : AbstractFSMState
    {
        public override void OnEnable()
        {
            base.OnEnable();
            
            StateType = FSMStateType.DEATH;
        }
        public override bool EnterState()
        {
            _navMeshAgent.isStopped = true;
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("ENTERED DEATH STATE");
                Destroy(_npc.gameObject, _npc.deSpawnTime);
            }
            return EnteredState;

        }

        public override void UpdateState()
        {
            Debug.Log("UPDATING DEATH STATE");
        }
    }
}
