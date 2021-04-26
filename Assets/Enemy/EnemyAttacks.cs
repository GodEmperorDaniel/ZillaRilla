using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attacks.Enemy;

public class EnemyAttacks : BaseAttack
{
    [SerializeField] private Animator _playerAnimator;

    [SerializeField] private EnemyPunchSettings punchSettings;

    private Coroutine c_attackCooldown;
    private HashSet<GameObject> _hashPlayerPunch = new HashSet<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        if (_playerAnimator == null)
        {
            Debug.LogWarning("No animator is set in " + gameObject.name + ", getting it through code");
            TryGetComponent<Animator>(out _playerAnimator);
        }
    }

    public void EnemyPunch()
    {
        if (c_attackCooldown == null)
        {
            Debug.Log("PUNCHING!!");
            //Debug.Log(_hashEnemiesPunch.Count);

            foreach (GameObject player in _hashPlayerPunch)
            {
                CallEntityHit(player, punchSettings);
                //Debug.Log("I hit: " + enemy.name);
            }
            //_playerAnimator.SetBool("RillaPunch", false);
            c_attackCooldown = StartCoroutine(AttackCooldown(punchSettings._attackCooldown));
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
                _hashPlayerPunch.Add(other.gameObject);
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
                _hashPlayerPunch.Remove(other.gameObject);
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
				if (!_hashPlayerPunch.Contains(other.gameObject))
				{
                    _hashPlayerPunch.Add(other.gameObject);
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
    //[System.Serializable]
    //ublic class enemyshootsettings : attacksettings
    //{

    //}
}
#endregion
