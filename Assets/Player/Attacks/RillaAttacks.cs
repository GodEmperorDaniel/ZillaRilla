using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RillaAttacks : BaseAttack
{
	[SerializeField] private Animator _playerAnimator;

	[SerializeField] private RillaPunchSettings punchSettings;

	[SerializeField] private RillaSlamSettings slamSettings;

	private HashSet<GameObject> _hashEnemiesPunch = new HashSet<GameObject>();
	private HashSet<GameObject> _hashEnemiesSlam = new HashSet<GameObject>();
	private Coroutine c_attackCooldown;
	private void Awake()
	{
		if (_playerAnimator == null)
		{ 
			Debug.LogWarning("No animator is set in " + gameObject.name + ", getting it through code");
			TryGetComponent<Animator>(out _playerAnimator);
		}
	}
	#region Attacks
	public void RillaPunch()
	{
		if (c_attackCooldown == null)
		{
			//punchSettings._attackHitbox.SetActive(true);
			//Debug.Log("PUNCHING!!");
			//Debug.Log(_hashEnemiesPunch.Count);

			foreach (GameObject enemy in _hashEnemiesPunch)
			{
				EntityHit(enemy, punchSettings);
				//Debug.Log("I hit: " + enemy.name);
			}
			_playerAnimator.SetBool("RillaPunch", false);
			c_attackCooldown = StartCoroutine(AttackCooldown(punchSettings._attackCooldown));
		}
	}

	public void RillaGroundSlam()
	{
		if (c_attackCooldown == null)
		{
			//Debug.Log("GroundSlam!!");
			slamSettings._attackHitbox.SetActive(true);
			foreach (GameObject enemy in _hashEnemiesSlam)
			{
				EntityHit(enemy, slamSettings);
				//Debug.Log("I hit: " + enemy.name);
			}
			_playerAnimator.SetBool("RillaSlam", false);
			c_attackCooldown = StartCoroutine(AttackCooldown(slamSettings._attackCooldown));
		}
	}
	private IEnumerator AttackCooldown(float resetTime)
	{
		yield return new WaitForSeconds(resetTime);
		//punchSettings._attackHitbox.SetActive(false);
		//slamSettings._attackHitbox.SetActive(false);
		//_hashEnemiesSlam.Clear();
		//_hashEnemiesPunch.Clear();
		c_attackCooldown = null;
	}
	#endregion
	#region TriggerData
	public override void CustomTriggerEnter(Collider other, int id) 
	{
		switch (id)
		{
			case 1:
			_hashEnemiesPunch.Add(other.gameObject);
				break;
			case 2:
				_hashEnemiesSlam.Add(other.gameObject);
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
				_hashEnemiesPunch.Remove(other.gameObject);
				break;
			case 2:
				_hashEnemiesSlam.Remove(other.gameObject);
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
				if (!_hashEnemiesPunch.Contains(other.gameObject))
				{
					_hashEnemiesPunch.Add(other.gameObject);
				}
				break;
			case 2:
				if (!_hashEnemiesSlam.Contains(other.gameObject))
				{
					_hashEnemiesSlam.Add(other.gameObject);
				}
				break;
			default:
				break;
		}
	}
	#endregion

	private void EntityHit(GameObject enemy, AttackSettings settings)
	{
		enemy.GetComponent<Attackable>().EntitiyHit(settings);
	}

}
#region Settings Structs
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
[System.Serializable]
public class AttackSettings
{
	public float _attackDamage;
	public GameObject _attackHitbox;
	public float _attackCooldown;
	public enum SettingType
	{
		PUNCH, SLAM
	}
	public SettingType _settingType;
}

#endregion