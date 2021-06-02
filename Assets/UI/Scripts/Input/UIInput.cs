using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace UI.Scripts.Input
{
    public class UIInput : MonoBehaviour, INavigate, IAccept, ICancel
    {
        [SerializeField] [Range(0, 1)] private float buttonThreshold = 0.15f;
        public PlayerInputActions PlayerInputActions;

        [HideInInspector] public Command navigateInput;
        [HideInInspector] public Command acceptInput;
        [HideInInspector] public Command cancelInput;

        public bool IsPressingAccept { get; private set; }
        public bool IsPressingCancel { get; private set; }
        public Vector2 NavigateDirection { get; private set; }


        private void Awake()
        {
            PlayerInputActions = new PlayerInputActions();

            navigateInput = gameObject.AddComponent<NavigateCommand>();
            acceptInput = gameObject.AddComponent<AcceptCommand>();
            cancelInput = gameObject.AddComponent<CancelCommand>();
            
        }
        
        public void OnNavigate(InputAction.CallbackContext context)
        {
            // Unity Events will trigger on both started and performed. This makes sure it only triggers on one of them.
            if (!context.started) return;

            Vector2 value = context.ReadValue<Vector2>();
            NavigateDirection = new Vector2(0, value.y);

            if (NavigateDirection == Vector2.zero) return;
            navigateInput.Execute();
        }

        // Default setting and execution of a button press
        private bool OnPressedDefault(InputAction.CallbackContext context, Command command)
        {
            // Unity Events will trigger on both started and performed. This makes sure it only triggers on one of them.
            if (!context.started) return false;

            float value = context.ReadValue<float>();
            bool isPressing = value >= buttonThreshold;
            if (command != null && isPressing) command.Execute();
            return isPressing;
        }

        public void OnAcceptPressed(InputAction.CallbackContext context)
        {
            IsPressingAccept = OnPressedDefault(context, acceptInput);
        }

        public void OnCancelPressed(InputAction.CallbackContext context)
        {
            IsPressingCancel = OnPressedDefault(context, cancelInput);
        }

    }
}