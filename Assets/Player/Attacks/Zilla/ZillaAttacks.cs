using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Zilla;
using Entities.Scripts;

public class ZillaAttacks : BaseAttack
{

	[SerializeField] private Animator _playerAnimator;

	[SerializeField] private ZillaTailSettings tailSettings;

	[SerializeField] private ZillaLazorSettings lazorSettings;
	
	private IZillaLazorInput _lazorInput;

	private HashSet<GameObject> _hashEnemiesTail = new HashSet<GameObject>();
	private HashSet<GameObject> _hashEnemiesLazor = new HashSet<GameObject>();
	private Coroutine c_attackCooldown;
	private Coroutine c_lazorGrowth;

	private void Awake()
	{
		_lazorInput = GetComponent<IZillaLazorInput>();
	}
	public void ZillaLazor()
	{
		if (c_attackCooldown == null)
		{
			_playerAnimator.SetBool("ZillaLazor", true);
			lazorSettings._attackHitbox.SetActive(true);
			if (c_lazorGrowth == null)
			{ 
				c_lazorGrowth = StartCoroutine(LazorAttack());
			}
		}
	}
	private IEnumerator LazorAttack()
	{
		while (_lazorInput.LazorButtonPressed)
		{
			if (lazorSettings._attackHitbox.transform.localScale.z < lazorSettings._lazorMaxRange)
			{
				lazorSettings._attackHitbox.transform.localScale += new Vector3(0, 0, lazorSettings._lazorGrowthPerSec * Time.deltaTime);
			}
			foreach (GameObject enemy in _hashEnemiesLazor)
			{
				enemy.GetComponent<Attackable>().EntitiyHit(lazorSettings);
			}
			yield return null;
		}
		lazorSettings._attackHitbox.transform.localScale = new Vector3(0.2f,0.2f,0.5f);
		lazorSettings._attackHitbox.SetActive(false);
		_playerAnimator.SetBool("ZillaLazor", false);
		c_attackCooldown = StartCoroutine(AttackCooldown(lazorSettings._attackCooldown));
		c_lazorGrowth = null;
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
				_hashEnemiesLazor.Add(other.gameObject);
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
				_hashEnemiesLazor.Remove(other.gameObject);
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
				if (!_hashEnemiesLazor.Contains(other.gameObject))
				{
					_hashEnemiesLazor.Add(other.gameObject);
				}
				break;
			default:
				break;
		}
	}
	#endregion
	private IEnumerator AttackCooldown(float resetTime)
	{
		if (c_lazorGrowth != null)
			c_lazorGrowth = null;
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
		public float _lazorGrowthPerSec;
		//public bool _stun;
		//[Header("Knockback")]
		//public bool _knockBack;
		//public float _knockBackRange;
	}
}

#endregion