using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scrips
{
    using Entities.Scripts;
    using Entities.Commands;
    public class CharacterInput : MonoBehaviour, IMoveInput, IRotationInput, IJumpInput, IZillaLazorInput, IReviveInput
    {
        [SerializeField] private Command _moveInput;

        //[SerializeField] private PlayerInputActions _inputsActions;
        [Tooltip("Sometimes i lose the reference for this so i just slap it in here i guess")]
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private bool _useJumpInput;
        [SerializeField] private bool _useJumpAsRevive = true;
        public Vector3 MoveDirection { get; private set; }
        public Vector3 RotationDirection { get; set; }
        public enum character
        {
            ZILLA, RILLA
        }

        [SerializeField] private character _character;
        private bool _attack1Pressed;
        private bool _rillaLeftPunch;

        public bool JumpButtonPressed { get; set; }
        public bool LazorButtonPressed { get; set; }
        public bool ReviveInputIsPressed{ get; set; }

        private void Awake()
        {
            if (_playerAnimator == null)
            {
                Debug.LogWarning("No animator is set in " + this.name + ", getting it through code");
                TryGetComponent<Animator>(out _playerAnimator);
            }
            PlayerInput playerInput = GetComponent<PlayerInput>();
        }

        #region Attacks
        public void OnAttack1Input(InputAction.CallbackContext c)
        {
            _attack1Pressed = c.ReadValueAsButton();
            //Debug.Log(_attack1Pressed);
            switch (_character)
            {
                case character.ZILLA:
                    if (!_playerAnimator.GetBool("ZillaTail") && !_playerAnimator.GetBool("ZillaLazorWindup") && _attack1Pressed)
                        _playerAnimator.SetBool("ZillaTail", true);
                    break;
                case character.RILLA:
                    if (!_playerAnimator.GetBool("RillaPunch") && !_playerAnimator.GetBool("RillaSlam") && _attack1Pressed)
                    {
                        _rillaLeftPunch = !_rillaLeftPunch;
                        _playerAnimator.SetBool("RillaPunch", true);
                        if (_rillaLeftPunch)
                            _playerAnimator.SetBool("Rilla_Left_Punch", true);
                        else
                            _playerAnimator.SetBool("Rilla_Right_Punch", true);
                    }
                    break;
                default:
                    break;
            }
        }
        public void OnAttack2Input(InputAction.CallbackContext c)
        {
            bool value = c.ReadValueAsButton();
            switch (_character)
            {
                case character.ZILLA:
                    LazorButtonPressed = value;
                    if (!_playerAnimator.GetBool("ZillaTail") && !_playerAnimator.GetBool("ZillaLazorWindup") && LazorButtonPressed)
                    { 
                        _playerAnimator.SetBool("ZillaLazorWindup", true);
                        //_playerAnimator.SetBool("ZillaLazor", true);
                    }
                    break;
                case character.RILLA:
                    if (!_playerAnimator.GetBool("RillaPunch") && !_playerAnimator.GetBool("RillaSlam"))
                        _playerAnimator.SetBool("RillaSlam", true);
                    break;
                default:
                    break;
            }
        }
        public void OnAttack3Input(InputAction.CallbackContext c)
        {
            Debug.Log("HAHAHA not implemented /Jonte");
            //_playerAnimator.SetBool("RillaPunch", true);
        }
		#endregion
		#region Aim
		private void OnMouseAimInput(InputAction.CallbackContext c)
        {
            //probably a little bit of match and coordinate conversions... will fix later
            //Vector2 value = c.ReadValue<Vector2>();

        }
        public void OnAnalogAimInput(InputAction.CallbackContext c)
        {
            Vector2 value = c.ReadValue<Vector2>();
            RotationDirection = new Vector3(value.x, 0, value.y);
            if(_moveInput.isActiveAndEnabled)
                _moveInput.Execute();
        }
        #endregion
        public void OnJumpInput(InputAction.CallbackContext c)
        {
            float value = c.ReadValue<float>();
            if (_useJumpInput)
            {
                _playerAnimator.SetBool("Jump", true); //dessa tv? rader kan nog skapa problem
                JumpButtonPressed = value == 1 ? true : false;
                //_moveInput.Execute();
            }
            if (_useJumpAsRevive)
            { 
                ReviveInputIsPressed = value == 1 ? true : false;
            }
        }
        public void Quit()
        {
            Application.Quit();
        }
        #region Movement
        public void OnMoveInput(InputAction.CallbackContext c)
        {
            Vector2 value = c.ReadValue<Vector2>();
            MoveDirection = new Vector3(value.x, 0, value.y);
            if(_moveInput.isActiveAndEnabled)
                _moveInput.Execute();
        }

        public void OnAnalogMove(InputAction.CallbackContext c)
        {
            
            Vector2 value = c.ReadValue<Vector2>();
            MoveDirection = new Vector3(value.x, 0, value.y);
            if(_moveInput.isActiveAndEnabled)
                _moveInput.Execute();
        }
        #endregion
        public character GetCharacter()
        {
            return _character;
        }  
	}
}

