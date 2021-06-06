using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InGameUI : MonoBehaviour
{
    //TODO: Button Prompts (Tutorial and such)

    [SerializeField] private FillableBar _zillaHealthBar;
    [SerializeField] private Image _zillaHealPrompt;
    [SerializeField] private FillableBar _rillaHealthBar;
    [SerializeField] private Image _rillaHealPrompt;
    [SerializeField] private Sprite _ySprite;
    [SerializeField] private Sprite _triSprite;
    [SerializeField] private FillableBar _progressBar;
    [SerializeField] private FillableBar _reviveMeter;
    [SerializeField] private TextMeshProUGUI _reviveCountdownText;
    [SerializeField] private Image _reviveButtonPrompt;
    [SerializeField] private FillableBar _zillaComboMeter;
    [SerializeField] private TextMeshProUGUI _zillaComboText;
    [SerializeField] private FillableBar _rillaComboMeter;
    [SerializeField] private TextMeshProUGUI _rillaComboText;
    [SerializeField] private TextMeshProUGUI _currentObjective;
    [SerializeField] private NewsBanner _newsBanner;
    [SerializeField] private FillableBar _bossHealth;
    [SerializeField] private FillableBar _bossShield;

    [Range(0.0f, 100.0f)] [SerializeField] private float bannerChancePerSecond;
    [Range(0.0f, 100.0f)] [SerializeField] private float newsChanceEnemy;
    [Range(0.0f, 100.0f)] [SerializeField] private float newsChanceBuilding;
    private Coroutine _randomNewsActivatorCoroutine;


    public NewsBanner NewsBanner => _newsBanner;
    public float NewsChanceEnemy => newsChanceEnemy;
    public float NewsChanceBuilding => newsChanceBuilding;


    private void OnEnable()
    {
        _randomNewsActivatorCoroutine = StartCoroutine(RandomNewsActivator());
    }

    private void OnDisable()
    {
        StopCoroutine(_randomNewsActivatorCoroutine);
    }

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
        TestToSetZillaHealPrompt(health);
    }

    public void SetRillaHealthOnUI(float health)
    {
        _rillaHealthBar.fillAmount = health;
        TestToSetRillaHealPrompt(health);
    }

    public void ChangeZillaFrame(Sprite newSprite)
    {
        _zillaHealthBar.ChangeFrame(newSprite);
    }

    public void ChangeRillaFrame(Sprite newSprite)
    {
        _rillaHealthBar.ChangeFrame(newSprite);
    }

    private void TestToSetZillaHealPrompt(float health)
    {
        if (health <= 0.5f && PlayerManager.Instance._zillaActualMeterPercent > 0)
        {
            PlayerInput input = GameManager.Instance._zilla.GetComponent<PlayerInput>();
            for (int i = 0; i < input.devices.Count; i++)
            {
                if (input.devices[i].device.ToString() == "XInputControllerWindows:/XInputControllerWindows")
                {
                    _zillaHealPrompt.sprite = _ySprite;
                }
                else
                {
                    _zillaHealPrompt.sprite = _triSprite;
                }
            }
            _zillaHealPrompt.gameObject.SetActive(true);
        }
        else
        {
            _zillaHealPrompt.gameObject.SetActive(false);
        }
    }

    private void TestToSetRillaHealPrompt(float health)
    {
        if (health <= 0.5f && PlayerManager.Instance._rillaActualMeterPercent > 0)
        {
            PlayerInput input = GameManager.Instance._rilla.GetComponent<PlayerInput>();
            for (int i = 0; i < input.devices.Count; i++)
            {
                if (input.devices[i].device.ToString() == "XInputControllerWindows:/XInputControllerWindows")
                {
                    _rillaHealPrompt.sprite = _ySprite;
                }
                else
                {
                    _rillaHealPrompt.sprite = _triSprite;
                }
            }
            _rillaHealPrompt.gameObject.SetActive(true);
        }
        else
        {
            _rillaHealPrompt.gameObject.SetActive(false);
        }
    }

#endregion

#region ProgressBar

    public void DeactivateProgressBar()
    {
        Debug.Log("not active");
        _progressBar.gameObject.SetActive(false);
    }

    public void ActivateProgressBar()
    {
        Debug.Log("active");
        _progressBar.gameObject.SetActive(true);
    }

    public void SetProgressOnUI(float progress)
    {
        _progressBar.fillAmount = Round(progress, 2);
    }

#endregion

#region NewsBanner

    private IEnumerator RandomNewsActivator()
    {
        print("RandomNewsActivator Coroutine started");
        yield return new WaitWhile(() => _newsBanner.BannerIsActivated);
        yield return new WaitWhile(() => GameManager.Instance.GameIsPaused);
        ActivateNewsBannerRandom("News Blurb", false, bannerChancePerSecond);
        yield return new WaitForSeconds(1.0f);

        _randomNewsActivatorCoroutine = StartCoroutine(RandomNewsActivator());
    }

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

    public void ActivateNewsBannerRandom(string category, bool forced, float chance)
    {
        _newsBanner.ActivateBannerRandom(category, forced, chance);
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

    public void ChangeButtonPromptOnRevive(Sprite newSprite)
    {
        _reviveButtonPrompt.sprite = newSprite;
    }

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
#region BossHealthShield
    public void ActivateBossHealthOnUI()
    {
        _bossHealth.gameObject.SetActive(true);
    }

    public void DeactivateBossHealthOnUI()
    {
        _bossHealth.gameObject.SetActive(false);
    }
    public void SetHealthOnBossShieldBar(float progress)
    {
        _bossShield.fillAmount = Round(progress, 2);
    }
    public void SetHealthOnBossHealthBar(float progress)
    {
        _bossHealth.fillAmount = Round(progress, 2);
    }

#endregion

    public void
        SetObjectiveOnUI(string objectiveName,
            string objectiveDescription) //I guess objectiveName Could be used somehow??
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