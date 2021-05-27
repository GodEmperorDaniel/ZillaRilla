using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Enemy;

public class SendTriggerInfo : MonoBehaviour
{
    public BaseAttack _base;
    [SerializeField] private int _ID;
    [SerializeField] private List<string> _targets;

    public List<string> Targets
    {
        get => _targets;
        private set => _targets = value;
    }

    private void Awake()
    {

        if (_base == null)
        {
            _base = GetComponentInParent<EnemyAttacks>();
            //Debug.LogError("NEED A ATTACK BASE FOR " + gameObject.name);
            //Debug.LogWarning("MY PARENT IS: " + gameObject.name);
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        for (int i = 0; i < _targets.Count; i++)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(_targets[i]))
            {
                //Debug.Log(gameObject.name + " Added " + other.name);
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
                Debug.Log(gameObject.name + " Removed " + other.name);
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
