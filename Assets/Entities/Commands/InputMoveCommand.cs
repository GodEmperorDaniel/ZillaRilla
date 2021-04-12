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

        private Coroutine _moving;
        private Vector3 _dir;

        private void Awake()
        {
            //_c = GetComponent<CharacterController>();
            _move = GetComponent<IMoveInput>();
            //_t = transform;
        }

        public override void Execute()
        {
            if (_moving == null)
            {
                _moving = StartCoroutine (Move());
            }
        }

        private IEnumerator Move()
        {

            while(_move.MoveDirection != Vector2.zero)
            {
                _dir = new Vector3(_move.MoveDirection.x, 0, _move.MoveDirection.y);
                _c.Move(_dir * Time.deltaTime * 5);
                transform.forward = new Vector3(_move.MoveDirection.x, 0, _move.MoveDirection.y).normalized;
                yield return null;
            }

            _moving = null;

        }

    }

}