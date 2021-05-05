using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Enemy;

public class SendTriggerInfo : MonoBehaviour
{
    [SerializeField] private BaseAttack _base;
    [SerializeField] private int _ID;
    [SerializeField] private List<string> _targets;

    private void Awake()
    {
        if (_base == null)
            Debug.LogError("NEED A ATTACK BASE FOR " + gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(_targets[i]))
            {
                _base.CustomTriggerEnter(other, _ID);
            }
        }  
    }
    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(_targets[i]))
            {
                _base.CustomTriggerExit(other, _ID);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        for (int i = 0; i < _targets.Count; i++)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(_targets[i]))
            {
                _base.CustomTriggerStay(other, _ID);
            }
        }
    }
}
