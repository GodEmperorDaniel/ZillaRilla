using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Enemy.Scripts
{
    using Entities.Scripts;
    using Entities.Commands;
    public class EnemyAI : MonoBehaviour, IMoveInput
    {
        [SerializeField]
        private Command _moveInput;

        public Vector3 MoveDirection { get; private set; }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

