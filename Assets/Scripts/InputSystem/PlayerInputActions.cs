//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/InputSystem/PlayerInputActions.inputactions
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

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""NinjaOnFoot"",
            ""id"": ""2d029eaf-7556-4d50-9d37-e298a9dbb8f9"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""329883c0-b3f5-4032-bd67-55be6a72bb70"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""4ab90599-3625-4e0a-bdc6-2f10809b2b31"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""b3053d16-4afb-44bc-ad5e-93f4749a7f1d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""3c99c38c-4e96-44c1-ad63-7cdfffa5c53d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Passthrough"",
                    ""type"": ""Button"",
                    ""id"": ""9d613550-0021-465d-b16b-dc6827f9f351"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""0cdfe77a-428b-4124-93d4-aea5134cf800"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LookUp"",
                    ""type"": ""Value"",
                    ""id"": ""5d31435a-73ca-4e21-a924-d235f9276a3b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""099fa6c6-fdda-419b-a3b9-814568e6b60d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4a0f642-8c4b-4589-be16-2a0c0b630a4c"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6410ce27-3449-4b2b-a2ba-98765551cd53"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""efb87e80-fb5a-446f-bf66-603da4360892"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f0739f3-8d6a-4a19-ab68-4ff79a46d17e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""67b0f473-8304-4d6e-b900-bb52c175d669"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1eaff3e2-7d11-4b50-a43d-b9df986d9562"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2749c811-c086-4b07-b053-61b168d2945a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e0b1230c-aa48-4cdb-908c-063e9acebab3"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""2a373e36-ddb5-4719-8928-f73b26efe213"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""058745d5-fde4-411f-81a3-8172f49428ca"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a97a723a-ee2b-4ead-bb3d-07ec5508ea5e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c1790bb7-20b1-40e6-a7fa-f634995d6c44"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9963992-6384-48a5-b69a-126bb5cd0c7d"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""348a08fc-6f80-467a-b6b9-177d7c0b38b0"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fdcc009-7280-49b5-a072-6ec2eac05bb1"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0aedcad5-d27d-4af4-92bf-56d4a16df284"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ab07873-f799-4190-9531-be73c7cc298f"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7eabe14e-8bac-4348-8d9e-008afa61d2cf"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d11258f1-3479-476a-bfba-a6c1d2d7ee0f"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e15f3af2-1b71-4d1d-9d7d-58e38ce84f6e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Passthrough"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d20079e1-1b1d-473e-aa24-61c14b06c428"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Passthrough"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f84d1bc0-3970-4696-9093-124a89f1b02c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Passthrough"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ebc7b40b-190c-47d1-b528-b01a97f1a886"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e41e03d7-7ec5-4bc2-bee1-8b5d3ff8137b"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66b3588e-3be7-4381-b9eb-43c79f11490d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04b8bee1-9c59-43ff-b3d0-b354a1238175"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""LookUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""187b5325-d5b7-4e8d-9967-c47a02414288"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""LookUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58d9da55-a47a-4ef8-8cfd-a615d9788d79"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""LookUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e79e7199-b370-4390-a063-2fc7ed8ed60c"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""LookUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e5690a6-6565-4a04-ae5e-43437eb8ee73"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard+Mouse"",
                    ""action"": ""LookUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GUI"",
            ""id"": ""132d0c93-3379-4f94-8f41-62a8db64f12f"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""56366291-348b-4e89-8a22-b1f89ef0ebfe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c50f7839-52d0-48cd-9017-59b1998e4afc"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
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
            ""name"": ""Keyboard+Mouse"",
            ""bindingGroup"": ""Keyboard+Mouse"",
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
        }
    ]
}");
        // NinjaOnFoot
        m_NinjaOnFoot = asset.FindActionMap("NinjaOnFoot", throwIfNotFound: true);
        m_NinjaOnFoot_Jump = m_NinjaOnFoot.FindAction("Jump", throwIfNotFound: true);
        m_NinjaOnFoot_Move = m_NinjaOnFoot.FindAction("Move", throwIfNotFound: true);
        m_NinjaOnFoot_Dash = m_NinjaOnFoot.FindAction("Dash", throwIfNotFound: true);
        m_NinjaOnFoot_Crouch = m_NinjaOnFoot.FindAction("Crouch", throwIfNotFound: true);
        m_NinjaOnFoot_Passthrough = m_NinjaOnFoot.FindAction("Passthrough", throwIfNotFound: true);
        m_NinjaOnFoot_Attack = m_NinjaOnFoot.FindAction("Attack", throwIfNotFound: true);
        m_NinjaOnFoot_LookUp = m_NinjaOnFoot.FindAction("LookUp", throwIfNotFound: true);
        // GUI
        m_GUI = asset.FindActionMap("GUI", throwIfNotFound: true);
        m_GUI_Newaction = m_GUI.FindAction("New action", throwIfNotFound: true);
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

    // NinjaOnFoot
    private readonly InputActionMap m_NinjaOnFoot;
    private INinjaOnFootActions m_NinjaOnFootActionsCallbackInterface;
    private readonly InputAction m_NinjaOnFoot_Jump;
    private readonly InputAction m_NinjaOnFoot_Move;
    private readonly InputAction m_NinjaOnFoot_Dash;
    private readonly InputAction m_NinjaOnFoot_Crouch;
    private readonly InputAction m_NinjaOnFoot_Passthrough;
    private readonly InputAction m_NinjaOnFoot_Attack;
    private readonly InputAction m_NinjaOnFoot_LookUp;
    public struct NinjaOnFootActions
    {
        private @PlayerInputActions m_Wrapper;
        public NinjaOnFootActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_NinjaOnFoot_Jump;
        public InputAction @Move => m_Wrapper.m_NinjaOnFoot_Move;
        public InputAction @Dash => m_Wrapper.m_NinjaOnFoot_Dash;
        public InputAction @Crouch => m_Wrapper.m_NinjaOnFoot_Crouch;
        public InputAction @Passthrough => m_Wrapper.m_NinjaOnFoot_Passthrough;
        public InputAction @Attack => m_Wrapper.m_NinjaOnFoot_Attack;
        public InputAction @LookUp => m_Wrapper.m_NinjaOnFoot_LookUp;
        public InputActionMap Get() { return m_Wrapper.m_NinjaOnFoot; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NinjaOnFootActions set) { return set.Get(); }
        public void SetCallbacks(INinjaOnFootActions instance)
        {
            if (m_Wrapper.m_NinjaOnFootActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnJump;
                @Move.started -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnMove;
                @Dash.started -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnDash;
                @Crouch.started -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnCrouch;
                @Passthrough.started -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnPassthrough;
                @Passthrough.performed -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnPassthrough;
                @Passthrough.canceled -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnPassthrough;
                @Attack.started -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnAttack;
                @LookUp.started -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnLookUp;
                @LookUp.performed -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnLookUp;
                @LookUp.canceled -= m_Wrapper.m_NinjaOnFootActionsCallbackInterface.OnLookUp;
            }
            m_Wrapper.m_NinjaOnFootActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Passthrough.started += instance.OnPassthrough;
                @Passthrough.performed += instance.OnPassthrough;
                @Passthrough.canceled += instance.OnPassthrough;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @LookUp.started += instance.OnLookUp;
                @LookUp.performed += instance.OnLookUp;
                @LookUp.canceled += instance.OnLookUp;
            }
        }
    }
    public NinjaOnFootActions @NinjaOnFoot => new NinjaOnFootActions(this);

    // GUI
    private readonly InputActionMap m_GUI;
    private IGUIActions m_GUIActionsCallbackInterface;
    private readonly InputAction m_GUI_Newaction;
    public struct GUIActions
    {
        private @PlayerInputActions m_Wrapper;
        public GUIActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_GUI_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_GUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GUIActions set) { return set.Get(); }
        public void SetCallbacks(IGUIActions instance)
        {
            if (m_Wrapper.m_GUIActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_GUIActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_GUIActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_GUIActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_GUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public GUIActions @GUI => new GUIActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
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
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard+Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface INinjaOnFootActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnPassthrough(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnLookUp(InputAction.CallbackContext context);
    }
    public interface IGUIActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
