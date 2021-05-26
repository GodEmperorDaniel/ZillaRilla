using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        private enum ArrowState
        {
            RESUME,
            EXIT_TO_MENU
        }

        [SerializeField] private RectTransform arrow;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button exitButton;

        public Vector3 arrowOffset;
        
        private ArrowState _arrowState;

        private void OnEnable()
        {
            SetArrowState(ArrowState.RESUME);
        }

        public void TogglePause()
        {
            GameManager.Instance.TogglePause();
        }

        private void Resume()
        {
            TogglePause();
        }

        private void ExitToMenu()
        {
            Debug.Log("Exit To Menu");
            GameManager.Instance.ExitToMainMenu();
        }
        
        public void Accept()
        {
            switch (_arrowState)
            {
                case ArrowState.RESUME:
                    TogglePause();
                    break;
                case ArrowState.EXIT_TO_MENU:
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

        private void SetArrowState(ArrowState state)
        {
            _arrowState = state;

            Vector3 buttonPosition = _arrowState switch
            {
                ArrowState.RESUME => resumeButton.transform.position,
                ArrowState.EXIT_TO_MENU => exitButton.transform.position,
                _ => throw new ArgumentOutOfRangeException()
            };

            Debug.Log("GameObject: " + gameObject.name + ", State: " + _arrowState);
            arrow.transform.position = buttonPosition + arrowOffset;
        }

        private void SetArrowState(int state)
        {
            SetArrowState((ArrowState) state);
        }

        public void MoveArrow(float navigateDirection)
        {
            int enumLength = Enum.GetValues(typeof(ArrowState)).Length;

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