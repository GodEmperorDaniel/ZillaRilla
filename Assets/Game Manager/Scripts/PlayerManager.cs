using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities.Scripts;

public class PlayerManager : Manager<PlayerManager>
{
    [Header("Revive")]
    [Range(0f,1f)]
    [SerializeField] private float _percentHealthOnRespawn;
    [SerializeField] private float _maxDistanceToRevive = 20;
    private float _distancePlayers;
    private Coroutine c_revivalInProgress;
    private IReviveInput _reviveInput;
    [Header("Combo-Meter")]
    [SerializeField] private float _neededToFillComboMeter = 1;
    [SerializeField] private float _timeToLoseCombo = 3;

    [Range(0.1f, 1f)]
    [SerializeField] private float _rageModeThreshhold = 1;
    [SerializeField] private float _newDamageMultiplier = 1.5f;

    private float _zillaMeterCounter = 0;
    [SerializeField] private Sprite _zillaRageFrame;
    [SerializeField] private Sprite _zillaOriginalFrame;
    private int _zillaCurrentComboCount = 0;
    private float _zillaActualMeterPercent;

    private float _rillaMeterCounter = 0;
    [SerializeField] private Sprite _rillaRageFrame;
    [SerializeField] private Sprite _rillaOriginalFrame;
    private int _rillaCurrentComboCount = 0;
    private float _rillaActualMeterPercent;

    private Coroutine c_zillaComboTimer;
    private Coroutine c_rillaComboTimer;

    private ZillaAttacks _zillaAttacks;
    private RillaAttacks _rillaAttacks;

    [Header("Healing")]
    private IHealInput _zillaHealInput;
    private Coroutine c_zillaHealthDelay;
    private IHealInput _rillaHealInput;
    private Coroutine c_rillaHealthDelay;
    [SerializeField] private float _healAmountPerComboMeterUnit = 1;

