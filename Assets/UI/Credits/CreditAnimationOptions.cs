using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditAnimationOptions : MonoBehaviour
{
    public void ResetSpeed()
    {
        CreditsManager.Instance.ResetSpeed();
    }

    public void LeaveToMainMenu()
    {
        CreditsManager.Instance.GoBackToMainMenu();
    }
}
