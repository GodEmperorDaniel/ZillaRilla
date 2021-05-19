using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Zilla;
using Entities.Scripts;

public class ZillaAttacks : BaseAttack
{
    //TODO Attack Destructible
    
    [SerializeField] private Animator _playerAnimator;
    
    [SerializeField] private ZillaTailSettings tailSettings;

    [SerializeField] private ZillaLazorSettings lazorSettings;

    private IZillaLazorInput _lazorInput;

    private List<GameObject> _listCanHitListTail = new List<GameObject>();
    private List<GameObject> _listCanHitListLazor = new List<GameObject>();
    private Coroutine c_attackCooldown;
    private Coroutine c_lazorGrowth;
    Ray ray;
    RaycastHit rayHit = new RaycastHit();
    bool hit = false;

    private void Awake()
    {
        _lazorInput = GetComponent<IZillaLazorInput>();
        tailSettings.playerIndex = 0;
        lazorSettings.playerIndex = 0;
    }

    public void ZillaLazor()
    {
        if (c_attackCooldown == null)
        {
            _playerAnimator.SetBool("ZillaLazorAttack", true);
            lazorSettings._attackHitbox.SetActive(true);

            if (c_lazorGrowth == null)
            {
                //doTheUpdateStuff = true;
                c_lazorGrowth = StartCoroutine(LazorAttack());
            }
        }
    }

