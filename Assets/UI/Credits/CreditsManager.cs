using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : Manager<CreditsManager>
{
    [SerializeField] private Animator _creditsAnimation;
    [SerializeField] private float DoubleClickMenuTime = 1;
    private Coroutine c_QuickPress;
    public void SpeedUp()
    {
        if (c_QuickPress != null)
        {
            GoBackToMainMenu();
        }
        else if (_creditsAnimation.speed == 1)
        {
            _creditsAnimation.speed = 4;
            c_QuickPress = StartCoroutine(QuickPressMenu());
        }
        else
        {
            _creditsAnimation.speed = 1;
            c_QuickPress = StartCoroutine(QuickPressMenu());
        }
    }
    public void ResetSpeed()
    {
        _creditsAnimation.speed = 1;
    }
    public void GoBackToMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
        Destroy(this.gameObject);
    }

    private IEnumerator QuickPressMenu()
    {
        yield return new WaitForSeconds(DoubleClickMenuTime);
        c_QuickPress = null;
    }
}
