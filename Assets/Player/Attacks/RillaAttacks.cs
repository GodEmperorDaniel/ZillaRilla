using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RillaAttacks : MonoBehaviour
{
	[SerializeField]
	private Animator _playerAnimator;

	[Header("RillaPunch Settings")]
	[SerializeField] private float _punchDamage = 10;
	[SerializeField] private float _punchRange = 1;
	[SerializeField] private bool something; //fix this plox
	
	private void Awake()
	{
		if (_playerAnimator == null)
		{ 
			Debug.LogWarning("No animator is set in " + gameObject.name + ", getting it through code");
			TryGetComponent<Animator>(out _playerAnimator);
		}
	}
	//TODO: FIX 3D CONE FOR COLLIDER TO REST ON AND CHANGE FROM RAYCAST
	public void RillaPunch()
	{
		Debug.Log("PUNCH!!");

		RaycastHit hitEnemy; 
	    Physics.Raycast(transform.position, transform.forward, out hitEnemy, _punchRange, LayerMask.NameToLayer("Enemy"));

		if (hitEnemy.collider != null)
		{
			Debug.Log("I hit enemy");
		}
		_playerAnimator.SetBool("RillaPunch", false);
	}

	public void RillaGroundSlam()
	{
		Debug.Log("GroundSlam!!");
		_playerAnimator.SetBool("RillaSlam", false);
	}

	private void HitEnemy(Collider enemy)
	{ 
	
	}
}
