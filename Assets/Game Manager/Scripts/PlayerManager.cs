using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities.Scripts;

public class PlayerManager : Manager<PlayerManager>
{
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
            if (_reviveInput.ReviveInputIsPressed)
            {
                percentageRevived += Time.deltaTime / revivalTarget._playerSettings._timeToRevive;
                //Debug.Log(percentageRevived);
                UIManager.Instance.InGameUI.SetReviveMeterOnUI(percentageRevived);
                if (percentageRevived >= 1)
                {
                    revivalTarget.ResetHealth(0.4f);
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
