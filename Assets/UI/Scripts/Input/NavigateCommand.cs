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
            UIManager.Instance.MenuSelection(_navigate.NavigateDirection.y);
        }
    }
}