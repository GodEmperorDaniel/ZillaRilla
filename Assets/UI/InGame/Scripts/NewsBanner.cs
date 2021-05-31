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
using UnityEngine.WSA;
using Random = UnityEngine.Random;

public class NewsBanner : MonoBehaviour
{
    // FIELDS
    private Animation _animation;
    private bool _bannerIsUp;
    private bool _textIsScrolling = false;

    private TextMeshProUGUI _textMesh;
    private RectMask2D _textMask;
    private RectTransform _textTransform;
    private int _currentLoop;

    public float textScrollSpeed;
    public int textLoops;
    public float bannerActivationSpeed;
    [SerializeField] private bool debugMode;

    [SerializeField] private string xmlFileLocation;
    [SerializeField] private string xmlFileLocationDebug;
    private Dictionary<string, Dictionary<string, string>> _newsTextCategories;

    //[SerializeField] private 


    // UNITY METHODS
    private void Start()
    {
#if DEBUG
        _newsTextCategories = XMLLoader.GetXMLDictionary(xmlFileLocationDebug);
#else
        debugMode = false
        _newsTextCategories = XMLLoader.GetXMLDictionary(xmlFileLocation);
#endif
        
        _animation = GetComponent<Animation>();
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        _textTransform = _textMesh.GetComponent<RectTransform>();
        _textMask = GetComponentInChildren<RectMask2D>();
    }

    private void Update()
    {
        if (_textIsScrolling)
        {
            ScrollText();
        }

        // DEBUG
        if (Keyboard.current.rKey.wasPressedThisFrame && debugMode)
        {
            Debug.Log("XML Reloaded");
            _newsTextCategories = XMLLoader.GetXMLDictionary(xmlFileLocation);
        }

        if (Keyboard.current.numpad1Key.wasPressedThisFrame && debugMode)
        {
            Debug.Log("Building Destruction Index");
            if (!_bannerIsUp) ActivateBanner("Building Destruction", 0);
        }
        else if (Keyboard.current.numpad2Key.wasPressedThisFrame && debugMode)
        {
            Debug.Log("Building Destruction Title");
            if (!_bannerIsUp) ActivateBanner("Building Destruction", "A lot!");
        }
        else if (Keyboard.current.numpad3Key.wasPressedThisFrame && debugMode)
        {
            Debug.Log("Enemy Killed Random");
            if (!_bannerIsUp) ActivateBannerRandom("Enemy Killed");
        }
    }


    // PUBLIC METHODS

    public void ActivateBanner(string category, int index)
    {
        
        Dictionary<string, string> newsContent = _newsTextCategories[category];
        StartCoroutine(InitializeText(newsContent.ElementAt(index).Value));
        _animation.Play("NewsBannerUp");
    }

    public void ActivateBanner(string category, string title)
    {
        Dictionary<string, string> newsContent = _newsTextCategories[category];
        StartCoroutine(InitializeText(newsContent[title]));
        _animation.Play("NewsBannerUp");
    }

    public void ActivateBannerRandom(string category)
    {
        // TODO Random chance to appear
        
        int max = _newsTextCategories[category].Count;
        int index = Random.Range(0, max);
        
        ActivateBanner(category, index);
    }

    public void DeactivateBanner()
    {
        _animation.Play("NewsBannerDown");
    }


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

}