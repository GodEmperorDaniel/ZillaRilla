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
	RillaSlamSettings _settings;

	FiniteStateMachine fsm;

    public void Start()
    {
		fsm = GetComponent<FiniteStateMachine>();
    }
    public void EntitiyHit(AttackSettings settings)
	{
		Debug.Log( gameObject.name + " LostHealth");
		RemoveHealth(settings._attackDamage);
		if (settings._settingType == AttackSettings.SettingType.SLAM)
		{
			_settings = settings as RillaSlamSettings;
			fsm.EnterState(FSMStateType.STUN);
		}
	}

	private void RemoveHealth(float damage)
	{
		_health -= damage;
		if (_health <= 0)
		{
			fsm.EnterState(FSMStateType.DEATH);
			animator.SetBool("Dead", true);
		}
		Debug.Log("RemovedHealth");
	}
}
