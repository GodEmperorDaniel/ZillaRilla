using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : Manager<CreditsManager>
{
    [SerializeField] private Animator _creditsAnimation;
    public void SpeedUp()
    {
        if (_creditsAnimation.speed == 1)
        {
            _creditsAnimation.speed = 4;
        }
        else
        {
            _creditsAnimation.speed = 1;
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
}
