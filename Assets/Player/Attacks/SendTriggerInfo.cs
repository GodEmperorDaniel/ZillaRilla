using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Enemy;

public class SendTriggerInfo : MonoBehaviour
{
    [SerializeField] private BaseAttack _base;
    [SerializeField] private int _ID;
    [SerializeField] private string _targetName;

    private void Awake()
    {
        if (_base == null)
            Debug.LogError("NEED A ATTACK BASE FOR " + gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetName))
        {
            _base.CustomTriggerEnter(other, _ID);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetName))
        {
            _base.CustomTriggerExit(other, _ID); 
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetName))
        {
            _base.CustomTriggerStay(other, _ID);
        }
    }
}
