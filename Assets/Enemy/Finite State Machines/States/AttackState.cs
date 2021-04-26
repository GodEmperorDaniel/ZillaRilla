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
        private Coroutine c_attackCooldown;
        private HashSet<GameObject> _hashEnemiesPunch = new HashSet<GameObject>();

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
                StartChaseTarget();
            }
        }

        public override bool ExitState()
        {
            base.ExitState();

            Debug.Log("EXITING ATTACK STATE");
            return true;
        }
        public void EnemyPunch()
        {
            if (c_attackCooldown == null)
            {
                //Debug.Log("PUNCHING!!");
                //Debug.Log(_hashEnemiesPunch.Count);

                foreach (GameObject enemy in _hashEnemiesPunch)
                {
                    CallEntityHit(enemy, _npc.punchSettings);
                    //Debug.Log("I hit: " + enemy.name);
                }
                _npc._playerAnimator.SetBool("RillaPunch", false);
                c_attackCooldown = StartCoroutine(AttackCooldown(_npc.punchSettings._attackCooldown));
            }
        }
        private Coroutine StartCoroutine(IEnumerator enumerator)
        {
            throw new NotImplementedException();
        }

        private void StartChaseTarget()
        {
            //TO DO STOPPING DISTANCE??
           if (_npc.Destiantion > _npc.attackRadius)
           {
                _navMeshAgent.isStopped = false;
              _fsm.EnterState(FSMStateType.CHASING);
           }
        }
        private IEnumerator AttackCooldown(float resetTime)
        {
            yield return new WaitForSeconds(resetTime);
            c_attackCooldown = null;
        }
        private void CallEntityHit(GameObject player, AttackSettings settings)
        {
            player.GetComponent<Attackable>().EntitiyHit(settings);
        }
    }
}
#region Settings Structs
namespace Attacks.Enemy
{
    [System.Serializable]
    public class RillaPunchSettings : AttackSettings
    {
    }
    [System.Serializable]
    public class RillaSlamSettings : AttackSettings
    {
        public bool _stun;
        [Header("Knockback")]
        public bool _knockBack;
        public float _knockBackRange;
    }
}
#endregion