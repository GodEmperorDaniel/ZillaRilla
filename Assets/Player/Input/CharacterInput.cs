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
        [SerializeField] private Animator _playerAnimator;
        public Vector3 MoveDirection { get; private set; }
        public Vector3 RotationDirection { get; set; }
        public enum character
        {
            ZILLA, RILLA
        }

        [SerializeField] private character _character;

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

		#region Attacks
		public void OnAttack1Input(InputAction.CallbackContext c)
        {
            switch (_character)
            {
                case character.ZILLA:
                    if (!_playerAnimator.GetBool("ZillaTail") && !_playerAnimator.GetBool("ZillaLazor"))
                        _playerAnimator.SetBool("ZillaTail", true);
                    break;
                case character.RILLA:
                    if(!_playerAnimator.GetBool("RillaPunch") && !_playerAnimator.GetBool("RillaSlam"))
                        _playerAnimator.SetBool("RillaPunch", true);
                    break;
                default:
                    break;
            }
        }
        public void OnAttack2Input(InputAction.CallbackContext c)
        {
            switch (_character)
            {
                case character.ZILLA:
                    //if (!_playerAnimator.GetBool("ZillaTail") && !_playerAnimator.GetBool("ZillaLazor"))
                        //_playerAnimator.SetBool("ZillaLazor", true);
                    break;
                case character.RILLA:
                    //if (!_playerAnimator.GetBool("RillaPunch") && !_playerAnimator.GetBool("RillaSlam"))
                        //_playerAnimator.SetBool("RillaSlam", true);
                    break;
                default:
                    break;
            }
            Debug.Log("No animations to see here");
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
        public void OnAnalogAimInput(InputAction.CallbackContext c)
        {
            Vector2 value = c.ReadValue<Vector2>();
            RotationDirection = new Vector3(value.x, 0, value.y);
            _moveInput.Execute();
        }

        public void OnJumpInput(InputAction.CallbackContext c)
        {
            float value = c.ReadValue<float>();
            _playerAnimator.SetBool("Jump", true);
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
            MoveDirection = new Vector3(value.x, 0, value.y);
            _moveInput.Execute();
        }
    }
}

