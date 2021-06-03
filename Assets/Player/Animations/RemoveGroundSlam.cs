using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveGroundSlam : MonoBehaviour
{
	[SerializeField] private float _despawnTime;

	private void Start()
	{
		Destroy(gameObject, _despawnTime);
	}
}
