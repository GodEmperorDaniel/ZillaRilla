using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Enemy;

public class EnemyAttacks : BaseAttack
{
    [SerializeField] private Animator _playerAnimator;

    [SerializeField] private EnemyPunchSettings punchSettings;
    [SerializeField] private EnemyShootinghSettings shootingSettings;

    private Coroutine c_attackCooldown;
    private HashSet<GameObject> _hashEnemyPunch = new HashSet<GameObject>();
    private HashSet<GameObject> _hashEnemyShoot = new HashSet<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        if (_playerAnimator == null)
        {
            Debug.LogWarning("No animator is set in " + gameObject.name + ", getting it through code");
            TryGetComponent<Animator>(out _playerAnimator);
        }
    }
    private void Update()
    {
        foreach (GameObject player in _hashEnemyShoot)
        {
            CallEntityHit(player, shootingSettings);
        }
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
            
            Rigidbody bulletClone = (Rigidbody)Instantiate(shootingSettings._bullet, shootingSettings._shootPosition.position, shootingSettings._bullet.transform.rotation);
            bulletClone.GetComponent<SendTriggerInfo>()._base = this;

            bulletClone.velocity = transform.forward * shootingSettings.bulletSpeed;
            
            c_attackCooldown = StartCoroutine(AttackCooldown(shootingSettings._attackCooldown));
        }
    }

    private IEnumerator AttackCooldown(float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        c_attackCooldown = null;
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
                _hashEnemyShoot.Add(other.gameObject);
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
                _hashEnemyShoot.Remove(other.gameObject);
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
                if (!_hashEnemyShoot.Contains(other.gameObject))
                {
                    _hashEnemyShoot.Add(other.gameObject);
                }
                break;
			default:
				break;
		}
	}
	#endregion
	private void CallEntityHit(GameObject player, AttackSettings settings)
    {
        player.GetComponent<Attackable>().EntitiyHit(settings);
    }

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


