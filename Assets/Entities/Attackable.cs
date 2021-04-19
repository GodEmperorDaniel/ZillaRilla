using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
	[SerializeField] private float health = 20;

	public void EntitiyHit(AttackSettings settings)
	{
		RemoveHealth(settings._attackDamage);
		
	}

	private void RemoveHealth(float damage)
	{ 
	
	}
}
