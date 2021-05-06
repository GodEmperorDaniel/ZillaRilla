using Assets.Enemy.Finite_State_Machines;
using Assets.Enemy.NPCCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ExecutionState
{
    NONE,
    ACTIVE,
    COMPLETED,
    TERMINATED,
};
public enum FSMStateType
{
    IDLE,
    CHASING,
    DEATH,
    ATTACK,
    STUN,
    FLEE,
    VULNERABLE,
};

public abstract class AbstractFSMState : ScriptableObject
{
    protected NavMeshAgent _navMeshAgent;
    protected NPC _npc;
    public FiniteStateMachine _fsm;
    public ExecutionState ExecutionState { get; protected set; }
    public FSMStateType StateType { get; protected set; }
    public bool EnteredState { get; protected set; }
    public virtual void OnEnable()
    {
        ExecutionState = ExecutionState.NONE;
    }
    public virtual bool EnterState()
    {
        bool sucessNavMesh = true;
        bool sucessNPC = true;
        ExecutionState = ExecutionState.ACTIVE;

        //Does the nav mesh agent exists?
        sucessNavMesh = (_navMeshAgent != null);
        sucessNPC = (_npc != null);

        return sucessNavMesh & sucessNPC;
    }

    public abstract void UpdateState();
    public virtual bool ExitState()
    {
        ExecutionState = ExecutionState.COMPLETED;
        return true;
    }

    public virtual void SetNavMeshAgent(NavMeshAgent navMeshAgent) {

        if (navMeshAgent != null)
        {
            _navMeshAgent = navMeshAgent;
        }
    
    }
    public virtual void SetexecutingFSM(FiniteStateMachine fsm)
    {
        if (fsm != null)
        {
            _fsm = fsm;
        }
    }

    public virtual void SetExecutingNPC(NPC npc)
    {
        if (npc != null)
        {
            _npc = npc;
        }
    }
}
