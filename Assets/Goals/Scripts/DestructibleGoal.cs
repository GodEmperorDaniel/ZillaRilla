using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleGoal : Goal
{
	[Range(0, 1)]
	[SerializeField] private float _percentOfHousesToDestroy = 1;
	[SerializeField] private List<DestructableBuilding> _listHousesToDestroy;
	private float _nrOfHousesToDestroyTotal;
	private float _nrOfHousesAlreadyDestroyed;
	private float _percentDestroyed;
	private bool _goalCalled;

	private void Awake()
	{
		_nrOfHousesToDestroyTotal = _listHousesToDestroy.Count;
	}
	private void Update()
	{
		for (int i = 0; i < _listHousesToDestroy.Count; i++)
		{
			if (!_listHousesToDestroy[i])
			{
				HouseDestroyed();
				_listHousesToDestroy.RemoveAt(i);
				break;
			}
			//fix arrow
		}
		if (!_goalCalled && _percentDestroyed >= _percentOfHousesToDestroy)
		{
			_goalCalled = true;
			Debug.Log("done the thing");
			GoalCompleted();
		}
	}
	private void HouseDestroyed()
	{
		_nrOfHousesAlreadyDestroyed++;
		_percentDestroyed = _nrOfHousesAlreadyDestroyed / _nrOfHousesToDestroyTotal;
	}
}
