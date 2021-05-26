using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] protected RectTransform arrow;
        [SerializeField] protected Button _playButton;
        protected PauseMenu.ArrowState _arrowState;

        public void Accept()
        {
            switch (_arrowState)
            {
                case PauseMenu.ArrowState.PLAY:
                    TogglePause();
                    break;
                case PauseMenu.ArrowState.Exit_TO_MENU:
                    ExitToMenu();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Cancel()
        {
            TogglePause();
        }

        private void SetArrowState(int state)
        {
            SetArrowState((PauseMenu.ArrowState) state);
        }

        public void MoveArrow(float navigateDirection)
        {
            int enumLength = Enum.GetValues(typeof(PauseMenu.ArrowState)).Length;

            // Wraps index between 0 and length of Enum to allow the selection to loop both ways.
            if (navigateDirection > 0.0f)
            {
                int newState = ((int) _arrowState - 1 + enumLength) % enumLength;
                SetArrowState(newState);
            }
            else
            {
                int newState = ((int) _arrowState + 1) % enumLength;
                SetArrowState(newState);
            }
        }
    }
}