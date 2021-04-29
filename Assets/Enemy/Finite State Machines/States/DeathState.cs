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
        [SerializeField] private ZillaAttacks _zilla;
        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.DEATH;
        }
        
        public override bool EnterState()
        {
            GameManager.Instance._zilla.gameObject.TryGetComponent<ZillaAttacks>(out _zilla);
            _navMeshAgent.isStopped = true;
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("ENTERED DEATH STATE");
                _zilla.RemoveEnemyFromList(_fsm.gameObject);
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
