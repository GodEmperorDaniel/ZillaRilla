using System;
using UnityEngine;

namespace UI.Scripts.Input
{
    public class NavigateCommand : Command
    {
        private INavigate _navigate;

        private void Awake()
        {
            _navigate = GetComponent<INavigate>();
        }

        public override void Execute()
        {
            if (_navigate.NavigateDirection.y == 0.0f) return;
            UIManager.Instance.MenuMovement(_navigate.NavigateDirection.y);


        }
    }
}