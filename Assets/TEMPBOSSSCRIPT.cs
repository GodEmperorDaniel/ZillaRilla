using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPBOSSSCRIPT : MonoBehaviour
{
	[SerializeField] Goal goal;
	private void OnDestroy()
	{
		goal.GoalCompleted();
	}
}
