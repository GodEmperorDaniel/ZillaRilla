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

	private List<GameObject> _listEnemiesTail = new List<GameObject>();
	private List<GameObject> _listEnemiesLazor = new List<GameObject>();
	private Coroutine c_attackCooldown;
	private Coroutine c_lazorGrowth;
	Ray ray;
	RaycastHit rayHit = new RaycastHit();
	bool hit = false;
	private void Awake()
	{
		_lazorInput = GetComponent<IZillaLazorInput>();
	}

	public void ZillaLazor()
	{
		if (c_attackCooldown == null)
		{
			_playerAnimator.SetBool("ZillaLazorAttack", true);
			lazorSettings._attackHitbox.SetActive(true);
			if (c_lazorGrowth == null)
			{
				//doTheUpdateStuff = true;
				c_lazorGrowth = StartCoroutine(LazorAttack());
			}
		}
	}
	private void FixedUpdate()
	{
		ray = new Ray(transform.position + new Vector3(0, lazorSettings._attackHitbox.transform.position.y, 0), transform.forward);
		for (int i = 0; i < lazorSettings._layersThatInterup.Count; i++)
		{
			hit = Physics.SphereCast(ray, lazorSettings._sphereCastRadius,out rayHit, (lazorSettings._lazorMaxRange + 1.467f) * transform.localScale.z , LayerMask.GetMask(lazorSettings._layersThatInterup[i]));
			if (hit)
			{ 
				//Debug.Log(hit + " " + rayHit.distance + " " + rayHit.collider.name + " " + lazorSettings._attackHitbox.transform.lossyScale.z);
			}
		}
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawRay(ray);
	}

	private IEnumerator LazorAttack()
	{
		while (_lazorInput.LazorButtonPressed)
		{
			yield return new WaitForFixedUpdate();
			if (!hit && lazorSettings._attackHitbox.transform.localScale.z < lazorSettings._lazorMaxRange)
			{
				lazorSettings._attackHitbox.transform.localScale += new Vector3(0, 0, lazorSettings._lazorGrowthPerSec * Time.deltaTime);
				yield return null;
			}
			else if (hit && (rayHit.distance + lazorSettings._sphereCastRadius - (1.467f * transform.localScale.z)) > lazorSettings._attackHitbox.transform.lossyScale.z + lazorSettings._lazorGrowthPerSec * Time.deltaTime)
			{
				lazorSettings._attackHitbox.transform.localScale += new Vector3(0, 0, lazorSettings._lazorGrowthPerSec * Time.deltaTime);
				yield return null;
			}
			else if (hit && (rayHit.distance + lazorSettings._sphereCastRadius - (1.467f * transform.localScale.z)) < lazorSettings._attackHitbox.transform.lossyScale.z + lazorSettings._lazorGrowthPerSec* Time.deltaTime)
			{
				lazorSettings._attackHitbox.transform.localScale = new Vector3(1, 1, (rayHit.distance + lazorSettings._sphereCastRadius - (1.467f * transform.localScale.z)) / (transform.lossyScale.z));
				yield return null;
			}
			for (int i = 0; i < _listEnemiesLazor.Count; i++)
			{
				if (_listEnemiesLazor[i] != null)
				{
					_listEnemiesLazor[i].GetComponent<Attackable>().EntitiyHit(lazorSettings);
				}
			}
			yield return null;
		}
		lazorSettings._attackHitbox.transform.localScale = new Vector3(1, 1, 0.5f);
		lazorSettings._attackHitbox.SetActive(false);
		_playerAnimator.SetBool("ZillaLazorAttack", false);
		c_attackCooldown = StartCoroutine(AttackCooldown(lazorSettings._attackCooldown));
		c_lazorGrowth = null;
	}

	public void ZillaTailWhip()
	{
		if (c_attackCooldown == null)
		{
			for (int i = 0; i < _listEnemiesTail.Count; i++)
			{
				if (_listEnemiesTail[i] != null)
				{
					if (_listEnemiesTail[i].layer == LayerMask.NameToLayer("Enemy"))
						CallEntityHit(_listEnemiesTail[i], tailSettings);
					else
					{
						AttackObject(_listEnemiesTail[i], (_listEnemiesTail[i].transform.position - transform.position).normalized);
					}
				}
			}
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
				_listEnemiesTail.Add(other.gameObject);
				break;
			case 2:
				_listEnemiesLazor.Add(other.gameObject);
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
				_listEnemiesTail.Remove(other.gameObject);
				break;
			case 2:
				_listEnemiesLazor.Remove(other.gameObject);
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
				if (!_listEnemiesTail.Contains(other.gameObject))
				{
					_listEnemiesTail.Add(other.gameObject);
				}
				break;
			case 2:
				if (!_listEnemiesLazor.Contains(other.gameObject))
				{
					_listEnemiesLazor.Add(other.gameObject);
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
		_playerAnimator.SetBool("ZillaTail", false);
		_playerAnimator.SetBool("ZillaLazor", false);
		_playerAnimator.SetBool("ZillaLazorWindup", false);
		c_attackCooldown = null;
	}
	public override void RemoveFromPlayerList(GameObject enemy)
	{
		if (_listEnemiesTail.Contains(enemy))
		{
			_listEnemiesTail.Remove(enemy);
		}
		if (_listEnemiesLazor.Contains(enemy))
		{
			_listEnemiesLazor.Remove(enemy);
		}
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
		public List<string> _layersThatInterup;
		public float _sphereCastRadius;
		//public bool _stun;
		//[Header("Knockback")]
		//public bool _knockBack;
		//public float _knockBackRange;
	}
}

#endregion