    protected override void Awake()
    {
        base.Awake();
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        _zillaHealInput = GameManager.Instance._zilla.GetComponent<IHealInput>();
        _rillaHealInput = GameManager.Instance._rilla.GetComponent<IHealInput>();
        _zillaAttacks = GameManager.Instance._zilla.GetComponent<ZillaAttacks>();
        _rillaAttacks = GameManager.Instance._rilla.GetComponent<RillaAttacks>();
    }
    //detta blir en rätt tung update :((
    private void Update()
    {
        UIManager.Instance.InGameUI.SetZillaComboCounter("x" + _zillaCurrentComboCount.ToString());
        UIManager.Instance.InGameUI.SetRillaComboCounter("x" + _rillaCurrentComboCount.ToString());

        _zillaActualMeterPercent = _zillaMeterCounter / _neededToFillComboMeter;
        UIManager.Instance.InGameUI.SetZillaComboMeter(_zillaActualMeterPercent);

        _rillaActualMeterPercent = _rillaMeterCounter / _neededToFillComboMeter;
        UIManager.Instance.InGameUI.SetRillaComboMeter(_rillaActualMeterPercent);

        if (_zillaActualMeterPercent >= _rageModeThreshhold)
        {
            UIManager.Instance.InGameUI.ChangeZillaFrame(_zillaRageFrame);
            _zillaAttacks.lazorSettings._damageMultiplier = _newDamageMultiplier;
            _zillaAttacks.tailSettings._damageMultiplier = _newDamageMultiplier;
        }
        else
        { 
            UIManager.Instance.InGameUI.ChangeZillaFrame(_zillaOriginalFrame);
            _zillaAttacks.lazorSettings._damageMultiplier = 1;
            _zillaAttacks.tailSettings._damageMultiplier = 1;
        }
        if (_rillaActualMeterPercent >= _rageModeThreshhold)
        {
            UIManager.Instance.InGameUI.ChangeRillaFrame(_rillaRageFrame);
            _rillaAttacks.punchSettings._damageMultiplier = _newDamageMultiplier;
            _rillaAttacks.slamSettings._damageMultiplier = _newDamageMultiplier;
        }
        else
        { 
            UIManager.Instance.InGameUI.ChangeRillaFrame(_rillaOriginalFrame);
            _rillaAttacks.punchSettings._damageMultiplier = 1;
            _rillaAttacks.slamSettings._damageMultiplier = 1;
        }
        if (_zillaHealInput.IHealPressed && c_zillaHealthDelay == null)
        {
            Debug.Log("HealingPlayer");
            c_zillaHealthDelay = StartCoroutine(HealingDelay(0,0.3f));
            StartCoroutine(HealPlayer(0));
        }
        if (_rillaHealInput.IHealPressed && c_rillaHealthDelay == null)
        {
            c_rillaHealthDelay = StartCoroutine(HealingDelay(1,0.3f));
            StartCoroutine(HealPlayer(1));
        }
    }
    //PLAYER MANAGEMENT
    #region Revive
    public void PlayerNeedsReviving(Attackable revivalTarget)
    {
        if (c_revivalInProgress == null)
        {
            if (revivalTarget == GameManager.Instance._zilla)
            {
                _reviveInput = GameManager.Instance._rilla.GetComponent<IReviveInput>();
            }
            else
            {
                _reviveInput = GameManager.Instance._zilla.GetComponent<IReviveInput>();
            }
            c_revivalInProgress = StartCoroutine(RevivalCountdown(revivalTarget));
        }
        else
            UIManager.Instance.UpdateObjectiveOnUI("", "YOU LOST");
    }
    private IEnumerator RevivalCountdown(Attackable revivalTarget)
    {
        UIManager.Instance.InGameUI.ActivateReviveBar();
        UIManager.Instance.InGameUI.ActivateReviveCountdown();
        float percentageRevived = 0;
        UIManager.Instance.InGameUI.SetReviveMeterOnUI(percentageRevived);
        float i = revivalTarget._playerSettings._timeUntilDeath;
        while (i > 0)
        {
            _distancePlayers = Vector3.Distance(GameManager.Instance._rilla.transform.position, GameManager.Instance._zilla.transform.position);
            if (_reviveInput.ReviveInputIsPressed && _distancePlayers < _maxDistanceToRevive)
            {
                percentageRevived += Time.deltaTime / revivalTarget._playerSettings._timeToRevive;
                UIManager.Instance.InGameUI.SetReviveMeterOnUI(percentageRevived);
                if (percentageRevived >= 1)
                {
                    revivalTarget.ResetHealth(_percentHealthOnRespawn);
                    c_revivalInProgress = null;
                    revivalTarget._playerSettings._isReviving = false;
                    break;
                }
                yield return null;
            }
            else
            {
                UIManager.Instance.InGameUI.SetCountdownTimeOnUI(i.ToString("F1"));
                i -= Time.deltaTime;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (i <= 0)
        {
            UIManager.Instance.UpdateObjectiveOnUI("","YOU LOST");
        }
        UIManager.Instance.InGameUI.DeactivateReviveBar();
        UIManager.Instance.InGameUI.DeactivateReviveCountdown();
        yield return null;
    }
    public void QuickRevivePlayer(Attackable revivalTarget)
    {
        StopCoroutine(c_revivalInProgress);
        c_revivalInProgress = null;
        UIManager.Instance.InGameUI.DeactivateReviveBar();
        UIManager.Instance.InGameUI.DeactivateReviveCountdown();
        revivalTarget.ResetHealth(_percentHealthOnRespawn);
        revivalTarget._playerSettings._isReviving = false;
    }
    #endregion
    #region PlayerCombo
    /// <summary>
    /// 0 = ZILLA, 1 = RILLA
    /// </summary>
    /// <param name="playerIndex"> 0 = ZILLA, 1 = RILLA </param>
    public void AddToPlayerCombo(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0:
                if(c_zillaComboTimer != null)
                    StopCoroutine(c_zillaComboTimer);
                if(_zillaActualMeterPercent < 1)
                    _zillaMeterCounter++;
                _zillaCurrentComboCount++;
                c_zillaComboTimer = StartCoroutine(ResetComboCounter(playerIndex));
                break;
            case 1:
                if(c_rillaComboTimer != null)
                    StopCoroutine(c_rillaComboTimer);
                if(_rillaActualMeterPercent < 1)
                    _rillaMeterCounter++;
                _rillaCurrentComboCount++;
                c_rillaComboTimer = StartCoroutine(ResetComboCounter(playerIndex));
                break;
            default:
                Debug.LogWarning("Nu blev något väldigt fel i AddToPlayerCombo i PlayerManager");
                break;
        }
    }
    public IEnumerator HealPlayer(int index)
    {
        switch (index)
        {
            case 0:
                if (_zillaActualMeterPercent > 0 && GameManager.Instance._zilla.GetHealthPercent() < 1)
                {
                    GameManager.Instance._zilla.HealPlayer(_healAmountPerComboMeterUnit);
                    _zillaMeterCounter--;
                }
                break;
            case 1:
                if (_rillaActualMeterPercent > 0 && GameManager.Instance._rilla.GetHealthPercent() < 1)
                {
                    GameManager.Instance._rilla.HealPlayer(_healAmountPerComboMeterUnit);
                    _rillaMeterCounter--;
                }
                break;
            default:
                Debug.Log("Something went wrong in HealPlayer in PlayerManager!");
                break;
        }
        yield return null;
    }
    private IEnumerator HealingDelay(int playerIndex, float healingDelay)
    {
        yield return new WaitForSeconds(healingDelay);
        switch (playerIndex)
        {
            case 0:
                c_zillaHealthDelay = null;
                break;
            case 1:
                c_rillaHealthDelay = null;
                break;
            default:
                Debug.LogWarning("SOMETHING WRONG IN PLAYER MANAGER!!");
                break;
        }

    }
    private IEnumerator ResetComboCounter(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0:
                yield return new WaitForSeconds(_timeToLoseCombo);
                _zillaCurrentComboCount = 0;
                c_zillaComboTimer = null;
                break;
            case 1:
                yield return new WaitForSeconds(_timeToLoseCombo);
                _rillaCurrentComboCount = 0;
                c_rillaComboTimer = null;
                break;
            default:
                Debug.LogWarning("Nu blev något väldigt fel in Ienumerator i PlayerManager");
                break;
        }
    }
    #endregion 
}
