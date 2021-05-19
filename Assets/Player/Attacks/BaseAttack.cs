using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
	public virtual void CustomTriggerEnter(Collider other, int id) { }
	public virtual void CustomTriggerExit(Collider other, int id) { }
	public virtual void CustomTriggerStay(Collider other, int id) { }

	public virtual void RemoveFromPlayerList(GameObject enemy){ }

	protected void ApplyForceToMovable(GameObject GO, Vector3 direction)
	{
		Rigidbody rb = GO.GetComponent<Rigidbody>();
		rb.AddForce(direction * 10, ForceMode.Impulse);
	}
	/// <summary>
	/// 0 = ZILLA, 1 = RILLA
	/// </summary>
	/// <param name="playerIndex"> 0 = ZILLA, 1 = RILLA </param>
	//protected void AddToComboMeter(int playerIndex)
	//{
	//	Debug.Log("Combo:d");
	//	PlayerManager.Instance.AddToPlayerCombo(playerIndex);
	//}
}
