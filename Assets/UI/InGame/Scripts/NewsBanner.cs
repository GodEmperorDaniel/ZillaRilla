using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public delegate void ExampleDelegate(string parameter, bool aBool);

public class NewsBanner : MonoBehaviour
{
    // FIELDS
    private Animation _animation;
    private bool _bannerIsActivated;
    private bool _bannerIsUp;
    private bool _textIsScrolling = false;

    private TextMeshProUGUI _textMesh;
    private RectMask2D _textMask;
    private RectTransform _textTransform;
    private int _currentLoop;

    public float textScrollSpeed;
    public int textLoops;
    [Range(0.0f, 100.0f)] public float activationChance = 50.0f;

    [SerializeField] private string xmlFileLocationDebug;

    
    private Dictionary<string, Dictionary<string, string>> _newsTextCategories;


    // UNITY METHODS
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


#region Banner Activation

    // TODO Deactivate banner when forced
    // TODO Allow for sending activationChance through parameters

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

    // Activate with a chance
    public void ActivateBannerRandom(string category, float chance, bool forced)
    {
        // Activate if randomized number is under the chance to activate
        float random = Random.Range(0.0f, 100.0f);
        if (chance < random) return;

        int max = _newsTextCategories[category].Count;
        int index = Random.Range(0, max);

        ActivateBanner(category, forced, index);
    }

    // Activate with default chance value
    public void ActivateBannerRandom(string category, bool forced)
    {
        ActivateBannerRandom(category, activationChance, forced);
    }

    public void DeactivateBanner()
    {
        _animation.Play("NewsBannerDown");
    }
    
#endregion


    // INTERNAL METHODS
    private void ActivateAnimationCompleted()
    {
        _bannerIsUp = true;
        _textIsScrolling = true;
        Debug.Log("Banner Activated");
    }

    private void DeactivateAnimationCompleted()
    {
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

        float newXPos = textPosition.x - textScrollSpeed;
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


    void TestStart(ExampleDelegate activationDelegate, string s, bool b)
    {
        DelegateHandler(activationDelegate, s, b);
    }

    void DelegateHandler(ExampleDelegate activationDelegate, string s, bool b)
    {
        activationDelegate(s, b);
    }
    
}