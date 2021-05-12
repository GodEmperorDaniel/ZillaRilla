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
	[SerializeField] private float _currentHealth;
	
	[SerializeField] private Animator _animator;
	[SerializeField] private float _iFrames;
	[SerializeField] public Player.Settings.IfPlayer _playerSettings;
	private RillaSlamSettings _rillaSlamSettings;
	private ZillaLazorSettings _zillaLazorSettings;
	private Coroutine c_invincible;

	private FiniteStateMachine _fsm;
	private FSMStateType _fsmStateType;
	private NPC _npc;

	private AttackSettings _settings;

	private Player.Scrips.CharacterInput player;

	// UNITY METHODS
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
	private void Update()
	{
		if (player != null)
		{
			if (_currentHealth == 0 && !_playerSettings._isReviving)
			{
				//Debug.Log("It starts 0 health");
				//player.gameObject.SetActive(false);
				//Debug.Log("It sets inactive");
				//_playerSettings.respawnPoint.AddRespawnTarget(this);
				_playerSettings._isReviving = true;
				PlayerManager.Instance.PlayerNeedsReviving(this);
			}
			else
			{
				float healthPercent = _currentHealth / _maxHealth;
				switch (player.GetCharacter())
				{
					case Player.Scrips.CharacterInput.character.ZILLA:
						UIManager.Instance.UpdateZillaHealthOnUI(healthPercent);
						break;
					case Player.Scrips.CharacterInput.character.RILLA:
						UIManager.Instance.UpdateRillaHealthOnUI(healthPercent);
						break;
					default:
						break;
				}
			}
		}
	}

	// PUBLIC METHODS
	public void EntitiyHit(AttackSettings settings)
	{
		_rillaSlamSettings = null;
		_zillaLazorSettings = null;
		_animator = GetComponent<Animator>();
		
		switch (settings._settingType)
		{
			case AttackSettings.SettingType.SLAM:
				_rillaSlamSettings = settings as RillaSlamSettings;
				if (_fsm != null && _rillaSlamSettings._stun)
				{
					_fsm.EnterState(FSMStateType.STUN);
					if (_npc.enemyType == EnemyType.BOSS)
					{ 
						_fsm.EnterState(FSMStateType.VULNERABLE);
					}
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
	
	// INTERNAL METHODS
	private void RemoveHealth(float damage)
	{
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
			{
				if (_currentHealth <= 0)
				{
					_fsm.EnterState(FSMStateType.DEATH);
					_animator.SetTrigger("Dead");
				}
				else
				{
					Debug.Log("Damage done " + damage + "Current health " + _currentHealth);
					_currentHealth -= damage;
					_fsm.EnterState(FSMStateType.IDLE);
				}
			}
		}
		else if (c_invincible == null && player != null)
		{
			Debug.Log(" THIS ENEMYS GOT HANDS");
			_currentHealth -= damage;
			c_invincible = StartCoroutine(InvincibilityFrames());
		}
		else if (_currentHealth > 0.0f && gameObject.layer == LayerMask.NameToLayer("Destructible"))
		{
			Debug.Log("Destructible Damaged for " + damage + "HP");
			_currentHealth -= damage;
			if (_currentHealth <= 0.0f)
			{
				SendMessage("BuildingDestruction");
			}
		}
    }
	public void ResetHealth(float healthResetPercent = 0)
	{
		if (healthResetPercent == 0)
		{
			Debug.LogWarning("Because parameter was left at 0 health is reset to 100%");
			healthResetPercent = 1;
		}
		Debug.Log("reseting health");
		_currentHealth = healthResetPercent * _maxHealth;
	}
    private IEnumerator InvincibilityFrames()
	{
		yield return new WaitForSeconds(_iFrames);
		c_invincible = null;
	}
}
namespace Player.Settings
{
	[Serializable]
	public class IfPlayer
	{
		[Header("Revive")]
		public float _timeToRevive;
		public float _timeUntilDeath;
		public bool _isReviving;
	}
}
