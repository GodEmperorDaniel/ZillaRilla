using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSettings
{
	public float _attackDamage;
	public GameObject _attackHitbox;
	public float _attackCooldown;
	public enum SettingType
	{
		PUNCH, SLAM, TAIL, LAZOR, SHOOTING
	}
	public SettingType _settingType;
}
