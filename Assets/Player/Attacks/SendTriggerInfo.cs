using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTriggerInfo : MonoBehaviour
{
    [SerializeField] private BaseAttack _base;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _base.CustomTriggerEnter(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _base.CustomTriggerExit(other); 
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _base.CustomTriggerStay(other);
        }
    }
}
