using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    //TODO: Button Prompts (Tutorial and such)

    [SerializeField] private FillableBar _zillaHealthBar;
    [SerializeField] private FillableBar _rillaHealthBar;
    [SerializeField] private FillableBar _progressBar;
    [SerializeField] private FillableBar _reviveMeter;
    [SerializeField] private TextMeshProUGUI _reviveCountdownText;
    [SerializeField] private FillableBar _zillaComboMeter;
    [SerializeField] private TextMeshProUGUI _zillaComboText;
    [SerializeField] private FillableBar _rillaComboMeter;
    [SerializeField] private TextMeshProUGUI _rillaComboText;
    [SerializeField] private TextMeshProUGUI _currentObjective;
    [SerializeField] private NewsBanner _newsBanner;
    [SerializeField] private FillableBar _bossHealth;

    #region HealthBar
    public void ActivateHealthBars()
    {
        _zillaHealthBar.gameObject.SetActive(true);
        _rillaHealthBar.gameObject.SetActive(true);
    }

    public void DeactivateHealthBars()
    {
        _zillaHealthBar.gameObject.SetActive(false);
        _rillaHealthBar.gameObject.SetActive(false);
    }

    public void SetZillaHealthOnUI(float health)
    {
        _zillaHealthBar.fillAmount = health;
    }

    public void SetRillaHealthOnUI(float health)
    {
        _rillaHealthBar.fillAmount = health;
    }

    public void ChangeZillaFrame(Sprite newSprite)
    {
        _zillaHealthBar.ChangeFrame(newSprite);
    }

    public void ChangeRillaFrame(Sprite newSprite)
    {
        _rillaHealthBar.ChangeFrame(newSprite);
    }

    #endregion

    #region ProgressBar

    public void DeactivateProgressBar()
    {
        _progressBar.gameObject.SetActive(false);
    }

    public void ActivateProgressBar()
    {
        _progressBar.gameObject.SetActive(true);
    }

    public void SetProgressOnUI(float progress)
    {
        _progressBar.fillAmount = Round(progress, 2);
    }

    #endregion

    #region NewsBanner

    public void ActivateNewsBanner(string category, bool forced, int index)
    {
        _newsBanner.ActivateBanner(category, forced, index);
    }

    public void ActivateNewsBanner(string category, bool forced, string title)
    {
        _newsBanner.ActivateBanner(category, forced, title);
    }
    
    public void ActivateNewsBanner(string category, bool forced)
    {
        _newsBanner.ActivateBanner(category, forced);
    }

    public void ActivateNewsBannerRandom(string category, bool forced)
    {
        _newsBanner.ActivateBannerRandom(category, forced);
    }

    public void DeactivateNewsBanner()
    {
        _newsBanner.DeactivateBanner();
    }

    #endregion

    #region Revive
    public void ActivateReviveElements()
    {
        _reviveMeter.gameObject.SetActive(true);
    }
    public void DeactivateReviveElements()
    {
        _reviveMeter.gameObject.SetActive(false);
    }
    //public void ActivateReviveCountdown()
    //{
    //    _reviveCountdownText.gameObject.SetActive(true);
    //}
    //public void DeactivateReviveCountdown()
    //{
    //    _reviveCountdownText.gameObject.SetActive(false);
    //}
    public void SetReviveMeterOnUI(float progress)
    {
        _reviveMeter.fillAmount = Round(progress, 2);
    }

    public void SetCountdownTimeOnUI(string timeToShow)
    {
        _reviveCountdownText.SetText(timeToShow);
    }
    public void SetRevivePositionOnUI(Vector3 pos)
    {
        _reviveMeter.transform.position = pos;
    }
    #endregion

    #region ComboMeter

    public void SetZillaComboMeter(float percentFilled)
    {
        _zillaComboMeter.fillAmount = Round(percentFilled, 2);
    }

    public void SetRillaComboMeter(float percentFilled)
    {
        _rillaComboMeter.fillAmount = percentFilled;
    }

    public void SetZillaComboCounter(string combo)
    {
        _zillaComboText.SetText(combo);
    }

    public void SetRillaComboCounter(string combo)
    {
        _rillaComboText.SetText(combo);
    }
    #endregion
    #region BossHealth
    public void ActivateBossHealthOnUI()
    {
        _bossHealth.gameObject.SetActive(true);
    }
    public void DeactivateBossHealthOnUI()
    {
        _bossHealth.gameObject.SetActive(false);
    }
    public void SetHealthOnBossHealthBar(float progress)
    {
        _bossHealth.fillAmount = Round(progress, 2);
    }
	#endregion
	public void SetObjectiveOnUI(string objectiveName, string objectiveDescription) //I guess objectiveName Could be used somehow??
    {
        _currentObjective.SetText(objectiveDescription);
    }

    // Should be moved to a Math Utility class
    // Rounds float to the amount of digits
    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float) digits);
        return Mathf.Round(value * mult) / mult;
    }
}