using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Enemy;
using System;

namespace Assets.Enemy.Finite_State_Machines.States
{
    [CreateAssetMenu(fileName = "AttackState", menuName = "ZillaRilla/States/Attack", order = 5)]

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
                if (_npc.enemyType == EnemyType.RANGE)
                {
                    _npc.getEnemyAttack.EnemyShoot();
                }
                else
                {
                    _npc.getEnemyAttack.EnemyPunch();
                }
                
                _npc.FaceTarget(_npc.PlayerTransform);
                Debug.Log("UPDATING ATTACK STATE");
                StartChaseTarget();
            }
        }

        public override bool ExitState()
        {
            _navMeshAgent.isStopped = false;
            base.ExitState();

            Debug.Log("EXITING ATTACK STATE");
            return true;
        }
        private void StartChaseTarget()
        {
            //TO DO STOPPING DISTANCE??
            if (_npc.Destiantion() > _npc.attackRadius)
           {
              
              _fsm.EnterState(FSMStateType.CHASING);
           }
        }
        
    }
}