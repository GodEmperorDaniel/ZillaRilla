using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FillableBar : MonoBehaviour
{
    //[HideInInspector]
    public float fillAmount;
    [SerializeField] private Image frame;
    [SerializeField] private Image fillImage;
    
    [SerializeField] private bool useText;
    [SerializeField] private Text percentText;
    
    private void Update()
    {
        fillImage.fillAmount = fillAmount;

        if (useText && percentText != null)
        {
            float actualPercent = fillAmount * 100;
            percentText.text = actualPercent + "%";
        }
    }
    public void ChangeFrame(Sprite newSprite)
    {
        if (frame)
        { 
            frame.sprite = newSprite;
        }
    }
}
