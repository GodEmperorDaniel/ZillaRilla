using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RillaAttacks : BaseAttack
{
	[SerializeField] private Animator _playerAnimator;

	[SerializeField] private RillaPunchSettings punchSettings;

	[SerializeField] private RillaSlamSettings slamSettings;

	private HashSet<GameObject> _hashEnemies = new HashSet<GameObject>();
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
			punchSettings._attackHitbox.SetActive(true);
			Debug.Log("PUNCHING!!");

			foreach (GameObject enemy in _hashEnemies)
			{
				HitEnemy(enemy, punchSettings);
				Debug.Log("I hit: " + enemy.name);
			}
			_playerAnimator.SetBool("RillaPunch", false);
			c_attackCooldown = StartCoroutine(AttackCooldown(punchSettings._attackCooldown));
		}
	}

	public void RillaGroundSlam()
	{
		if (c_attackCooldown == null)
		{
			Debug.Log("GroundSlam!!");
			slamSettings._attackHitbox.SetActive(true);
			foreach (GameObject enemy in _hashEnemies)
			{
				HitEnemy(enemy, slamSettings);
				Debug.Log("I hit: " + enemy.name);
			}
			_playerAnimator.SetBool("RillaSlam", false);
			c_attackCooldown = StartCoroutine(AttackCooldown(slamSettings._attackCooldown));
		}
	}
	private IEnumerator AttackCooldown(float resetTime)
	{
		yield return new WaitForSeconds(resetTime);
		punchSettings._attackHitbox.SetActive(false);
		slamSettings._attackHitbox.SetActive(false);
		_hashEnemies.Clear();
		c_attackCooldown = null;
	}
	#endregion
	#region TriggerData
	public override void CustomTriggerEnter(Collider other) 
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Debug.Log("STAY ENEMY ADDED");
			_hashEnemies.Add(other.gameObject);
		}
	}
	public override void CustomTriggerExit(Collider other) 
	{	
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			_hashEnemies.Remove(other.gameObject);
		}
	}
	public override void CustomTriggerStay(Collider other)
	{
		if (!_hashEnemies.Contains(other.gameObject))
		{
			_hashEnemies.Add(other.gameObject);
		}
	}
	#endregion

	private void HitEnemy(GameObject enemy, AttackSettings settings)
	{
		//enemy.GetComponent<Atackable>().HitEnemy(settings);
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
}

#endregion