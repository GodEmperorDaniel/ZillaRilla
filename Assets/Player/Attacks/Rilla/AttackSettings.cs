using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSettings
{
	[HideInInspector]
	public int playerIndex;
	public float _attackDamage;
	public float _damageMultiplier = 1;
	public GameObject _attackHitbox;
	public float _attackCooldown;
	public enum SettingType
	{
		PUNCH, SLAM, TAIL, LAZOR, SHOOTING
	}
	public SettingType _settingType;
	public float _knockbackStrength;
	public float _knockbackTime;
}
