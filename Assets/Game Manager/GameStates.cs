
using UnityEngine;

namespace GameStates
{
    public class BootState : IState
    {
        public void Enter()
        {
            Debug.Log("Entering Boot State");
        }

        public void Execute()
        {
            Debug.Log("Executing Boot State");
        }

        public void Exit()
        {
            Debug.Log("Exiting Boot State");
        }
    }
    
    public class MainMenuState : IState
    {
        public void Enter()
        {
            Debug.Log("Entering Main Menu State");
        }

        public void Execute()
        {
            Debug.Log("Executing Main Menu State");
        }

        public void Exit()
        {
            Debug.Log("Exiting Main Menu State");
        }
    }
}
