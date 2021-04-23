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
        [SerializeField] private Command _moveInput;

        //[SerializeField] private PlayerInputActions _inputsActions;
        [Tooltip("Sometimes i lose the reference for this so i just slap it in here i guess")]
        [SerializeField] private InputAction _input;

        [SerializeField] private Animator _playerAnimator;

        [SerializeField] private InputDevice _inputDevice;
        public Vector3 MoveDirection { get; private set; }
        public Vector3 RotationDirection { get; set; }

        private Vector2 lastMove;

        public bool JumpButtonPressed { get; set; }

        private void Awake()
        {
            if (_playerAnimator == null)
            {
                Debug.LogWarning("No animator is set in " + this.name + ", getting it through code");
                TryGetComponent<Animator>(out _playerAnimator);
            }
            PlayerInput playerInput = GetComponent<PlayerInput>();
            if (playerInput.currentActionMap == null)
            {
                //playerInput.;
            }
        }
        private void OnEnable()
        {
            //_inputsActions.Enable();
            //_inputsActions.Player.Jump.performed += OnJumpInput;
            //_inputsActions.Player.Move.performed += OnMoveInput;
            //_inputsActions.Player.Attack1.performed += OnAttack1Input;
            //_inputsActions.Player.Attack2.performed += OnAttack2Input;
            //_inputsActions.Player.Attack3.performed += OnAttack3Input;
            //_inputsActions.Player.MouseAim.performed += OnMouseAimInput;
            //_inputsActions.Player.AnalogAim.performed += OnAnalogAimInput;
            //_inputsActions.Player.AnalogMove.performed += OnAnalogMove;
        }
        public void SetDeviceInfo(string deviceInfo)
        {
            Debug.Log(deviceInfo);
            //_inputDevice = device;
        }
		#region Attacks
		public void OnAttack1Input(InputAction.CallbackContext c)
        {
            if(!_playerAnimator.GetBool("RillaPunch") && !_playerAnimator.GetBool("RillaSlam"))
                _playerAnimator.SetBool("RillaPunch", true);
        }
        public void OnAttack2Input(InputAction.CallbackContext c)
        {
            if(!_playerAnimator.GetBool("RillaPunch") && !_playerAnimator.GetBool("RillaSlam"))
                _playerAnimator.SetBool("RillaSlam", true);
        }
        public void OnAttack3Input(InputAction.CallbackContext c)
        {
            Debug.Log("HAHAHA not implemented /Jonte");
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

        public void OnJumpInput(InputAction.CallbackContext c)
        {
            float value = c.ReadValue<float>();
            JumpButtonPressed = value == 1 ? true : false;
           _moveInput.Execute();
        }

        public void OnMoveInput(InputAction.CallbackContext c)
        {
            Vector2 value = c.ReadValue<Vector2>();
            MoveDirection = new Vector3(value.x, 0, value.y);
            _moveInput.Execute();
        }

        public void OnAnalogMove(InputAction.CallbackContext c)
        {
                Vector2 value = c.ReadValue<Vector2>();
                Debug.Log(value);
                MoveDirection = new Vector3(value.x, 0, value.y);
                _moveInput.Execute();
        }

        private void OnDisable()
        {
            //_inputsActions.Disable();
            //_inputsActions.Player.Jump.performed -= OnJumpInput;
            //_inputsActions.Player.Move.performed -= OnMoveInput;
        }
    }
}