using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Enemy.NPCCode;
using UnityEngine;
using Attacks.Enemy;
using UnityEditor;

public class EnemyAttacks : BaseAttack
{
    [SerializeField] private Animator _enemyAnimator;

    [SerializeField] private EnemyPunchSettings punchSettings;
    [SerializeField] private EnemyShootinghSettings shootingSettings;

    private Coroutine c_attackCooldown;
    private HashSet<GameObject> _hashEnemyPunch = new HashSet<GameObject>();
    public List<GameObject> _listEnemyShoot = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        if (_enemyAnimator == null)
        {
            Debug.LogWarning("No animator is set in " + gameObject.name + ", getting it through code");
            TryGetComponent<Animator>(out _enemyAnimator);
        }
    }

    private void Update()
    {
        foreach (GameObject player in _listEnemyShoot)
        {
            if(player.layer == LayerMask.NameToLayer("Player"))
                CallEntityHit(player, shootingSettings);
        }
        _listEnemyShoot.Clear(); //fuck you or me 
    }

    public void EnemyPunch()
    {
        if (c_attackCooldown == null)
        {
            //Debug.Log("PUNCHING!!");
            //Debug.Log(_hashEnemiesPunch.Count);

            foreach (GameObject player in _hashEnemyPunch)
            {
                CallEntityHit(player, punchSettings);
                //Debug.Log("I hit: " + enemy.name);
            }

            //_playerAnimator.SetBool("RillaPunch", false);
            c_attackCooldown = StartCoroutine(AttackCooldown(punchSettings._attackCooldown));
        }
    }

    public void EnemyShoot()
    {
        if (c_attackCooldown == null)
        {
            Rigidbody bulletClone = (Rigidbody) Instantiate(shootingSettings._bullet,
            shootingSettings._shootPosition.position, shootingSettings._bullet.transform.rotation);
            bulletClone.GetComponent<SendTriggerInfo>()._base = this;
            bulletClone.GetComponent<Bullet>()._attacks = this;

            bulletClone.velocity = transform.forward * shootingSettings.bulletSpeed;

            c_attackCooldown = StartCoroutine(AttackCooldown(shootingSettings._attackCooldown));
        }
    }

    private IEnumerator AttackCooldown(float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        _enemyAnimator.SetBool("Attack", false);
        c_attackCooldown = null;
    }

    private void CallEntityHit(GameObject player, AttackSettings settings)
    {
        player.GetComponent<Attackable>().EntitiyHit(settings);
    }

    private RaycastHit hit;
    private Vector3 targetingOrigin;
    
    public bool IsPlayerInView(Transform player, NPC npc)
    {
        targetingOrigin = transform.position + Vector3.up * 3f;
        Vector3 direction = player.position - transform.position;
        string[] targetLayers = shootingSettings._bullet.GetComponent<SendTriggerInfo>().Targets.ToArray();
        LayerMask hitLayers = LayerMask.GetMask(targetLayers);
        
        bool playerInView = false;
        if (Physics.Raycast(targetingOrigin, direction, out  hit, npc.lookRadius, hitLayers))
        {
            playerInView = hit.collider.gameObject.layer == LayerMask.NameToLayer("Player");
        }

        return playerInView;
    }

    private void OnDrawGizmos()
    {
        if (hit.transform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(targetingOrigin, hit.point);
        }
    }

    #region TriggerData

    public override void CustomTriggerEnter(Collider other, int id)
    {
        switch (id)
        {
            case 1:
                _hashEnemyPunch.Add(other.gameObject);
                break;
            case 2:
                _listEnemyShoot.Add(other.gameObject);
                break;
            default:
                Debug.Log("Something whent wrong in CustomTriggerEnter!");
                break;
        }
    }

    public override void CustomTriggerExit(Collider other, int id)
    {
        switch (id)
        {
            case 1:
                _hashEnemyPunch.Remove(other.gameObject);
                break;
            case 2:
                _listEnemyShoot.Remove(other.gameObject);
                break;
            default:
                Debug.Log("Something whent wrong in CustomTriggerExit!");
                break;
        }
    }

    public override void CustomTriggerStay(Collider other, int id)
    {
        switch (id)
        {
            case 1:
                if (!_hashEnemyPunch.Contains(other.gameObject))
                {
                    _hashEnemyPunch.Add(other.gameObject);
                }

                break;
            case 2:
                if (!_listEnemyShoot.Contains(other.gameObject))
                {
                    _listEnemyShoot.Add(other.gameObject);
                }

                break;
            default:
                break;
        }
    }

    #endregion
}

#region Settings Structs

namespace Attacks.Enemy
{
    [System.Serializable]
    public class EnemyPunchSettings : AttackSettings
    {
    }

    [System.Serializable]
    public class EnemyShootinghSettings : AttackSettings
    {
        public Rigidbody _bullet;
        public float bulletSpeed = 10;
        public Transform _shootPosition;
    }
}

#endregion