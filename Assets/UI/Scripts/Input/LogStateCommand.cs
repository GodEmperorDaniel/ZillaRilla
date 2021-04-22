using UnityEngine;

namespace UI.Scripts
{
    public class LogStateCommand : Command
    {
        public override void Execute()
        {
            //Debug.Log("Current Game State: " + GameManager.Instance.CurrentGameState);
        }
    }
}