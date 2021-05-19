using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Enemy.Finite_State_Machines.States
{
    [CreateAssetMenu(fileName = "SpawnState", menuName = "ZillaRilla/States/Spawn", order = 8)]
    class SpawnState: AbstractFSMState 
    {
        [SerializeField] private float test;
        public override void OnEnable()
        {

            base.OnEnable();
            StateType = FSMStateType.SPAWNING;
        }
        public override bool EnterState()
        {
            _navMeshAgent.isStopped = false;
            EnteredState = base.EnterState();

            //if (EnteredState)
            //{
            //    Debug.Log("ENTERED SPAWNING STATE");

            //}
            return EnteredState;

        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                SpawnEnemy();
                _fsm.EnterState(FSMStateType.IDLE);
            }
        }

        public override bool ExitState()
        {
            base.ExitState();

            //Debug.Log("EXITING SPAWNING STATE");
            return true;
        }

        private void SpawnEnemy()
        {
            Instantiate(_npc.GetEnemyObject, _npc.ThisTransform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
}
