using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main_Menu
{
    public class MainMenu : MonoBehaviour
    {
        private enum ArrowState
        {
            PLAY,
            CREDITS,
            QUIT
        }

        [SerializeField] private RectTransform arrow;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _quitButton;

        private ArrowState _arrowState;

        public Vector3 arrowOffset;


        private void OnEnable()
        {
            SetArrowState(ArrowState.PLAY);
        }

        private void SetArrowState(ArrowState state)
        {
            _arrowState = state;

            Vector3 buttonPosition = _arrowState switch
            {
                ArrowState.PLAY => _playButton.transform.position,
                ArrowState.CREDITS => _optionsButton.transform.position,
                ArrowState.QUIT => _quitButton.transform.position,
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
            UIManager.Instance.UISounds.PlaySound("Menu Selection");
            
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

        public void Accept()
        {
            switch (_arrowState)
            {
                case ArrowState.PLAY:
                    Play();
                    break;
                case ArrowState.CREDITS:
                    Credits();
                    break;
                case ArrowState.QUIT:
                    Quit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Cancel()
        {
            // TODO See if Cancel is needed on Main Menu
        }

        private void Play()
        {
            Debug.Log("Start Game!");
            UIManager.Instance.UISounds.PlaySound("Play");
            GameManager.Instance.StartNewGame();
        }

        private void Credits()
        {
            Debug.Log("Show Credits!");
            GameManager.Instance.Credits();
        }

        private void Quit()
        {
            Debug.Log("Quit Game");
            GameManager.Instance.QuitGame();
        }
    }
}