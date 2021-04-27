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

	private FiniteStateMachine fsm;

    public void Start()
    {
		fsm = GetComponent<FiniteStateMachine>();
    }
    public void EntitiyHit(AttackSettings settings)
	{
		Debug.Log( gameObject.name + " LostHealth");
		RemoveHealth(settings._attackDamage);
		switch (settings._settingType)
		{
			case AttackSettings.SettingType.SLAM:
				_rillaSlamSettings = settings as RillaSlamSettings;
				if(_rillaSlamSettings._stun)
					fsm.EnterState(FSMStateType.STUN);
				break;
			case AttackSettings.SettingType.LAZOR:
				_zillaLazorSettings = settings as ZillaLazorSettings;
				break;
			default:
				break;
		}
	}

	private void RemoveHealth(float damage)
	{
		if (c_invincible == null)
		{
			if (_health <= 0)
			{
				fsm.EnterState(FSMStateType.DEATH);
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
