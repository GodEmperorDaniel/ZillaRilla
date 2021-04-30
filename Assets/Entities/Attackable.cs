using Attacks.Rilla;
using Assets.Enemy.Finite_State_Machines;
using Assets.Enemy.NPCCode;
using Attacks.Zilla;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attackable : MonoBehaviour
{
	[SerializeField] private float _maxHealth;
	[SerializeField] private float _currentHealth = 20;
	[SerializeField] private Animator _animator;
	[SerializeField] private float _iFrames;
	private RillaSlamSettings _rillaSlamSettings;
	private ZillaLazorSettings _zillaLazorSettings;
	private Coroutine c_invincible;

	private FiniteStateMachine _fsm;
	private FSMStateType _fsmStateType;
	private NPC _npc;

	private AttackSettings _settings;

	private Player.Scrips.CharacterInput player;

	private void Awake()
	{
		_currentHealth = _maxHealth;
	}
	public void Start()
    {
		TryGetComponent<FiniteStateMachine>(out _fsm);
		TryGetComponent<Player.Scrips.CharacterInput>(out player);
		TryGetComponent<NPC>(out _npc);
		
	}
    public void EntitiyHit(AttackSettings settings)
	{
		//Debug.Log( gameObject.name + " LostHealth");
		_animator = GetComponent<Animator>();

		switch (settings._settingType)
		{
			case AttackSettings.SettingType.SLAM:
				_rillaSlamSettings = settings as RillaSlamSettings;
				if (_rillaSlamSettings._stun)
					_fsm.EnterState(FSMStateType.STUN);
				if (_rillaSlamSettings._stun && _npc.enemyType == EnemyType.BOSS)
				{ 
					_fsm.EnterState(FSMStateType.VULNERABLE);
					//Debug.Log("vuln");
				}
				break;
			case AttackSettings.SettingType.LAZOR:
				_zillaLazorSettings = settings as ZillaLazorSettings;
				break;
			default:
				break;
		}
		RemoveHealth(settings._attackDamage);
	}
	private void Update()
	{
		if (player != null)
		{
			switch (player.GetCharacter())
			{
				case Player.Scrips.CharacterInput.character.ZILLA:
					UIManager.Instance.UpdateZillaHealthOnUI(_currentHealth / _maxHealth);
					break;
				case Player.Scrips.CharacterInput.character.RILLA:
					UIManager.Instance.UpdateRillaHealthOnUI(_currentHealth / _maxHealth);
					break;
				default:
					break;
			}
		}
	}

	private void RemoveHealth(float damage)
	{
		//Debug.Log("KOLLA H�R: " + _fsm._currentState.StateType);
		if (c_invincible == null && _npc != null && _npc.enemyType != EnemyType.BOSS)
		{
			if (_currentHealth <= 0)
			{
				if (_fsm != null)
				{
					_fsm.EnterState(FSMStateType.DEATH);
				}
				_animator.SetTrigger("Dead");
			}
			else
			{
				_currentHealth -= damage;
				UIManager.Instance.SpawnHitIcon(gameObject.transform.position);
			}
			//Debug.Log("RemovedHealth");
			c_invincible = StartCoroutine(InvincibilityFrames());
		}
		else if (_fsm != null && _fsm._currentState.StateType == FSMStateType.VULNERABLE)
		{
			if (_zillaLazorSettings != null && _zillaLazorSettings._settingType == AttackSettings.SettingType.LAZOR)
				if (_currentHealth <= 0)
				{
					_fsm.EnterState(FSMStateType.DEATH);
					_animator.SetTrigger("Dead");
				}
				else
				{
					_currentHealth -= damage;
					_fsm.EnterState(FSMStateType.IDLE);
				}
		}
		else if (player != null)
		{
			_currentHealth -= damage;
		}
    }

    private IEnumerator InvincibilityFrames()
	{
		yield return new WaitForSeconds(_iFrames);
		c_invincible = null;
	}
}
