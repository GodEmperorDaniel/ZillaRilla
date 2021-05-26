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
            if (!EnteredState) return;
            
            if (_npc.enemyType == EnemyType.RANGE)
            {
                // else move towards target
                List<Transform> targets = _npc.GetPlayerList;
                EnemyAttacks enemyAttacks = _npc.GetEnemyAttack;
                List<float> distance = new List<float>();
                Debug.Log(targets.Count);
                for (int i = 0; i < targets.Count; i++)
                {
                    distance.Add(Vector3.Distance(targets[i].position, _npc.transform.position));
                }
                //float distance0 = Vector3.Distance(targets[0].position, _npc.transform.position);
                //float distance1 = Vector3.Distance(targets[1].position, _npc.transform.position);

                //Debug.Log("Current target: " + _npc.PlayerTransform.name);
                if (targets.Count == 2)
                {
                    _npc.ClosestPlayerDistance(out Transform closestTarget);
                    if (closestTarget != _npc.PlayerTransform && enemyAttacks.IsPlayerInView(closestTarget, _npc))
                    {
                        _npc.PlayerTransform = closestTarget;
                    }
                    if (enemyAttacks.IsPlayerInView(_npc.PlayerTransform, _npc))
                    {
                        // Attacks if current target i visible
                        _npc._animator.SetBool("Attack", true);
                        //enemyAttacks.EnemyShoot();
                    }
                    else if (distance[0] < _npc.attackRadius && distance[1] < _npc.attackRadius)
                    {
                        //Switches target if the other is in range
                        if (_npc.PlayerTransform == targets[1] && enemyAttacks.IsPlayerInView(targets[0], _npc))
                            _npc.PlayerTransform = _npc.GetPlayerList[0];
                        else if (enemyAttacks.IsPlayerInView(targets[1], _npc))
                            _npc.PlayerTransform = _npc.GetPlayerList[1];
                        else
                            _fsm.EnterState(FSMStateType.CHASING);
                    }
                    else
                    {
                        _fsm.EnterState(FSMStateType.CHASING);
                    }
                }
                else
                {
                    if (enemyAttacks.IsPlayerInView(targets[0], _npc))
                    {
                        _npc._animator.SetBool("Attack", true);
                    }
                    else
                        _fsm.EnterState(FSMStateType.IDLE);
                }
            }
            else
            {
                //_npc._animator.SetTrigger("Attack");
                _npc.GetEnemyAttack.EnemyPunch();
            }

            _npc.FaceTarget(_npc.PlayerTransform);
            //Debug.Log("UPDATING ATTACK STATE");
            StartChaseTarget();
            //if (_npc.enemyType == EnemyType.MELEE)
                
        }

        public override bool ExitState()
        {
            _navMeshAgent.isStopped = false;
            base.ExitState();
            _npc.RemoveTarget();
            Debug.Log("EXITING ATTACK STATE");
            return true;
        }

        private void StartChaseTarget()
        {
            //TO DO STOPPING DISTANCE??
            if (_npc.ClosestPlayerDistance() > _npc.attackRadius)
            {
                _fsm.EnterState(FSMStateType.CHASING);
            }
        }
    }
}