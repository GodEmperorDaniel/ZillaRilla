using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scrips
{
    using Entities.Scripts;
    using Entities.Commands;
    public class CharacterInput : MonoBehaviour, IMoveInput
    {
        [SerializeField]
        private Command _moveInput;

        [SerializeField]
        private PlayerInputActions _inputsActions;

        public Vector2 MoveDirection { get; private set; }

        private void Awake()
        {
            _inputsActions = new PlayerInputActions();
        }
        private void OnEnable()
        {
            _inputsActions.Enable();
            _inputsActions.Player.Jump.performed += OnJumpInput;
            _inputsActions.Player.Move.performed += OnMoveInput;
            _inputsActions.Player.Attack.performed += OnAttackInput;
            _inputsActions.Player.MouseAim.performed += OnMouseAimInput;
            _inputsActions.Player.AnalogAim.performed += OnAnalogAimInput;
        }


        private void OnAttackInput(InputAction.CallbackContext c)
        {
            //float value = c.ReadValue<float>();
        }
        private void OnMouseAimInput(InputAction.CallbackContext c)
        {
            //float value = c.ReadValue<float>();
        }
        private void OnAnalogAimInput(InputAction.CallbackContext c)
        {
            //float value = c.ReadValue<float>();
        }

        private void OnJumpInput(InputAction.CallbackContext c)
        {
            //float value = c.ReadValue<float>();
        }

        private void OnMoveInput(InputAction.CallbackContext c)
        {
            Vector2 value = c.ReadValue<Vector2>();
            Debug.Log(value);
            MoveDirection = value;
            _moveInput.Execute();
        }

        private void OnDisable()
        {
            _inputsActions.Disable();
            _inputsActions.Player.Jump.performed -= OnJumpInput;
            _inputsActions.Player.Move.performed -= OnMoveInput;
        }
    }
}