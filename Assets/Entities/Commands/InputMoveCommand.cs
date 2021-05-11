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
        [SerializeField] private float _lazorRotationSpeed;
        private Vector3 _mov;

        private IMoveInput _move;
        private IRotationInput _rotate;
        private IZillaLazorInput _lazor;

        private Coroutine c_moving;
        private Coroutine c_rotate;

        [Header("Jump")]
        [SerializeField] private float _jumpStength = 3;
        [SerializeField] private float _jumpTime = 0.25F;
        [SerializeField] private float _gravity = 8;

        private IJumpInput _jump;

        private Coroutine c_jump;
        private bool _isJumping = false;

        [SerializeField] private float _jumpCooldownTime = 0;
        private Coroutine c_jumpCooldown;

        [Header("Interacting")]
        [SerializeField] private float _objectKnockbackForce;

        [Header("Self generating")]
        [SerializeField] private Transform _transform;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float rayCastLenght = 0.2f;
        [SerializeField] private Animator _animator;
        [SerializeField] private Player.Scrips.CharacterInput.character _character;
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
            _jump = GetComponent<IJumpInput>();
            _lazor = GetComponent<IZillaLazorInput>();
            if(_transform == null)
                _transform = transform;
            _character = GetComponent<Player.Scrips.CharacterInput>().GetCharacter();
        }

        public override void Execute()
        {
            if (c_moving == null)
                c_moving = StartCoroutine(Move());
            if (c_rotate == null)
                c_rotate = StartCoroutine(Rotate());
            if (c_jump == null && _jump.JumpButtonPressed)
                c_jump = StartCoroutine(Jump());
            if (c_jumpCooldown == null && _isJumping == true)
                c_jumpCooldown = StartCoroutine(JumpCooldown());
        }
        private void Update()
        {
            Gravity();
            JumpWhileStill();
            Debug.DrawLine(transform.position + new Vector3(0, 1, 0), (transform.position + rayCastLenght * Vector3.down), Color.red);
        }

        private void Gravity()
        {
            if (!_characterController.isGrounded)
            {
                _mov.y -= _gravity * Time.deltaTime;
            }
        }

        private void JumpWhileStill()
        {
            if (c_moving == null && _mov.y != 0)
            {
                c_moving = StartCoroutine(Move());
            }
        }

        private IEnumerator Move()
        {
            while (_move.MoveDirection != Vector3.zero || !_characterController.isGrounded)
            {
                _mov.x = _move.MoveDirection.x;
                _mov.z = _move.MoveDirection.z;
                _characterController.Move(new Vector3(_mov.x * _moveSpeed, _mov.y, _mov.z * _moveSpeed) * Time.deltaTime);

                if (_rotate.RotationDirection == Vector3.zero && _move.MoveDirection != Vector3.zero)
                {
                    _animator.SetFloat("Speed", _move.MoveDirection.magnitude);
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

        private IEnumerator Jump() //make this move more smooth with lerp
        {
            float count = 0;
            while (count < _jumpTime && _jump.JumpButtonPressed)
            {
                _mov.y = _jumpStength;
                count = count + Time.deltaTime;
                yield return null;
            }
            _isJumping = true;
            yield return null;
        }

        private IEnumerator JumpCooldown()
        {
            bool raycast = Physics.Raycast(transform.position + new Vector3(0,1,0),Vector3.down, rayCastLenght); 
            while (!raycast) 
            {
                raycast = Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, rayCastLenght);
                yield return new WaitForSeconds(0.3f); 
            }
            _animator.SetBool("Jump", false);
            yield return new WaitForSeconds(_jumpCooldownTime);
            _jump.JumpButtonPressed = false;
            _isJumping = false;
            c_jump = null;
            c_jumpCooldown = null;
        }
		#region CustomCollision
		private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Movable"))
            {
                Rigidbody rb = hit.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(_move.MoveDirection, ForceMode.Impulse);
            }
        }
        #endregion
    }
}