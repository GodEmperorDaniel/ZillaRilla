using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleGoal : Goal
{
	[Range(0, 1)]
	[SerializeField] private float percentOfHousesToDestroy = 1;
	[SerializeField] private List<DestructableBuilding> housesToDestroy;
	
}
