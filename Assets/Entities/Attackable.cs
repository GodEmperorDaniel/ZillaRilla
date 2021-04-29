using Attacks.Rilla;
using Assets.Enemy.Finite_State_Machines;
using Assets.Enemy.NPCCode;
using Attacks.Zilla;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attackable : MonoBehaviour
{
	[SerializeField] private float _health = 20;
	[SerializeField] private Animator animator;
	[SerializeField] private float _iFrames;
	private RillaSlamSettings _rillaSlamSettings;
	private ZillaLazorSettings _zillaLazorSettings;
	private Coroutine c_invincible;

	private FiniteStateMachine _fsm;
	private FSMStateType _fsmStateType;
	private NPC _npc;

	private AttackSettings _settings;

    public void Start()
    {
		
		_fsm = GetComponent<FiniteStateMachine>();
		if (TryGetComponent(out NPC npc))
		{
			_npc = GetComponent<NPC>();
		}
		
		
	}
    public void EntitiyHit(AttackSettings settings)
	{
		Debug.Log( gameObject.name + " LostHealth");
		switch (settings._settingType)
		{
			case AttackSettings.SettingType.SLAM:
				_rillaSlamSettings = settings as RillaSlamSettings;
				if (_rillaSlamSettings._stun)
					_fsm.EnterState(FSMStateType.STUN);
				if (_rillaSlamSettings._stun && _npc.enemyType == EnemyType.BOSS)
					_fsm.EnterState(FSMStateType.VULNERABLE);

				break;
			case AttackSettings.SettingType.LAZOR:
				_zillaLazorSettings = settings as ZillaLazorSettings;
				break;
			default:
				break;
		}
		RemoveHealth(settings._attackDamage);
	}

	private void RemoveHealth(float damage)
	{
		//Debug.Log("KOLLA HÄR: " + _fsm._currentState.StateType);
		if (_fsm._currentState.StateType == FSMStateType.VULNERABLE)
		{
			if (_zillaLazorSettings._settingType == AttackSettings.SettingType.LAZOR)
				if (_health <= 0)
				{
					_fsm.EnterState(FSMStateType.DEATH);
					animator.SetTrigger("Dead");
				}
				else
				{
					_health -= damage;
					_fsm.EnterState(FSMStateType.IDLE);
				}

		}
		if (c_invincible == null && _npc.enemyType != EnemyType.BOSS)
		{
			if (_health <= 0)
			{
				_fsm.EnterState(FSMStateType.DEATH);
				animator.SetTrigger("Dead");
			}
			else
			{
				_health -= damage;
			}
			Debug.Log("RemovedHealth");
			c_invincible = StartCoroutine(InvincibilityFrames());
		}
        
    }

    private IEnumerator InvincibilityFrames()
	{
		yield return new WaitForSeconds(_iFrames);
		c_invincible = null;
	}
}
