//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/InputSystem/InputSystem.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputSystem : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSystem"",
    ""maps"": [
        {
            ""name"": ""Click"",
            ""id"": ""a270351c-1cf5-40e8-b0a0-22750c0bdcb0"",
            ""actions"": [
                {
                    ""name"": ""ClickBehaviour"",
                    ""type"": ""Button"",
                    ""id"": ""16c68835-6034-44c4-bf9f-c63c1e88171c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a40a6c72-5af0-402b-ac71-5f7a6ef28c80"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ClickBehaviour"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35c969c8-a54f-46fe-96ba-1a24c3f8475e"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ClickBehaviour"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Click
        m_Click = asset.FindActionMap("Click", throwIfNotFound: true);
        m_Click_ClickBehaviour = m_Click.FindAction("ClickBehaviour", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Click
    private readonly InputActionMap m_Click;
    private IClickActions m_ClickActionsCallbackInterface;
    private readonly InputAction m_Click_ClickBehaviour;
    public struct ClickActions
    {
        private @InputSystem m_Wrapper;
        public ClickActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @ClickBehaviour => m_Wrapper.m_Click_ClickBehaviour;
        public InputActionMap Get() { return m_Wrapper.m_Click; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ClickActions set) { return set.Get(); }
        public void SetCallbacks(IClickActions instance)
        {
            if (m_Wrapper.m_ClickActionsCallbackInterface != null)
            {
                @ClickBehaviour.started -= m_Wrapper.m_ClickActionsCallbackInterface.OnClickBehaviour;
                @ClickBehaviour.performed -= m_Wrapper.m_ClickActionsCallbackInterface.OnClickBehaviour;
                @ClickBehaviour.canceled -= m_Wrapper.m_ClickActionsCallbackInterface.OnClickBehaviour;
            }
            m_Wrapper.m_ClickActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ClickBehaviour.started += instance.OnClickBehaviour;
                @ClickBehaviour.performed += instance.OnClickBehaviour;
                @ClickBehaviour.canceled += instance.OnClickBehaviour;
            }
        }
    }
    public ClickActions @Click => new ClickActions(this);
    public interface IClickActions
    {
        void OnClickBehaviour(InputAction.CallbackContext context);
    }
}
