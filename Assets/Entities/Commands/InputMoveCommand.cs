using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Commands
{
    using Entities.Scripts;
    public class InputMoveCommand : Command
    {
        [SerializeField]
        private AnimationCurve _speed;
        [SerializeField]
        private Transform _t;
        [SerializeField]
        private CharacterController _c;
        
        private IMoveInput _move;
        private IRotationInput _rotate;

        private Coroutine c_moving;
        private Coroutine c_rotate;
        private Coroutine c_reseter;
       //private Vector3 _dir;

        [SerializeField]
        [Tooltip("The time to reset the players move acceleration (basically a deadzone), where the acceleration is based on the acceleratio curve used")]
        private float _accResetTime = 0;

        private float _time = 0;

        private void Awake()
        {
            if(_c == null)
                _c = GetComponent<CharacterController>();
            _move = GetComponent<IMoveInput>();
            _rotate = GetComponent<IRotationInput>();
            if(_t == null)
                _t = transform;
        }

        public override void Execute()
        {
            if (c_moving == null)
                c_moving = StartCoroutine (Move());
            if (c_rotate == null)
                c_rotate = StartCoroutine(Rotate());
        }

        private IEnumerator Move()
        {
            if (c_reseter != null)
            { 
                StopCoroutine(c_reseter);
                c_reseter = null;
            }
            while(_move.MoveDirection != Vector3.zero)
            {
                _time += Time.deltaTime;
                _c.Move(_move.MoveDirection * Time.deltaTime * _speed.Evaluate(_time));

                if (_rotate.RotationDirection == Vector3.zero)
                {
                    transform.forward = _move.MoveDirection;
                } 
                yield return null;
            }
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

        private IEnumerator AccelCooldown()
        {
            yield return new WaitForSeconds(_accResetTime);
            _time = 0;
        }

    }

}