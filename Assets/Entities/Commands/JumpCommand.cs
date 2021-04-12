using System.Collections.Generic;
using UnityEngine;

namespace Entities.Commands
{
    using Entities.Scripts;
    public class JumpCommand : Command
    {  
        private IJumpInput _jumpInput;

        //[SerializeField]
        //private float jumpHeight = 1;

        private void Awake()
        {
            _jumpInput = GetComponent<IJumpInput>();
        }
    }
}