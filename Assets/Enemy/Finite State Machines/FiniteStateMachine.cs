using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Assets.Enemy.NPCCode;

namespace Assets.Enemy.Finite_State_Machines
{
    public class FiniteStateMachine: MonoBehaviour
    {

        AbstractFSMState _currentState;

        [SerializeField]
        List<AbstractFSMState> _validState;
        Dictionary<FSMStateType, AbstractFSMState> _fsmStates;

        public void Awake()
        {
            _currentState = null;
            _fsmStates = new Dictionary<FSMStateType, AbstractFSMState>();

            NavMeshAgent navMesAgent = this.GetComponent<NavMeshAgent>();
            NPC npc = this.GetComponent<NPC>();
            
            foreach (AbstractFSMState state in _validState)
            {
                state.SetexecutingFSM(this);
                state.SetExecutingNPC(npc);
                state.SetNavMeshAgent(navMesAgent);
                _fsmStates.Add(state.StateType, state);
            }
        }
        public void Start()
        {
            EnterState(FSMStateType.IDLE);
        }
        public void Update()
        {
            if (_currentState != null)
            {
                _currentState.UpdateState();
            }
        }

        #region     STATE MANAGEMENT
        public void EnterState(AbstractFSMState nextState)
        {
            if (nextState == null)
            {
                return;
            }
            if (_currentState != null)
            {
                _currentState.ExitState();
            }
            _currentState = nextState;
            _currentState.EnterState();
        }

        public void EnterState(FSMStateType stateType)
        {
            if (_fsmStates.ContainsKey(stateType))
            {
                AbstractFSMState nextState = _fsmStates[stateType];

                EnterState(nextState);
            }
        }
        #endregion
    }
}
