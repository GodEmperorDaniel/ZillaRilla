using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewsBanner : MonoBehaviour
{
    // FIELDS
    private Animation _animation;
    private bool _bannerIsUp;
    private bool _textIsScrolling = false;

    [SerializeField] private TextMeshProUGUI _newsText;
    [SerializeField] private RectMask2D _textMask;
    private RectTransform _textTransform;
    private int _currentLoop;

    public float textScrollSpeed;
    public int textLoops;
    public float bannerActivationSpeed;
    public bool debugMode = false;

    private Dictionary<string, string> _xmlNewsText;

    
    // UNITY METHODS
    private void Start()
    {
        _xmlNewsText = XMLLoader.GetXMLDictionary("xml_text_test.xml");
        
        _animation = GetComponent<Animation>();
        _textTransform = _newsText.GetComponent<RectTransform>();
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
            if (!_bannerIsUp) _xmlNewsText = XMLLoader.GetXMLDictionary("xml_text_test.xml");;
        }
        if (Keyboard.current.rKey.wasPressedThisFrame && debugMode)
        {
            Debug.Log("XML Reloaded");
            if (!_bannerIsUp) _xmlNewsText = XMLLoader.GetXMLDictionary("xml_text_test.xml");;
        }
        
        if (Keyboard.current.numpad1Key.wasPressedThisFrame && debugMode)
        {
            Debug.Log("Text 1");
            if (!_bannerIsUp) ActivateBanner(0);
        }
        else if (Keyboard.current.numpad2Key.wasPressedThisFrame && debugMode)
        {
            Debug.Log("text 2");
            if (!_bannerIsUp) ActivateBanner(1);
        }
        else if (Keyboard.current.numpad3Key.wasPressedThisFrame && debugMode)
        {
            Debug.Log("Text 3");
            if (!_bannerIsUp) ActivateBanner(2);
        }
        else if (Keyboard.current.numpad4Key.wasPressedThisFrame && debugMode)
        {
            Debug.Log("Text 4");
            if (!_bannerIsUp) ActivateBanner(3);
        }
        
    }

    // PUBLIC METHODS
    public void ActivateBanner(int textIndex)
    {
        StartCoroutine(InitializeText(_xmlNewsText.ElementAt(textIndex).Value));
        _animation.Play("NewsBannerUp");
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
        _newsText.SetText(text);
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