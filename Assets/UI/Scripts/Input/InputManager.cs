using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Manager<InputManager>
{
    [SerializeField] private UIInput _uiInput;
    [SerializeField] private Player.Scrips.CharacterInput _characterInput;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _uiInput.gameObject.SetActive(false);
        _characterInput.gameObject.SetActive(false);
    }
}