    private void FixedUpdate()
    {
        Transform t = transform;
        Vector3 originOffsetVector = new Vector3(0, lazorSettings._attackHitbox.transform.position.y, 0);
        List<string> hitLayers = lazorSettings._layersThatInterup;

        ray = new Ray(t.position + originOffsetVector, t.forward);

        for (int i = 0; i < hitLayers.Count; i++)
        {
            float maxRayDistance = (lazorSettings._lazorMaxRange + 1.467f) * t.localScale.z;
            int hitLayer = LayerMask.GetMask(hitLayers[i]);
            hit = Physics.SphereCast(ray, lazorSettings._sphereCastRadius, out rayHit, maxRayDistance, hitLayer);
            if (hit)
            {
                break;
                //Debug.Log(hit + " " + rayHit.distance + " " + rayHit.collider.name + " " + lazorSettings._attackHitbox.transform.lossyScale.z);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ray);
    }

    private IEnumerator LazorAttack()
    {
        while (_lazorInput.LazorButtonPressed)
        {
            yield return new WaitForFixedUpdate();

            Vector3 hitBoxLocalScale = lazorSettings._attackHitbox.transform.localScale;
            Vector3 hitBoxLossyScale = lazorSettings._attackHitbox.transform.lossyScale;
            Vector3 growthVector = new Vector3(0, 0, lazorSettings._lazorGrowthPerSec * Time.deltaTime);
            float maxRange = lazorSettings._lazorMaxRange;

            float hitBoxOffset = lazorSettings._attackHitbox.transform.localPosition.z;
            float adjustedSphereRadius = lazorSettings._sphereCastRadius - hitBoxOffset * transform.localScale.z;
            float adjustedHitDistance = rayHit.distance + adjustedSphereRadius;
            float nextFrameDistance = hitBoxLossyScale.z + growthVector.z;

            if (!hit && hitBoxLocalScale.z < maxRange)
            {
                lazorSettings._attackHitbox.transform.localScale += growthVector;
                yield return null;
            }
            else if (hit && adjustedHitDistance > nextFrameDistance)
            {
                lazorSettings._attackHitbox.transform.localScale += growthVector;
                yield return null;
            }
            else if (hit && adjustedHitDistance < nextFrameDistance)
            {
                float lazorSettingsSphereCastRadius = (rayHit.distance + lazorSettings._sphereCastRadius - hitBoxOffset
                    * transform.localScale.z) / transform.lossyScale.z;
                lazorSettings._attackHitbox.transform.localScale = new Vector3(1, 1, lazorSettingsSphereCastRadius);
                yield return null;
            }

            for (int i = 0; i < _listCanHitListLazor.Count; i++)
            {
                if (_listCanHitListLazor[i] != null)
                {
                    //_listCanHitListLazor[i].GetComponent<Attackable>().EntitiyHit(lazorSettings);
                    CallEntityHit(_listCanHitListLazor[i], lazorSettings);
                }
            }

            yield return null;
        }

        lazorSettings._attackHitbox.transform.localScale = new Vector3(1, 1, 0.5f);
        lazorSettings._attackHitbox.SetActive(false);
        _playerAnimator.SetBool("ZillaLazorAttack", false);
        c_attackCooldown = StartCoroutine(AttackCooldown(lazorSettings._attackCooldown));
        c_lazorGrowth = null;
    }

    public void ZillaTailWhip()
    {
        if (c_attackCooldown != null) return;

        for (int i = 0; i < _listCanHitListTail.Count; i++)
        {
            if (_listCanHitListTail[i] == null) continue;

            if (_listCanHitListTail[i].layer == LayerMask.NameToLayer("Enemy"))
            { 
                //if (tailSettings._knockbackStrength > 0)
                //{
                //    Vector3 normDirection =
                //        (_listCanHitListTail[i].transform.position - transform.position).normalized;
                //    _listCanHitListTail[i].GetComponent<KnockBack>().ApplyKnockBack(normDirection, tailSettings._knockbackStrength, tailSettings._knockbackTime);   
                //}
                CallEntityHit(_listCanHitListTail[i], tailSettings);
            }
            else if (_listCanHitListTail[i].layer == LayerMask.NameToLayer("Destructible"))
                CallEntityHit(_listCanHitListTail[i], tailSettings);
            else
            {
                Vector3 normalizedDirection =
                    (_listCanHitListTail[i].transform.position - transform.position).normalized;
                ApplyForceToMovable(_listCanHitListTail[i], normalizedDirection);
            }
        }

        c_attackCooldown = StartCoroutine(AttackCooldown(tailSettings._attackCooldown));
    }

    private void CallEntityHit(GameObject enemy, AttackSettings settings)
    {
        //AddToComboMeter(0);
        enemy.GetComponent<Attackable>().EntitiyHit(settings);
    }

    #region TriggerData

    public override void CustomTriggerEnter(Collider other, int id)
    {
        switch (id)
        {
            case 1:
                _listCanHitListTail.Add(other.gameObject);
                break;
            case 2:
                _listCanHitListLazor.Add(other.gameObject);
                break;
            default:
                Debug.Log("Something went wrong in CustomTriggerEnter!");
                break;
        }
    }

    public override void CustomTriggerExit(Collider other, int id)
    {
        switch (id)
        {
            case 1:
                _listCanHitListTail.Remove(other.gameObject);
                break;
            case 2:
                _listCanHitListLazor.Remove(other.gameObject);
                break;
            default:
                Debug.Log("Something went wrong in CustomTriggerExit!");
                break;
        }
    }

    public override void CustomTriggerStay(Collider other, int id)
    {
        switch (id)
        {
            case 1:
                if (!_listCanHitListTail.Contains(other.gameObject))
                {
                    _listCanHitListTail.Add(other.gameObject);
                }

                break;
            case 2:
                if (!_listCanHitListLazor.Contains(other.gameObject))
                {
                    _listCanHitListLazor.Add(other.gameObject);
                }

                break;
            default:
                break;
        }
    }

    #endregion

    private IEnumerator AttackCooldown(float resetTime)
    {
        if (c_lazorGrowth != null)
            c_lazorGrowth = null;
        yield return new WaitForSeconds(resetTime);
        _playerAnimator.SetBool("ZillaTail", false);
        _playerAnimator.SetBool("ZillaLazor", false);
        _playerAnimator.SetBool("ZillaLazorWindup", false);
        c_attackCooldown = null;
    }

    public override void RemoveFromPlayerList(GameObject enemy)
    {
        if (_listCanHitListTail.Contains(enemy))
        {
            _listCanHitListTail.Remove(enemy);
        }

        if (_listCanHitListLazor.Contains(enemy))
        {
            _listCanHitListLazor.Remove(enemy);
        }
    }
}

#region Settings Structs

namespace Attacks.Zilla
{
    [System.Serializable]
    public class ZillaTailSettings : AttackSettings
    {
    }

    [System.Serializable]
    public class ZillaLazorSettings : AttackSettings
    {
        public float _lazorMaxRange;
        public float _lazorGrowthPerSec;
        public List<string> _layersThatInterup;

        public float _sphereCastRadius;
    }
}
#endregion