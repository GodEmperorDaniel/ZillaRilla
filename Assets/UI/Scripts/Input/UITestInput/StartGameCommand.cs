using UnityEngine;

namespace UI.Scripts
{
    public class StartGameCommand : Command
    {
        public override void Execute()
        {
            /*if (GameManager.Instance.CurrentGameState != GameManager.GameState.BOOT)
            {
                return;
            }*/
            
            Debug.Log("StartGame called");
            GameManager.Instance.LoadMainMenu();
        }
    }
}