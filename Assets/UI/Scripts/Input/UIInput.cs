using UnityEngine;
using UnityEngine.InputSystem;
namespace UI.Scripts.Input
{
    public class UIInput : MonoBehaviour, INavigate, IAccept, ICancel
    {
        [SerializeField] [Range(0, 1)] private float buttonThreshold = 0.15f;
        private PlayerInputActions _playerInputActions;

        [HideInInspector] public Command navigateInput;
        [HideInInspector] public Command acceptInput;
        [HideInInspector] public Command cancelInput;

        public bool IsPressingAccept { get; private set; }
        public bool IsPressingCancel { get; private set; }
        public Vector2 NavigateDirection { get; private set; }


        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();

            navigateInput = gameObject.AddComponent<NavigateCommand>();
            acceptInput = gameObject.AddComponent<AcceptCommand>();
            cancelInput = gameObject.AddComponent<CancelCommand>();
        }

        private void OnEnable()
        {
            _playerInputActions.Enable();

            _playerInputActions.UI.Navigate.started += OnNavigate;
            _playerInputActions.UI.Accept.performed += OnAcceptPressed;
            _playerInputActions.UI.Cancel.performed += OnCancelPressed;
        }

        private void OnDisable()
        {
            _playerInputActions.UI.Navigate.started -= OnNavigate;
            _playerInputActions.UI.Accept.performed -= OnAcceptPressed;
            _playerInputActions.UI.Cancel.performed -= OnCancelPressed;

            _playerInputActions.Disable();
        }

    
        private void OnNavigate(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            NavigateDirection = new Vector2(0, value.y);
            if (navigateInput == null && NavigateDirection == Vector2.zero) return;
            navigateInput.Execute();
        }

        // Default setting and execution of a button press
        private bool OnPressedDefault(InputAction.CallbackContext context, Command command)
        {
            float value = context.ReadValue<float>();
            bool isPressing = value >= buttonThreshold;
            if (command != null && isPressing) command.Execute();
            return isPressing;
        }

        private void OnAcceptPressed(InputAction.CallbackContext context)
        {
            Debug.Log("Accept Pressed!");
            IsPressingAccept = OnPressedDefault(context, acceptInput);
        }

        private void OnCancelPressed(InputAction.CallbackContext context)
        {
            Debug.Log("Cancel Pressed!");
            IsPressingCancel = OnPressedDefault(context, cancelInput);
        }
    
        private void OnClickPressed(InputAction.CallbackContext context)
        {
            Debug.Log("Clicked!");
            //IsPressingClick = OnPressedDefault(context, cancelInput);
        }
    }
}