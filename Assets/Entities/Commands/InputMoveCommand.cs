using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Entities.Commands
{
    using Entities.Scripts;
    public class InputMoveCommand : Command
    {
		#region Variables
		[Header("Movement")]
        [SerializeField] private float _moveSpeed = 10;
        [SerializeField] private float _lazorMoveSpeed = 5;
        [SerializeField] private float _lazorRotationSpeed;
        [SerializeField] private bool _rillaStillOnSlam = true;
        private Vector3 _mov;

        private IMoveInput _move;
        private IRotationInput _rotate;
        private IZillaLazorInput _lazor;

        private Coroutine c_moving;
        private Coroutine c_rotate;

        [Header("Other")]
        [SerializeField] private float _gravity = 8;

        [Header("Interacting")]
        [SerializeField] private float _objectMoveForce = 2;


         private Transform _transform;
         private CharacterController _characterController;
         private Animator _animator;
         private Player.Scrips.CharacterInput.character _character;
        #endregion

        private void Awake()
        {
            if (_animator == null)
            {
                Debug.Log("Getting Animator");
                _animator = GetComponent<Animator>();
            }
            if(_characterController == null)
                _characterController = GetComponent<CharacterController>();
            _move = GetComponent<IMoveInput>();
            _rotate = GetComponent<IRotationInput>();
            _lazor = GetComponent<IZillaLazorInput>();
            if(_transform == null)
                _transform = transform;
            _character = GetComponent<Player.Scrips.CharacterInput>().GetCharacter();
            _mov.y = -_gravity;
        }

        public override void Execute()
        {
            if (c_moving == null)
                c_moving = StartCoroutine(Move());
            if (c_rotate == null)
                c_rotate = StartCoroutine(Rotate());
        }

        private IEnumerator Move()
        {
            while (_move.MoveDirection != Vector3.zero)
            {
                switch (_character)
                {
                    case Player.Scrips.CharacterInput.character.ZILLA:
                        if (_lazor.LazorButtonPressed)
                        {
                            _mov.x = _move.MoveDirection.x;
                            _mov.z = _move.MoveDirection.z;
                            _characterController.Move(new Vector3(_mov.x * _lazorMoveSpeed, _mov.y, _mov.z * _lazorMoveSpeed) * Time.deltaTime);
                            _animator.SetFloat("Speed", _move.MoveDirection.magnitude / 2);
                        }
                        else
                        {
                            _mov.x = _move.MoveDirection.x;
                            _mov.z = _move.MoveDirection.z;
                            _characterController.Move(new Vector3(_mov.x * _moveSpeed, _mov.y, _mov.z * _moveSpeed) * Time.deltaTime);
                            _animator.SetFloat("Speed", _move.MoveDirection.magnitude);
                        }
                        break;
                    case Player.Scrips.CharacterInput.character.RILLA:
                        if (!(_animator.GetBool("RillaSlam") && _rillaStillOnSlam))
                        {
                            _mov.x = _move.MoveDirection.x;
                            _mov.z = _move.MoveDirection.z;
                            _characterController.Move(new Vector3(_mov.x * _moveSpeed, _mov.y, _mov.z * _moveSpeed) * Time.deltaTime);
                            _animator.SetFloat("Speed", _move.MoveDirection.magnitude);
                        }
                        break;
                    default:
                        break;
                }
                if (_rotate.RotationDirection == Vector3.zero && _move.MoveDirection != Vector3.zero && !_lazor.LazorButtonPressed)
                {
                    transform.forward = _move.MoveDirection;
                }
                yield return null;
            }
            _animator.SetFloat("Speed", _move.MoveDirection.magnitude);
            _characterController.Move(Vector3.zero);
            c_moving = null;
        }

        private IEnumerator Rotate()
        {
            switch (_character)
            {
                case Player.Scrips.CharacterInput.character.ZILLA:
                    if (_rotate.RotationDirection != Vector3.zero && _lazor.LazorButtonPressed)
                    {
                        //Debug.Log(_character.ToString() + " USING FIRST");
                        //Debug.Log(_lazor.LazorButtonPressed);
                        Quaternion target = Quaternion.LookRotation(_rotate.RotationDirection);
                        Quaternion newTargetRotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * _lazorRotationSpeed);
                        transform.rotation = newTargetRotation;

                        yield return null;
                    }
                    else if (_rotate.RotationDirection != Vector3.zero && !_lazor.LazorButtonPressed)
                    {
                        //Debug.Log(_character.ToString() + " USING SECOND");
                        transform.forward = _rotate.RotationDirection;
                        yield return null;
                    }
                    c_rotate = null;
                    break;
                case Player.Scrips.CharacterInput.character.RILLA:
                   if (_rotate.RotationDirection != Vector3.zero)
                    {
                        //Debug.Log(_character.ToString() + " USING FIRST");
                        transform.forward = _rotate.RotationDirection;
                        yield return null;
                    }
                    c_rotate = null;
                    break;
                default:
                    Debug.Log("SOMETHING WRONG IN SCRIPT: " + this + " IN GAMEOBJECT: " + gameObject.name);
                    c_rotate = null;
                    break;
            }
        }
		#region CustomCollision
		private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Movable"))
            {
                Rigidbody rb = hit.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(_move.MoveDirection * _objectMoveForce, ForceMode.Impulse);
            }
        }
        #endregion
        private void OnDisable()
        {
            c_moving = null;
            c_rotate = null;
        }
    }
}