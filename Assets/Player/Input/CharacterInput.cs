using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scrips
{
    using Entities.Scripts;
    using Entities.Commands;
    public class CharacterInput : MonoBehaviour, IMoveInput, IRotationInput, IJumpInput
    {
        [SerializeField]
        private Command _moveInput;

        [SerializeField]
        private PlayerInputActions _inputsActions;

        [SerializeField]
        private Animator _playerAnimator;
        public Vector3 MoveDirection { get; private set; }
        public Vector3 RotationDirection { get; set; }

        public bool JumpButtonPressed { get; set; }

        private void Awake()
        {
            if (_playerAnimator == null)
            {
                Debug.LogWarning("No animator is set in " + this.name + ", getting it through code");
                TryGetComponent<Animator>(out _playerAnimator);
            }
            _inputsActions = new PlayerInputActions();
        }
        private void OnEnable()
        {
            _inputsActions.Enable();
            _inputsActions.Player.Jump.performed += OnJumpInput;
            _inputsActions.Player.Move.performed += OnMoveInput;
            _inputsActions.Player.Attack1.performed += OnAttack1Input;
            _inputsActions.Player.Attack2.performed += OnAttack2Input;
            _inputsActions.Player.Attack3.performed += OnAttack3Input;
            _inputsActions.Player.MouseAim.performed += OnMouseAimInput;
            _inputsActions.Player.AnalogAim.performed += OnAnalogAimInput;
        }
		#region Attacks
		private void OnAttack1Input(InputAction.CallbackContext c)
        {
            _playerAnimator.SetBool("RillaPunch", true);
        }
        private void OnAttack2Input(InputAction.CallbackContext c)
        {
            _playerAnimator.SetBool("RillaSlam", true);
        }
        private void OnAttack3Input(InputAction.CallbackContext c)
        {
            Debug.Log("HAHAHA not implemented");
            //_playerAnimator.SetBool("RillaPunch", true);
        }
		#endregion
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
            float value = c.ReadValue<float>();
            JumpButtonPressed = value == 1 ? true : false;
            _moveInput.Execute();
        }

        private void OnMoveInput(InputAction.CallbackContext c)
        {
            Vector2 value = c.ReadValue<Vector2>();
            MoveDirection = new Vector3(value.x, 0, value.y);
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