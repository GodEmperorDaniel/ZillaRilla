// GENERATED AUTOMATICALLY FROM 'Assets/UI/Scripts/Input/UITestInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @UITestInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @UITestInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UITestInput"",
    ""maps"": [
        {
            ""name"": ""UITest"",
            ""id"": ""74689d3b-e886-457f-acc8-3f7ad5cc9f5c"",
            ""actions"": [
                {
                    ""name"": ""Start Game"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a2b9f18d-b219-4849-b92a-4c97c467ffd8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Log State"",
                    ""type"": ""Button"",
                    ""id"": ""a8d8b69a-b33e-43c8-b797-6f0178b65b03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause Game"",
                    ""type"": ""Button"",
                    ""id"": ""1591c054-5796-4ee7-b298-9712a7a828b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Nudge Debug Box"",
                    ""type"": ""Button"",
                    ""id"": ""8b28678b-5a31-494c-8116-a9c4ff0b043b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3a30fb9b-e642-45cd-b7fa-911886e2c219"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Gamepad;Touch;Joystick;XR"",
                    ""action"": ""Start Game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ebe6e04a-ba51-4f6b-aa2d-20b93207446c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Log State"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3be995bb-9d97-46e2-963f-62649b81e72f"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause Game"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01bb5e5d-a4ac-4b5b-aa99-89f51f41cddf"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Nudge Debug Box"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // UITest
        m_UITest = asset.FindActionMap("UITest", throwIfNotFound: true);
        m_UITest_StartGame = m_UITest.FindAction("Start Game", throwIfNotFound: true);
        m_UITest_LogState = m_UITest.FindAction("Log State", throwIfNotFound: true);
        m_UITest_PauseGame = m_UITest.FindAction("Pause Game", throwIfNotFound: true);
        m_UITest_NudgeDebugBox = m_UITest.FindAction("Nudge Debug Box", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // UITest
    private readonly InputActionMap m_UITest;
    private IUITestActions m_UITestActionsCallbackInterface;
    private readonly InputAction m_UITest_StartGame;
    private readonly InputAction m_UITest_LogState;
    private readonly InputAction m_UITest_PauseGame;
    private readonly InputAction m_UITest_NudgeDebugBox;
    public struct UITestActions
    {
        private @UITestInput m_Wrapper;
        public UITestActions(@UITestInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @StartGame => m_Wrapper.m_UITest_StartGame;
        public InputAction @LogState => m_Wrapper.m_UITest_LogState;
        public InputAction @PauseGame => m_Wrapper.m_UITest_PauseGame;
        public InputAction @NudgeDebugBox => m_Wrapper.m_UITest_NudgeDebugBox;
        public InputActionMap Get() { return m_Wrapper.m_UITest; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UITestActions set) { return set.Get(); }
        public void SetCallbacks(IUITestActions instance)
        {
            if (m_Wrapper.m_UITestActionsCallbackInterface != null)
            {
                @StartGame.started -= m_Wrapper.m_UITestActionsCallbackInterface.OnStartGame;
                @StartGame.performed -= m_Wrapper.m_UITestActionsCallbackInterface.OnStartGame;
                @StartGame.canceled -= m_Wrapper.m_UITestActionsCallbackInterface.OnStartGame;
                @LogState.started -= m_Wrapper.m_UITestActionsCallbackInterface.OnLogState;
                @LogState.performed -= m_Wrapper.m_UITestActionsCallbackInterface.OnLogState;
                @LogState.canceled -= m_Wrapper.m_UITestActionsCallbackInterface.OnLogState;
                @PauseGame.started -= m_Wrapper.m_UITestActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_UITestActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_UITestActionsCallbackInterface.OnPauseGame;
                @NudgeDebugBox.started -= m_Wrapper.m_UITestActionsCallbackInterface.OnNudgeDebugBox;
                @NudgeDebugBox.performed -= m_Wrapper.m_UITestActionsCallbackInterface.OnNudgeDebugBox;
                @NudgeDebugBox.canceled -= m_Wrapper.m_UITestActionsCallbackInterface.OnNudgeDebugBox;
            }
            m_Wrapper.m_UITestActionsCallbackInterface = instance;
            if (instance != null)
            {
                @StartGame.started += instance.OnStartGame;
                @StartGame.performed += instance.OnStartGame;
                @StartGame.canceled += instance.OnStartGame;
                @LogState.started += instance.OnLogState;
                @LogState.performed += instance.OnLogState;
                @LogState.canceled += instance.OnLogState;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
                @NudgeDebugBox.started += instance.OnNudgeDebugBox;
                @NudgeDebugBox.performed += instance.OnNudgeDebugBox;
                @NudgeDebugBox.canceled += instance.OnNudgeDebugBox;
            }
        }
    }
    public UITestActions @UITest => new UITestActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IUITestActions
    {
        void OnStartGame(InputAction.CallbackContext context);
        void OnLogState(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnNudgeDebugBox(InputAction.CallbackContext context);
    }
}
