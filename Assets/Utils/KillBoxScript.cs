using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBoxScript : MonoBehaviour
{
    [SerializeField] private Vector3 _respawnOffset;
    private bool _fixZillaPos;
    private bool _fixRillaPos;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.gameObject == GameManager.Instance._zilla.gameObject)
            {
                _fixZillaPos = true;
            }
            else
            {
                _fixRillaPos = true;
            }
        }
        else
        {
            Debug.Log(other.name + " destroyed by killbox");    
            Destroy(other.gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (_fixRillaPos)
        {
            GameManager.Instance._rilla.gameObject.transform.position = GameManager.Instance._zilla.gameObject.transform.position + _respawnOffset;
            _fixRillaPos = false;
        }
        if (_fixZillaPos)
        {
            Debug.Log("Will fix zilla");
            GameManager.Instance._zilla.gameObject.transform.position = GameManager.Instance._rilla.gameObject.transform.position + _respawnOffset;
            _fixZillaPos = false;
        }
    }
}
