using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
	[SerializeField] private float health = 20;
	RillaSlamSettings _settings;

	public void EntitiyHit(AttackSettings settings)
	{
		//Debug.Log( gameObject.name + " LostHealth");
		RemoveHealth(settings._attackDamage);
		if (settings._settingType == AttackSettings.SettingType.SLAM)
		{
			_settings = settings as RillaSlamSettings;
			Debug.Log(_settings._stun);
		}

	}

	private void RemoveHealth(float damage)
	{
		health -= damage;
		//Debug.Log("RemovedHealth");
	}
}
