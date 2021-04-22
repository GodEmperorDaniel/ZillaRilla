using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Commands
{
    using Entities.Scripts;
    public class InputMoveCommand : Command
    {
		#region Variables
		[Header("Movement")]
        [SerializeField] private float _speedy = 10;

        private Vector3 _mov;

        private IMoveInput _move;
        private IRotationInput _rotate;

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

        [Header("Self generating")]
        [SerializeField] private Transform _transform;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float rayCastLenght = 0.2f;
		#endregion
        
		private void Awake()
        {
            if(_characterController == null)
                _characterController = GetComponent<CharacterController>();
            _move = GetComponent<IMoveInput>();
            _rotate = GetComponent<IRotationInput>();
            _jump = GetComponent<IJumpInput>();
            if(_transform == null)
                _transform = transform;
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
                _characterController.Move(new Vector3(_mov.x * _speedy, _mov.y, _mov.z * _speedy) * Time.deltaTime);

                if (_rotate.RotationDirection == Vector3.zero && _move.MoveDirection != Vector3.zero)
                {
                    transform.forward = _move.MoveDirection;
                }
                yield return null;
            }
            _characterController.Move(Vector3.zero);
            c_moving = null;
        }

        private IEnumerator Rotate()
        {
            if (_rotate.RotationDirection != Vector3.zero)
            {
                transform.forward = _rotate.RotationDirection;
                yield return null;
            }
            c_rotate = null;
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
            bool raycast = Physics.Raycast(transform.position,Vector3.down, rayCastLenght); 
            while (!raycast) 
            {
                raycast = Physics.Raycast(transform.position, Vector3.down, rayCastLenght);
                yield return null; 
            }
            yield return new WaitForSeconds(_jumpCooldownTime);
            c_jump = null;
            c_jumpCooldown = null;
            _isJumping = false;
        }
    }
}