using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Zilla;

public class ZillaAttacks : BaseAttack
{

	[SerializeField] private Animator _playerAnimator;

	[SerializeField] private ZillaTailSettings tailSettings;

	[SerializeField] private ZillaLazorSettings lazorSettings;

	private HashSet<GameObject> _hashEnemiesTail = new HashSet<GameObject>();
	private Coroutine c_attackCooldown;

	public void ZillaLazor()
	{
		Debug.Log("IMA FIRING MAH LAZOR");
		_playerAnimator.SetBool("ZillaLazor", false);
	}

	public void ZillaTailWip()
	{
		if (c_attackCooldown == null)
		{
			foreach (GameObject enemy in _hashEnemiesTail)
			{
				CallEntityHit(enemy, tailSettings);
				//Debug.Log("I hit: " + enemy.name);
			}
			_playerAnimator.SetBool("ZillaTail", false);
			c_attackCooldown = StartCoroutine(AttackCooldown(tailSettings._attackCooldown));
		}
	}
	private void CallEntityHit(GameObject enemy, AttackSettings settings)
	{
		enemy.GetComponent<Attackable>().EntitiyHit(settings);
	}
	#region TriggerData
	public override void CustomTriggerEnter(Collider other, int id)
	{
		switch (id)
		{
			case 1:
				_hashEnemiesTail.Add(other.gameObject);
				break;
			case 2:
				//_hashEnemiesSlam.Add(other.gameObject);
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
				_hashEnemiesTail.Remove(other.gameObject);
				break;
			case 2:
				//_hashEnemiesSlam.Remove(other.gameObject);
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
				if (!_hashEnemiesTail.Contains(other.gameObject))
				{
					_hashEnemiesTail.Add(other.gameObject);
				}
				break;
			case 2:
				//if (!_hashEnemiesSlam.Contains(other.gameObject))
				//{
				//	_hashEnemiesSlam.Add(other.gameObject);
				//}
				break;
			default:
				break;
		}
	}
	#endregion
	private IEnumerator AttackCooldown(float resetTime)
	{
		yield return new WaitForSeconds(resetTime);
		c_attackCooldown = null;
	}
}

#region Settings Structs
namespace Attacks.Zilla
{
	[System.Serializable]
	public class ZillaTailSettings : AttackSettings
	{
	}
	[System.Serializable]
	public class ZillaLazorSettings : AttackSettings
	{
		public float _lazorMaxRange;
		//public bool _stun;
		//[Header("Knockback")]
		//public bool _knockBack;
		//public float _knockBackRange;
	}
}

#endregion