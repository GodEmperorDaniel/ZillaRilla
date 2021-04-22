using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTriggerInfo : MonoBehaviour
{
    [SerializeField] private BaseAttack _base;
    [SerializeField] private int _ID;

    private void Awake()
    {
        if (_base == null)
            Debug.LogError("NEED A ATTACK BASE FOR " + gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _base.CustomTriggerEnter(other, _ID);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _base.CustomTriggerExit(other, _ID); 
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            _base.CustomTriggerStay(other, _ID);
        }
    }
}
