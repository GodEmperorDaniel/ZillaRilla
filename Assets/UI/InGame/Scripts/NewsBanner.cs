using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class NewsBanner : MonoBehaviour
{
#region Fields

    private Animation _animation;
    private string _animationCurrentlyPlaying;
    private float _animationTimeOnPause;
    private bool _animationPlaying;
    private bool _bannerIsActivated;
    private bool _bannerIsUp;
    private bool _textIsScrolling = false;

    private TextMeshProUGUI _textMesh;
    private RectMask2D _textMask;
    private RectTransform _textTransform;
    private int _currentLoop;

    public float textScrollSpeed;
    public int textLoops;
    [Range(0.0f, 100.0f)] public float defaultActivationChance = 50.0f;

    [SerializeField] private string xmlFileLocationDebug;

    private Dictionary<string, Dictionary<string, string>> _newsTextCategories;

#endregion

#region Getters/Setters

    public bool BannerIsActivated => _bannerIsActivated;

#endregion

#region Unity Methods

    private void Start()
    {
        string xmlFileLocation = Application.streamingAssetsPath + "/XML";
        _newsTextCategories = XMLLoader.GetXMLDictionary(xmlFileLocation);

        _animation = GetComponent<Animation>();
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        _textTransform = _textMesh.GetComponent<RectTransform>();
        _textMask = GetComponentInChildren<RectMask2D>();
    }

    private void Update()
    {
        if (_textIsScrolling && _bannerIsUp)
        {
            ScrollText();
        }
    }

#endregion

#region Banner Activation

    // Banner Activation by index, title or just category.
    // Forced will deactivate any activated banner and prioritize the next banner.
    private IEnumerator ActivationHandler(string category, string title, bool forced)
    {
        // Bug: If multiple forced try to activate while activating they will queue and play after each other
        if (_bannerIsActivated && !forced) yield break;
        if (_animation.isPlaying) yield return new WaitWhile(() => _animation.isPlaying);
        if (_bannerIsUp) DeactivateBanner();

        yield return new WaitWhile(() => _animation.isPlaying);

        _bannerIsActivated = true;
        Dictionary<string, string> newsContent = _newsTextCategories[category];
        StartCoroutine(InitializeText(newsContent[title]));
        _animation.Play("NewsBannerUp");
        _animationPlaying = true;
        _animationCurrentlyPlaying = "NewsBannerUp";
    }

    // Activate by category and title
    public void ActivateBanner(string category, bool forced, string title)
    {
        StartCoroutine(ActivationHandler(category, title, forced));
    }

    // Activate by category and index
    public void ActivateBanner(string category, bool forced, int index)
    {
        Dictionary<string, string> newsContent = _newsTextCategories[category];
        string title = newsContent.ElementAt(index).Key;
        StartCoroutine(ActivationHandler(category, title, forced));
    }

    // Activate category and use string from the category
    public void ActivateBanner(string category, bool forced)
    {
        int max = _newsTextCategories[category].Count;
        int index = Random.Range(0, max);

        ActivateBanner(category, forced, index);
    }

    // Activate with default chance value
    public void ActivateBannerRandom(string category, bool forced)
    {
        ActivateBannerRandom(category, forced, defaultActivationChance);
    }

    // Activate with a chance
    public void ActivateBannerRandom(string category, bool forced, float chance)
    {
        // Activate if randomized number is under the chance to activate
        float random = Random.Range(0.0f, 100.0f);
        if (chance < random) return;

        if (category == "News Blurb") print("News Blurb Activated!");
        int max = _newsTextCategories[category].Count;
        int index = Random.Range(0, max);

        ActivateBanner(category, forced, index);
    }

    public void DeactivateBanner()
    {
        _animation.Play("NewsBannerDown");
        _animationPlaying = true;
        _animationCurrentlyPlaying = "NewsBannerDown";
    }

    // Fix for banner animation on pause. Saves the animation position (time)
    // on pause and set it again on unpause.
    public void PauseBannerAnimation()
    {
        if (_animationPlaying)
        {
            _animationTimeOnPause = _animation[_animationCurrentlyPlaying].time;
        }
    }

    public void ResumeBannerAnimation()
    {
        if (_animationPlaying)
        {
            _animation.Play(_animationCurrentlyPlaying);
            _animation[_animationCurrentlyPlaying].time = _animationTimeOnPause;
        }
    }

#endregion

#region Internal Methods

    private void ActivateAnimationCompleted()
    {
        _animationPlaying = false;
        _bannerIsUp = true;
        _textIsScrolling = true;
        Debug.Log("Banner Activated");
    }

    private void DeactivateAnimationCompleted()
    {
        _animationPlaying = false;
        _bannerIsUp = false;
        _bannerIsActivated = false;
        Debug.Log("Banner Deactivated");
    }

    private void TextScrollCompleted()
    {
        DeactivateBanner();
    }

    private IEnumerator InitializeText(string text)
    {
        _textMesh.SetText(text);
        Canvas.ForceUpdateCanvases();

        yield return new WaitForEndOfFrame();

        SetTextAtStartPosition();
    }

    private void ScrollText()
    {
        Vector3 textPosition = _textTransform.localPosition;
        float textWidth = _textTransform.rect.width;
        float maskWidth = _textMask.canvasRect.width;
        float textEndOffset = -(textWidth + maskWidth) / 2;

        if (textPosition.x < textEndOffset)
        {
            _currentLoop++;
            SetTextAtStartPosition();
            textPosition = _textTransform.localPosition; // Update position variable
        }

        if (_currentLoop == textLoops)
        {
            _textIsScrolling = false;
            _currentLoop = 0;
            TextScrollCompleted();
            return;
        }

        float newXPos = textPosition.x - textScrollSpeed * Time.timeScale;
        textPosition.Set(newXPos, textPosition.y, textPosition.z);
        _textTransform.localPosition = textPosition;
    }

    private void SetTextAtStartPosition()
    {
        Vector3 textPosition = _textTransform.localPosition;
        float textWidth = _textTransform.rect.width;
        float maskWidth = _textMask.canvasRect.width;
        float textStartOffset = (textWidth + maskWidth) / 2;

        textPosition = new Vector3(textStartOffset, textPosition.y, textPosition.z);
        _textTransform.localPosition = textPosition;
    }

#endregion
}