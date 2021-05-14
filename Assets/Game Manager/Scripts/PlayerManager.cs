using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities.Scripts;

public class PlayerManager : Manager<PlayerManager>
{
    [Range(0f,1f)]
    [SerializeField] private float _percentHealthOnRespawn;
    [SerializeField] private float _maxDistanceToRevive = 20;
    private float _distancePlayers;
    private Coroutine c_revivalInProgress;
    private IReviveInput _reviveInput;
    //PLAYER MANAGEMENT
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
}
