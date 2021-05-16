using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Rilla;


public class RillaAttacks : BaseAttack
{
	[SerializeField] private Animator _playerAnimator;

	[SerializeField] private RillaPunchSettings punchSettings;

	[SerializeField] private RillaSlamSettings slamSettings;

	private List<GameObject> _listPunch = new List<GameObject>();
	private List<GameObject> _listSlam = new List<GameObject>();
	private Coroutine c_attackCooldown;
	private void Awake()
	{
		punchSettings.playerIndex = 1;
		slamSettings.playerIndex = 1;
		if (_playerAnimator == null)
		{
			Debug.LogWarning("No animator is set in " + gameObject.name + ", getting it through code");
			TryGetComponent<Animator>(out _playerAnimator);
		}
	}
	#region Attacks
	public void RillaPunch()
	{
		if (c_attackCooldown != null) return;
		
		for (int i = 0; i < _listPunch.Count; i++)
		{
			if (_listPunch[i] == null) continue; 
			
			if (_listPunch[i].layer == LayerMask.NameToLayer("Enemy"))
				CallEntityHit(_listPunch[i], punchSettings);
			else if (_listPunch[i].layer == LayerMask.NameToLayer("Destructible"))
				CallEntityHit(_listPunch[i], punchSettings);
			else
			{
				ApplyForceToMovable(_listPunch[i], (_listPunch[i].transform.position - transform.position).normalized);
			}
		}
		c_attackCooldown = StartCoroutine(AttackCooldown(punchSettings._attackCooldown));
	}

	public void RillaGroundSlam()
	{
		if (c_attackCooldown == null)
		{
			for (int i = 0; i < _listSlam.Count; i++)
			{
				if (_listSlam[i] != null)
				{
					if (_listSlam[i].layer == LayerMask.NameToLayer("Enemy"))
						CallEntityHit(_listSlam[i], slamSettings);
					else if (_listSlam[i].layer == LayerMask.NameToLayer("Destructible"))
						CallEntityHit(_listSlam[i], slamSettings);
					else
					{
						ApplyForceToMovable(_listSlam[i], (_listSlam[i].transform.position - transform.position).normalized);
					}
				}
			}
			c_attackCooldown = StartCoroutine(AttackCooldown(slamSettings._attackCooldown));
		}
	}
	private IEnumerator AttackCooldown(float resetTime)
	{
		yield return new WaitForSeconds(resetTime);
		_playerAnimator.SetBool("RillaPunch", false);
		_playerAnimator.SetBool("RillaSlam", false);
		_playerAnimator.SetBool("Rilla_Left_Punch", false);
		_playerAnimator.SetBool("Rilla_Right_Punch", false);
		c_attackCooldown = null;
	}
	#endregion
	#region TriggerData
	public override void CustomTriggerEnter(Collider other, int id)
	{
		switch (id)
		{
			case 1:
				_listPunch.Add(other.gameObject);
				break;
			case 2:
				_listSlam.Add(other.gameObject);
				break;
			default:
				Debug.Log("Something whent wrong in CustomTriggerEnter!");
				break;
		}

	}
	public override void CustomTriggerExit(Collider other, int id)
	{
		switch (id)
		{
			case 1:
				_listPunch.Remove(other.gameObject);
				break;
			case 2:
				_listSlam.Remove(other.gameObject);
				break;
			default:
				Debug.Log("Something whent wrong in CustomTriggerExit!");
				break;
		}
	}
	public override void CustomTriggerStay(Collider other, int id)
	{
		switch (id)
		{
			case 1:
				if (!_listPunch.Contains(other.gameObject))
				{
					_listPunch.Add(other.gameObject);
				}
				break;
			case 2:
				if (!_listSlam.Contains(other.gameObject))
				{
					_listSlam.Add(other.gameObject);
				}
				break;
			default:
				break;
		}
	}
	#endregion

	private void CallEntityHit(GameObject enemy, AttackSettings settings)
	{
		//AddToComboMeter(1);
		enemy.GetComponent<Attackable>().EntitiyHit(settings);
	}

	public override void RemoveFromPlayerList(GameObject enemy) //this can be removed and all its referenses!!
	{
		if (_listPunch.Contains(enemy))
		{
			_listPunch.Remove(enemy);
		}
		if (_listSlam.Contains(enemy))
		{
			_listSlam.Remove(enemy);
		}
	}

}

#region Settings Structs
namespace Attacks.Rilla
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
////[System.Serializable]
//public class AttackSettings
//{
//	public float _attackDamage;
//	public GameObject _attackHitbox;
//	public float _attackCooldown;
//	public enum SettingType
//	{
//		PUNCH, SLAM
//	}
//	public SettingType _settingType;
//}

#endregion