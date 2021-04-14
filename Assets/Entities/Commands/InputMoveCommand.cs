using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Commands
{
    using Entities.Scripts;
    public class InputMoveCommand : Command
    {
        [Header("Movement")]
        [SerializeField]
        private AnimationCurve _speed;

        [SerializeField]
        [Tooltip("The time to reset the players move acceleration (basically a deadzone), where the acceleration is based on the acceleratio curve used")]
        private float _accResetTime = 0;

        private float _time = 0;

        private Vector3 _mov;

        private IMoveInput _move;
        private IRotationInput _rotate;

        private Coroutine c_moving;
        private Coroutine c_rotate;
        private Coroutine c_reseter;

        [Header("Jump")]
        [SerializeField]
        private float _jumpHeight = 3;
        [SerializeField]
        private float _gravity = 8;

        private IJumpInput _jump;

        private Coroutine c_jump;
        private bool _isJumping = false;

        [SerializeField]
        private float _jumpCooldownTime = 0;
        private Coroutine c_jumpCooldown;

        [Header("Self generating")]
        [SerializeField]
        private Transform _t;
        [SerializeField]
        private CharacterController _c;
        [SerializeField]
        private float rayCastLenght = 0.2f;

        private void Awake()
        {
            if(_c == null)
                _c = GetComponent<CharacterController>();
            _move = GetComponent<IMoveInput>();
            _rotate = GetComponent<IRotationInput>();
            _jump = GetComponent<IJumpInput>();
            if(_t == null)
                _t = transform;
        }

        public override void Execute()
        {
            if (c_moving == null)
                c_moving = StartCoroutine (Move());
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
            if (!_c.isGrounded)
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
            if (c_reseter != null)
            { 
                StopCoroutine(c_reseter);
                c_reseter = null;
            }
            while (_move.MoveDirection != Vector3.zero || !_c.isGrounded)
            {
                _mov.x = _move.MoveDirection.x;
                _mov.z = _move.MoveDirection.z;
                _time += Time.deltaTime;
                _c.Move(_mov * Time.deltaTime * _speed.Evaluate(_time));

                if (_rotate.RotationDirection == Vector3.zero && _move.MoveDirection != Vector3.zero)
                {
                    transform.forward = _move.MoveDirection;
                }
                yield return null;
            }
            _c.Move(Vector3.zero);
            c_reseter = StartCoroutine(AccelCooldown());
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

        private IEnumerator Jump()
        {
            _mov.y = _jumpHeight;
            _jump.JumpButtonPressed = false;
            yield return null;
            _isJumping = true;
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

        private IEnumerator AccelCooldown()
        {
            yield return new WaitForSeconds(_accResetTime);
            _time = 0;
        }

    }
}