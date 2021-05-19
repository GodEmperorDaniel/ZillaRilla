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
    private float _zillaMeterPercent = 0;
    private int _zillaCurrentComboCount = 0;
    private float _rillaMeterPercent = 0;
    private int _rillaCurrentComboCount = 0;
    private Coroutine c_zillaComboTimer;
    private Coroutine c_rillaComboTimer;

    private void Update()
    {
        UIManager.Instance.InGameUI.SetZillaComboCounter("x" + _zillaCurrentComboCount.ToString());
        UIManager.Instance.InGameUI.SetRillaComboCounter("x" + _rillaCurrentComboCount.ToString());
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
            Debug.Log("YOU LOST");
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
                _zillaMeterPercent++;
                _zillaCurrentComboCount++;
                c_zillaComboTimer = StartCoroutine(ResetComboCounter(playerIndex));
                if (_zillaMeterPercent / _neededToFillComboMeter <= 1)
                    UIManager.Instance.InGameUI.SetZillaComboMeter(_zillaMeterPercent / _neededToFillComboMeter);
                break;
            case 1:
                if(c_rillaComboTimer != null)
                    StopCoroutine(c_rillaComboTimer);
                _rillaMeterPercent++;
                _rillaCurrentComboCount++;
                c_rillaComboTimer = StartCoroutine(ResetComboCounter(playerIndex));
                if(_zillaMeterPercent / _neededToFillComboMeter <= 1)
                    UIManager.Instance.InGameUI.SetRillaComboMeter(_rillaMeterPercent / _neededToFillComboMeter);
                break;
            default:
                Debug.LogWarning("Nu blev något väldigt fel i AddToPlayerCombo i PlayerManager");
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
