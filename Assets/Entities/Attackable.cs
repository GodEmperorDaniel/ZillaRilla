using Attacks.Rilla;
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

	public void EntitiyHit(AttackSettings settings)
	{
		Debug.Log( gameObject.name + " LostHealth");
		RemoveHealth(settings._attackDamage);
		if (settings._settingType == AttackSettings.SettingType.SLAM)
		{
			_settings = settings as RillaSlamSettings;
			//Debug.Log(_settings._stun);
		}

	}

	private void RemoveHealth(float damage)
	{
		_health -= damage;
		if (_health <= 0)
		{ 
			animator.SetBool("Dead", true);
		}
		//Debug.Log("RemovedHealth");
	}
}
