using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFlagOfDeathScript : MonoBehaviour
{
	[SerializeField] private GameObject _flagPrefab;
	private GameObject _flagObject;
	public void CreateFlagOfDeath()
	{
		_flagObject = Instantiate(_flagPrefab, gameObject.transform);
	}

	public void RemoveFlagOfDeath()
	{
		if (_flagPrefab)
		{
			_flagPrefab = null;
			Destroy(_flagPrefab);
		}
	}
}
