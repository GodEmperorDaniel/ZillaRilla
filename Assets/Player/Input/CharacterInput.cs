using System.Collections;
using System.Collections.Generic;
using UI.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scrips
{
    using Entities.Scripts;
    using Entities.Commands;

    public class CharacterInput : MonoBehaviour, IMoveInput, IRotationInput, IZillaLazorInput, IReviveInput,
        IHealInput, IPauseInput
    {
        [SerializeField] private Command _moveInput;
        

        //[SerializeField] private PlayerInputActions _inputsActions;
        [Tooltip("Sometimes i lose the reference for this so i just slap it in here i guess")] [SerializeField]
        private Animator _playerAnimator;

        [SerializeField] private bool _useJumpAsRevive = true;
        public Vector3 MoveDirection { get; private set; }
        public Vector3 RotationDirection { get; set; }

        public enum Character
        {
            ZILLA,
            RILLA
        }


        [SerializeField] private Character _character;
        private RillaAttacks _rillaAttacks;
        private ZillaAttacks _zillaAttacks;
        private bool _attack1Pressed;
        private bool _rillaLeftPunch;
        private Attackable _attackable;
        public bool LazorButtonPressed { get; set; }
        public bool ReviveInputIsPressed { get; set; }
        public bool IHealPressed { get; set; }
        public bool IsPressingPause { get; set; }


        private void Awake()
        {
            if (_playerAnimator == null)
            {
                Debug.LogWarning("No animator is set in " + this.name + ", getting it through code");
                TryGetComponent<Animator>(out _playerAnimator);
            }
            _attackable = GetComponent<Attackable>();
            if (_character == Character.ZILLA)
            {
                _zillaAttacks = GetComponent<ZillaAttacks>();
            }
            else
            {
                _rillaAttacks = GetComponent<RillaAttacks>();
            }
        }

        #region Attacks

        public void OnAttack1Input(InputAction.CallbackContext c)
        {
            if (!_attackable._playerSettings._isReviving)
            {
                switch (_character)
                {
                    case Character.ZILLA:
                        if (_zillaAttacks.c_tailCooldown == null && !_playerAnimator.GetBool("ZillaLazorWindup") && c.started)
                            _playerAnimator.SetBool("ZillaTail", true);
                        break;
                    case Character.RILLA:
                        if (_rillaAttacks.c_punchCoolDown == null && !_playerAnimator.GetBool("RillaSlam") && c.started)
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
            else
                _attack1Pressed = false;
        }

        public void OnAttack2Input(InputAction.CallbackContext c)
        {
            if (!_attackable._playerSettings._isReviving)
            {
                bool value = c.ReadValueAsButton();
                switch (_character)
                {
                    case Character.ZILLA:
                        LazorButtonPressed = value;
                        if (_zillaAttacks.c_lazorCooldown == null /*&& !_playerAnimator.GetBool("ZillaTail")*/ &&
                            LazorButtonPressed)
                        {
                            _playerAnimator.SetBool("ZillaLazorWindup", true);
                        }
                        break;
                    case Character.RILLA:
                        if (_rillaAttacks.c_slamCoolDown == null && !_playerAnimator.GetBool("RillaPunch"))
                            _playerAnimator.SetBool("RillaSlam", true);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Aim
        public void OnAnalogAimInput(InputAction.CallbackContext c)
        {
            if (!_attackable._playerSettings._isReviving)
            {
                Vector2 value = c.ReadValue<Vector2>();
                RotationDirection = new Vector3(value.x, 0, value.y);
                if (_moveInput.isActiveAndEnabled)
                    _moveInput.Execute();
            }
        }
        #endregion

        public void OnJumpInput(InputAction.CallbackContext c)
        {
            if (!_attackable._playerSettings._isReviving)
            {
                float value = c.ReadValue<float>();
                ReviveInputIsPressed = value == 1 ? true : false;
            }
            else
            {
                ReviveInputIsPressed = false;
            }
        }

        public void Heal(InputAction.CallbackContext c)
        {
            if (!_attackable._playerSettings._isReviving)
            {
                IHealPressed = c.ReadValueAsButton();
            }
            else
                IHealPressed = false;
        }

        public void Quit()
        {
            Application.Quit();
        }

        #region Movement

        public void OnMoveInput(InputAction.CallbackContext c)
        {
            if (!_attackable._playerSettings._isReviving)
            {
                Vector2 value = c.ReadValue<Vector2>();
                MoveDirection = new Vector3(value.x, 0, value.y);
                if (_moveInput.isActiveAndEnabled)
                    _moveInput.Execute();
            }
            else
                MoveDirection = Vector3.zero;
        }

        public void OnAnalogMove(InputAction.CallbackContext c)
        {
            if (!_attackable._playerSettings._isReviving)
            {
                Vector2 value = c.ReadValue<Vector2>();
                MoveDirection = new Vector3(value.x, 0, value.y);
                if (_moveInput.isActiveAndEnabled)
                    _moveInput.Execute();
            }
            else
                MoveDirection = Vector3.zero;
        }

        #endregion

        public Character GetCharacter()
        {
            return _character;
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            float value = context.ReadValue<float>();
            IsPressingPause = value >= 0.15f;
            if (IsPressingPause) GameManager.Instance.TogglePause();
            
            
        }

        public void OnUINavigate(InputAction.CallbackContext context)
        {
            UIManager.Instance.UIInput.OnNavigate(context);
        }

        public void OnUIAccept(InputAction.CallbackContext context)
        {
            UIManager.Instance.UIInput.OnAcceptPressed(context);
        }

        public void OnUICancel(InputAction.CallbackContext context)
        {
            UIManager.Instance.UIInput.OnCancelPressed(context);
        }
    }
}