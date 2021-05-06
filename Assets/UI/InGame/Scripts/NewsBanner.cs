using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewsBanner : MonoBehaviour
{
    // TODO: Set text properly somehow
    // TODO: Store text?
    // TODO: Text rolling on screen (Activate when animation complete)
    // TODO: Signal when the text rolling is done

    private Animation _animation;
    private bool _bannerIsUp;

    [SerializeField] private TextMeshProUGUI _newsText;
    [SerializeField] private RectMask2D _textMask;
    private RectTransform _textTransform;
    private ContentSizeFitter _textSizeFitter;

    private void Start()
    {
        _animation = GetComponent<Animation>();
        _textTransform = _newsText.GetComponent<RectTransform>();
        _textSizeFitter = _newsText.GetComponent<ContentSizeFitter>();
    }

    public void ActivateBanner(string text)
    {
        StartCoroutine(InitializeText(text));
        _animation.Play("NewsBannerUp");
    }

    public void DeactivateBanner()
    {
        _animation.Play("NewsBannerDown");
    }

    private void ActivateAnimationCompleted()
    {
        _bannerIsUp = true;
        Debug.Log("Banner Activated");
    }

    private void DeactivateAnimationCompleted()
    {
        _bannerIsUp = false;
        Debug.Log("Banner Deactivated");
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("SPACE!!");
            if (!_bannerIsUp) ActivateBanner("Test Text");
            else DeactivateBanner();
        }
    }

    private IEnumerator InitializeText(string text)
    {
        _newsText.SetText(text);
        Canvas.ForceUpdateCanvases();

        yield return new WaitForEndOfFrame();
        
        //Debug.Log("Text Bounds: " + _newsText.mesh.bounds);
        
        Vector3 textPosition = _textTransform.localPosition;
        float textWidth = _textTransform.rect.width;
        float maskWidth = _textMask.canvasRect.width;
        float textStartOffset = (textWidth + maskWidth) / 2;

        //Debug.Log("Text Width: " + textWidth + ", Mask Width: " + maskWidth + ", Text Start Offset: " + textStartOffset);
        
        textPosition = new Vector3(textStartOffset, textPosition.y, textPosition.z);
        _textTransform.localPosition = textPosition;
    }

  
}