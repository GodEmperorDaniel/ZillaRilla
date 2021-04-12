using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scrips
{
    using Entities.Scripts;
    using Entities.Commands;
    public class CharacterInput : MonoBehaviour, IMoveInput, IRotationInput
    {
        [SerializeField]
        private Command _moveInput;

        [SerializeField]
        private PlayerInputActions _inputsActions;

        public Vector3 MoveDirection { get; private set; }
        public Vector3 RotationDirection { get; set; }

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
            //probably a little bit of match and coordinate conversions... will fix later
            //Vector2 value = c.ReadValue<Vector2>();

        }
        private void OnAnalogAimInput(InputAction.CallbackContext c)
        {
            Vector2 value = c.ReadValue<Vector2>();
            RotationDirection = new Vector3(value.x, 0, value.y);
        }

        private void OnJumpInput(InputAction.CallbackContext c)
        {
            //float value = c.ReadValue<float>();
        }

        private void OnMoveInput(InputAction.CallbackContext c)
        {
            Vector2 value = c.ReadValue<Vector2>();
            MoveDirection = new Vector3(value.x, 0, value.y);
            _moveInput?.Execute();
        }

        private void OnDisable()
        {
            _inputsActions.Disable();
            _inputsActions.Player.Jump.performed -= OnJumpInput;
            _inputsActions.Player.Move.performed -= OnMoveInput;
        }
    }
}