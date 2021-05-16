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
        [SerializeField] private GameObject _bugSplatDecal;
        private ZillaAttacks _zilla;
        private RillaAttacks _rilla;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.DEATH;
        }
        
        public override bool EnterState()
        {
            GameManager.Instance._zilla.gameObject.TryGetComponent<ZillaAttacks>(out _zilla);
            GameManager.Instance._rilla.gameObject.TryGetComponent<RillaAttacks>(out _rilla);
            _navMeshAgent.isStopped = true;
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                //Debug.Log("ENTERED DEATH STATE");
                _zilla.RemoveFromPlayerList(_fsm.gameObject);
                _rilla.RemoveFromPlayerList(_fsm.gameObject);
                Instantiate(_bugSplatDecal,_fsm.gameObject.transform.position, _bugSplatDecal.transform.rotation);
                Destroy(_npc.gameObject, _npc.deSpawnTime);
            }
            return EnteredState;

        }

        public override void UpdateState()
        {
            //Debug.Log("UPDATING DEATH STATE");
        }
    }
}
