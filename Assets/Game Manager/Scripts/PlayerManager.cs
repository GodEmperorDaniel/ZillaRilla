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
        UIManager.Instance.InGameUI.ActivateProgressBar();
        float percentageRevived;
        float i = revivalTarget._playerSettings._timeUntilDeath;
        while (i > 0)
        {
            if (_reviveInput.ReviveInputIsPressed)
            {
                percentageRevived =+ Time.deltaTime / revivalTarget._playerSettings._timeToRevive;
                UIManager.Instance.InGameUI.SetProgressOnUI(percentageRevived);
                if (percentageRevived >= 1)
                {
                    revivalTarget.ResetHealth();
                    c_revivalInProgress = null;
                }
            }
            else
            {
                UIManager.Instance.UpdateObjectiveOnUI("", i.ToString("F1"));
                i -= Time.deltaTime;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return null;
        revivalTarget._playerSettings._isReviving = false;
        c_revivalInProgress = null;
    }
}
