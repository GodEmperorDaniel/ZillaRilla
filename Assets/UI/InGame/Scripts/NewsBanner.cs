using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewsBanner : MonoBehaviour
{
    // TODO: Set text properly somehow
    // TODO: Store text?
    // TODO: Text rolling on screen (Activate when animation complete)
    // TODO: Signal when the text rolling is done

    private Animation _animation;
    private bool _bannerIsUp;

    [SerializeField] private TextMeshProUGUI _newsText;

    private void Start()
    {
        _animation = GetComponent<Animation>();
        
        
    }

    public void ActivateBanner(string text)
    {
        _animation.Play("NewsBannerUp");
        _newsText.SetText(text);
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

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("Set Text");
        }
    }
}
