using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Zilla;

public class ZillaAttacks : MonoBehaviour
{

	[SerializeField] private Animator _playerAnimator;

	[SerializeField] private ZillaTailSettings tailSettings;

	[SerializeField] private ZillaLazorSettings lazorSettings;

	private HashSet<GameObject> _hashEnemiesTail = new HashSet<GameObject>();
	private Coroutine c_attackCooldown;

	public void ZillaLazor()
	{ 
		
	}

	public void ZillaTailWip()
	{ 
		
	}
	private void CallEntityHit(GameObject enemy, AttackSettings settings)
	{
		enemy.GetComponent<Attackable>().EntitiyHit(settings);
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
	//[System.Serializable]
	//public class AttackSettings
	//{
	//	public float _attackDamage;
	//	public GameObject _attackHitbox;
	//	public float _attackCooldown;
	//	public enum SettingType
	//	{
	//		TAIL, LAZOR
	//	}
	//	public SettingType _settingType;
	//}

	#endregion