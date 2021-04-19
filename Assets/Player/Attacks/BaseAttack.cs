using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
	public virtual void CustomTriggerEnter(Collider other) { }
	public virtual void CustomTriggerExit(Collider other) { }

	public virtual void CustomTriggerStay(Collider other) { }
}
