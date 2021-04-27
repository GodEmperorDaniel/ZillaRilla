using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
	public virtual void CustomTriggerEnter(Collider other, int id) { }
	public virtual void CustomTriggerExit(Collider other, int id) { }
	public virtual void CustomTriggerStay(Collider other, int id) { }
}
