using UnityEngine;

namespace UI.Scripts
{
    public class StartGameCommand : Command
    {
        public override void Execute()
        {
            if (GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
            {
                return;
            }
            
            Debug.Log("StartGame called");
            GameManager.Instance.StartGame();
        }
    }
}