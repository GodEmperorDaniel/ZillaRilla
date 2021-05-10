using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewsBanner : MonoBehaviour
{
    // TODO: Store text somehow. Script? XML?

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

    public List<string> newsTexts;

    
    // UNITY METHODS
    private void Start()
    {
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
        if (Keyboard.current.spaceKey.wasPressedThisFrame && debugMode)
        {
            Debug.Log("SPACE!!");
            if (!_bannerIsUp) ActivateBanner(1);
        }
    }

    // PUBLIC METHODS
    public void ActivateBanner(int textIndex)
    {
        if (textIndex >= newsTexts.Count)
        {
            Debug.LogError("Index outside of List");
            return;
        }
        StartCoroutine(InitializeText(newsTexts[textIndex]));
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