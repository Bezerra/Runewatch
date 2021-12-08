// GENERATED AUTOMATICALLY FROM 'Assets/Input System + Input Scripts/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""b16d9d04-72a8-41d2-b1a1-59d567f9ebbf"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b2723a99-9619-4e96-9257-495d9b1fa82e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""b2476ec2-e205-4e6c-acac-891962f611c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9aff7465-bbf6-462b-9051-1a59ceda9305"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""c260fb6a-d302-410c-b6ce-0de4fdafb3cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Secondary Fire"",
                    ""type"": ""Button"",
                    ""id"": ""088ad916-c193-4bce-b8e2-52e5e13fa1d7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Spell 1"",
                    ""type"": ""Button"",
                    ""id"": ""4a2539f9-315d-4a8b-889a-a0a1b6b3ae2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Spell 2"",
                    ""type"": ""Button"",
                    ""id"": ""c15dbde8-43b3-407b-bdd8-cf74ad7b3b76"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Spell 3"",
                    ""type"": ""Button"",
                    ""id"": ""face129a-550c-4d75-bc24-3f11829a0d83"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Spell 4"",
                    ""type"": ""Button"",
                    ""id"": ""e8310be2-ecba-4eff-9087-8877bdfe3408"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""299561e6-e343-4100-accb-75de2a0c61de"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""b975ce31-d459-45ff-ad6e-ee541fe973c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuickSave"",
                    ""type"": ""Button"",
                    ""id"": ""0f25e72a-a24c-4efb-82f7-6d450cdf9928"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuickLoad"",
                    ""type"": ""Button"",
                    ""id"": ""b8ce6067-546c-4b83-8810-8a1e4fab440c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""c764ebdd-c789-4440-a40d-00fc72cb4c5b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cheat Console"",
                    ""type"": ""Button"",
                    ""id"": ""a9a81bc7-7db8-40a3-bf95-74d3c0778866"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""e10cad96-58af-4949-8508-f2e9f3b6bb5e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PreviousAndNextSpellMouseScroll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e3751412-a8cf-452f-80b0-3eb3d40b4f98"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Next Spell"",
                    ""type"": ""Button"",
                    ""id"": ""e5f54a8c-aae2-4492-87c0-9fbe2e271edd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Previous Spell"",
                    ""type"": ""Button"",
                    ""id"": ""92bcf55d-39f7-49ed-be72-3ab930bcb58f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""8406574f-19f4-42bb-aeea-1d8c53f3ba7a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a99e3582-77ef-4570-a421-3633e6482aba"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3a9315ea-b05f-4220-9f48-9545fac9fc7c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e8927432-ca88-487c-a6df-3b44ec11bc72"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ceb85a7f-3c1b-441a-ba1b-290e3c947dba"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""245e82a6-150a-4631-95d1-933295c2d915"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82f16dd7-9c8d-4104-859c-e2dacbdb9a66"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d75a0f2-d879-49eb-b87e-c08c3132f51b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Spell 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27e28a1a-46c8-4a2d-9ace-35ca196b840c"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Spell 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85c48f14-4e0c-4ab2-b1e7-d438d581aa26"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Spell 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c9224e0-582f-4f4e-b37c-307df8a2fe96"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Spell 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c72b945c-628c-47d0-b1a1-2b23a455df61"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6efdffc-33b5-421f-b774-d17c641982ba"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": """",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""576f0d4f-a6c9-46d6-896b-72e07addcb6e"",
                    ""path"": ""<Keyboard>/f5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""QuickSave"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""188b7645-adf0-4dd8-8864-6bf160be4b32"",
                    ""path"": ""<Keyboard>/f8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""QuickLoad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55dfad8c-872a-414d-afec-d7e293bfa1d1"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""06a29a43-e56c-40a9-befe-ae454b18492f"",
                    ""path"": ""<Keyboard>/backslash"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Cheat Console"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""000b562b-b0dd-492d-9fbc-092f15b925f4"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Cheat Console"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8ea6f7d-2b8a-4e14-bb8a-3134a01f0505"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f23a4fe-532a-44c3-8194-a249b01b665d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Secondary Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e53e149f-386b-4e13-bffb-45ceb490a43f"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a9ec9ef-11df-4d4e-a0d1-ecff32bf53ec"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""PreviousAndNextSpellMouseScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24d60890-a98d-4c08-9486-68a57b94de3a"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Previous Spell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4cc961f-ea65-4f3d-b9d2-a14eb81fd1ad"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Next Spell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad1e7525-c51e-40bf-ba9a-f1ca0f9c603e"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Interface"",
            ""id"": ""c8bb89d4-0d85-4256-9938-c989b35d6205"",
            ""actions"": [
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""143b3ccf-43af-4290-b4ab-0dd34466cafb"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bd0ee727-d46c-47b9-8b37-300fc130e395"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""cce9b583-51c3-479c-913b-758450a9deeb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""Button"",
                    ""id"": ""d0b9f83f-e2dc-411b-a8d5-aaa430a020d7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e02770ab-060c-45c5-aef9-1be4701e63b8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""48313662-771c-4790-bdfc-5ac14903acf7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6168aa7e-b20c-4c72-bca5-e6ecabb799a4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""6b3719a0-7e0f-4ca8-8613-8a98271bd3d2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""7b4a554a-6878-4104-8494-43b2eb9609c4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""a2de4d01-29de-4168-aa8f-16a59e1d662c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""db60bb29-93d4-4f4b-8047-1a3c3d337483"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""4a36843f-7b5c-4c6a-93fc-1575a0e4f1a1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e0d165d2-923a-4dac-9bd7-9002369a7d4c"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""baf19312-5718-4a7c-972b-710503564c46"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4f82b314-99b0-496e-9e7f-86a3f4ff5410"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c51ba9e8-d707-44ef-948f-e2a62e79f62e"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c799f6ff-39a8-4e42-b405-43b440d03280"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c1d58df7-ccc7-4681-af34-86bc4a6b0634"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""df1d075a-9dfd-4bbf-8370-50c3a68d3fda"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e704d807-37a7-4ea5-a709-5724552c23a1"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""729b01d5-e1cf-45fe-b4b0-82814cbed3ba"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""0e6628fe-f747-4500-b71a-f580bd984437"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""77451fa2-f951-4f6d-9122-1aa29d5dd607"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ca69d34c-19a9-464c-a146-71356a231c57"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""776c0da3-2816-4085-af03-7ef634b0c06a"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0cf567aa-3203-4afe-b174-c8ec789dd882"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""9c39de34-9fd4-4090-843f-d7baa69f3dcf"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""11249311-5a38-4f6a-ba09-9f11a631a44c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d727816b-7ee8-4dc3-b8f5-2da0273feb18"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6b8c21f0-f1db-46fc-9bb5-18b237952b43"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9418da01-a40b-4c25-aeab-fa0df388b142"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""41092858-2194-4654-8a5a-2d6844a300be"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b1fb98c8-3c9a-4dbf-985e-6970f1ec97e5"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a307c5c1-f04e-4b11-af08-c079db83907c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2d89ffe6-c322-4787-b128-737e9761bc30"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5ac6e3cb-efcd-4807-b42c-c410722a4ac6"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""587c746b-a2bf-4a5f-9529-c79632135428"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36da64a7-0c71-4ce4-bbfb-0bf8780e1f6c"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97e01747-7598-4bfd-9b80-4d058460c808"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5149a08-5029-44c7-95f3-18efb90cbcf6"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1c903670-e267-4716-9fd1-9111505e62f8"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69c40149-ad4b-461f-94b8-98fb3080e20f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17a85880-e53f-4772-8797-cb9ce082d28f"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b59e6bd5-35b9-4e22-b7d2-330ad99f77ed"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40bc6809-fb18-4952-b447-a2ad3b58e566"",
                    ""path"": ""<XRController>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9df29872-22ab-4a3b-9bed-56505a37191e"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""120b8155-91f1-4d55-a821-3dec27d644bc"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b984f873-aa31-4a79-a314-7749e60f1c7e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fedb96e0-95e3-44ac-8ea5-243e8c52b724"",
                    ""path"": ""<XRController>/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDevicePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3bdd06ab-5076-4152-913a-5ef093afcbbb"",
                    ""path"": ""<XRController>/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDeviceOrientation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab48d243-e729-41b3-9d15-d6a2659b1d48"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""AbilityChoice"",
            ""id"": ""7887e4ed-ad47-42f3-bb45-b41fd9aba8b5"",
            ""actions"": [
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""fa304cfa-c29c-4fff-9a48-d4d248fd1de7"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""53ff61bb-8222-4185-af15-4c69b6bec278"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""90ee91df-f179-4af4-a7f2-8a8e2f3d3acd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""Button"",
                    ""id"": ""ee816537-bc3d-4015-9e38-ae02734538fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""68707562-d4aa-43e4-9518-ec0ad62b0aec"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""39007fec-89a4-4075-b4bb-ca05779d6db6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4ccf6415-1b9e-4369-a922-89899403ad1c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""f4a0ae75-5eb7-4e71-89e3-906474a3db34"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""909fd010-e2d8-4e4b-b6c0-4f29e4bccb67"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""81d98ff8-48ca-409a-8f53-a316013edb64"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""0fb1ca82-245a-4631-808d-14806730e38f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ef05a173-dd2c-4223-a51d-c9c6dadaffad"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3d374d77-397e-45fb-9030-2217f9f20700"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fb730870-46f0-4c05-ba3c-06ac2a5931fc"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""096a563b-6f89-451a-8b91-6f447b5b4768"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9346ee63-a824-4ac9-ac5b-438b65940054"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5f111496-3370-407c-895b-439cc43333a3"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1e8b9eda-5e4e-4ba0-aa7f-b1581b64add8"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""701b01a9-af89-4ce1-8ead-9e52faa70356"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6921e38d-c911-4221-8ad7-77abbf2011e5"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""0c0f2e5e-c808-4d15-b26c-7d7338d775ba"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d2aec3ac-ebae-4e32-bddf-94e64de7fe67"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b8be0b70-4981-4803-867a-bf07b5d46d32"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fd75f362-0748-4d3f-aa20-34ae790385ed"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9a872a9c-1630-43ee-8c18-7da847f84d08"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""b08e6f99-96a3-4225-a582-20eee326e173"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c0328d93-5ec2-448b-b3db-44500d253060"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1e7329c6-9200-462e-99f5-a42fb90e2f07"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""461b0952-ddfe-4aaa-8dce-46784e08ea8f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6d0697f7-ca26-46ce-811d-4cef9481196c"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b2e39875-3587-48ee-b70a-f47ca76c216c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4c659bd5-78a0-4f62-948b-ccb4f26e2c32"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""06fd57f7-d5f2-456c-a9e3-cb8545a8d8e7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""36893170-4127-4919-87d5-07750f173798"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b8b529be-17a5-405a-871b-6ead0043eac2"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf40761a-3493-45d7-b60a-7418f523c912"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""475997f2-147a-4c47-82e0-0f9b725a14ca"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f40939c5-0036-4f8c-a392-d91d1c0d3df9"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73b42ecd-0f7b-4843-ac5d-b7634c279932"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53d97da5-fcac-42a1-ade3-d9580568d2b3"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7c47d35-03c4-44dd-b472-d746860cf116"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ed4e91a-742a-46da-abd6-3a435887d793"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12e03530-8dfc-45a4-a8bd-b0b8d5d2cf26"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b5de4c84-6777-4dd6-87df-7f6e0157bbc5"",
                    ""path"": ""<XRController>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""498db775-9569-4613-bdfa-a087a20d9b08"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32934de6-1295-4702-9aa6-d7006c6fc36a"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""84b7b2c9-b9ad-4d63-bf62-8e715383959c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Computer"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""148b6ab5-8a0f-447e-bc57-0de332ce6863"",
                    ""path"": ""<XRController>/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDevicePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa1a4626-0fa1-4177-bb22-36831f0d7059"",
                    ""path"": ""<XRController>/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDeviceOrientation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CheatsConsole"",
            ""id"": ""752ab6fc-31a5-4a71-95bf-7dadfb4c65de"",
            ""actions"": [
                {
                    ""name"": ""CheatConsole"",
                    ""type"": ""Button"",
                    ""id"": ""dbcffa74-c8b6-4b56-90e0-318ea036a73f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c8c5165d-dde2-4af3-9622-048dc3d3dba0"",
                    ""path"": ""<Keyboard>/backslash"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""CheatConsole"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4a0ad87-65dc-40b4-bf89-16e375fbb7af"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Computer"",
                    ""action"": ""CheatConsole"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""None"",
            ""id"": ""d8f646cc-9fdc-44a7-a5c3-12f6fc07dc55"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""a5a00b0f-9f42-4158-a3de-62ac226eee5a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fae21406-1a3d-49fe-baf4-d54f14369a32"",
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
            ""name"": ""Computer"",
            ""bindingGroup"": ""Computer"",
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
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Camera = m_Gameplay.FindAction("Camera", throwIfNotFound: true);
        m_Gameplay_Fire = m_Gameplay.FindAction("Fire", throwIfNotFound: true);
        m_Gameplay_SecondaryFire = m_Gameplay.FindAction("Secondary Fire", throwIfNotFound: true);
        m_Gameplay_Spell1 = m_Gameplay.FindAction("Spell 1", throwIfNotFound: true);
        m_Gameplay_Spell2 = m_Gameplay.FindAction("Spell 2", throwIfNotFound: true);
        m_Gameplay_Spell3 = m_Gameplay.FindAction("Spell 3", throwIfNotFound: true);
        m_Gameplay_Spell4 = m_Gameplay.FindAction("Spell 4", throwIfNotFound: true);
        m_Gameplay_Run = m_Gameplay.FindAction("Run", throwIfNotFound: true);
        m_Gameplay_Dash = m_Gameplay.FindAction("Dash", throwIfNotFound: true);
        m_Gameplay_QuickSave = m_Gameplay.FindAction("QuickSave", throwIfNotFound: true);
        m_Gameplay_QuickLoad = m_Gameplay.FindAction("QuickLoad", throwIfNotFound: true);
        m_Gameplay_Pause = m_Gameplay.FindAction("Pause", throwIfNotFound: true);
        m_Gameplay_CheatConsole = m_Gameplay.FindAction("Cheat Console", throwIfNotFound: true);
        m_Gameplay_Interact = m_Gameplay.FindAction("Interact", throwIfNotFound: true);
        m_Gameplay_PreviousAndNextSpellMouseScroll = m_Gameplay.FindAction("PreviousAndNextSpellMouseScroll", throwIfNotFound: true);
        m_Gameplay_NextSpell = m_Gameplay.FindAction("Next Spell", throwIfNotFound: true);
        m_Gameplay_PreviousSpell = m_Gameplay.FindAction("Previous Spell", throwIfNotFound: true);
        // Interface
        m_Interface = asset.FindActionMap("Interface", throwIfNotFound: true);
        m_Interface_TrackedDeviceOrientation = m_Interface.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
        m_Interface_TrackedDevicePosition = m_Interface.FindAction("TrackedDevicePosition", throwIfNotFound: true);
        m_Interface_RightClick = m_Interface.FindAction("RightClick", throwIfNotFound: true);
        m_Interface_MiddleClick = m_Interface.FindAction("MiddleClick", throwIfNotFound: true);
        m_Interface_ScrollWheel = m_Interface.FindAction("ScrollWheel", throwIfNotFound: true);
        m_Interface_Click = m_Interface.FindAction("Click", throwIfNotFound: true);
        m_Interface_Point = m_Interface.FindAction("Point", throwIfNotFound: true);
        m_Interface_Cancel = m_Interface.FindAction("Cancel", throwIfNotFound: true);
        m_Interface_Submit = m_Interface.FindAction("Submit", throwIfNotFound: true);
        m_Interface_Navigate = m_Interface.FindAction("Navigate", throwIfNotFound: true);
        m_Interface_Pause = m_Interface.FindAction("Pause", throwIfNotFound: true);
        // AbilityChoice
        m_AbilityChoice = asset.FindActionMap("AbilityChoice", throwIfNotFound: true);
        m_AbilityChoice_TrackedDeviceOrientation = m_AbilityChoice.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
        m_AbilityChoice_TrackedDevicePosition = m_AbilityChoice.FindAction("TrackedDevicePosition", throwIfNotFound: true);
        m_AbilityChoice_RightClick = m_AbilityChoice.FindAction("RightClick", throwIfNotFound: true);
        m_AbilityChoice_MiddleClick = m_AbilityChoice.FindAction("MiddleClick", throwIfNotFound: true);
        m_AbilityChoice_ScrollWheel = m_AbilityChoice.FindAction("ScrollWheel", throwIfNotFound: true);
        m_AbilityChoice_Click = m_AbilityChoice.FindAction("Click", throwIfNotFound: true);
        m_AbilityChoice_Point = m_AbilityChoice.FindAction("Point", throwIfNotFound: true);
        m_AbilityChoice_Cancel = m_AbilityChoice.FindAction("Cancel", throwIfNotFound: true);
        m_AbilityChoice_Submit = m_AbilityChoice.FindAction("Submit", throwIfNotFound: true);
        m_AbilityChoice_Navigate = m_AbilityChoice.FindAction("Navigate", throwIfNotFound: true);
        // CheatsConsole
        m_CheatsConsole = asset.FindActionMap("CheatsConsole", throwIfNotFound: true);
        m_CheatsConsole_CheatConsole = m_CheatsConsole.FindAction("CheatConsole", throwIfNotFound: true);
        // None
        m_None = asset.FindActionMap("None", throwIfNotFound: true);
        m_None_Newaction = m_None.FindAction("New action", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Movement;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Camera;
    private readonly InputAction m_Gameplay_Fire;
    private readonly InputAction m_Gameplay_SecondaryFire;
    private readonly InputAction m_Gameplay_Spell1;
    private readonly InputAction m_Gameplay_Spell2;
    private readonly InputAction m_Gameplay_Spell3;
    private readonly InputAction m_Gameplay_Spell4;
    private readonly InputAction m_Gameplay_Run;
    private readonly InputAction m_Gameplay_Dash;
    private readonly InputAction m_Gameplay_QuickSave;
    private readonly InputAction m_Gameplay_QuickLoad;
    private readonly InputAction m_Gameplay_Pause;
    private readonly InputAction m_Gameplay_CheatConsole;
    private readonly InputAction m_Gameplay_Interact;
    private readonly InputAction m_Gameplay_PreviousAndNextSpellMouseScroll;
    private readonly InputAction m_Gameplay_NextSpell;
    private readonly InputAction m_Gameplay_PreviousSpell;
    public struct GameplayActions
    {
        private @InputActions m_Wrapper;
        public GameplayActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Camera => m_Wrapper.m_Gameplay_Camera;
        public InputAction @Fire => m_Wrapper.m_Gameplay_Fire;
        public InputAction @SecondaryFire => m_Wrapper.m_Gameplay_SecondaryFire;
        public InputAction @Spell1 => m_Wrapper.m_Gameplay_Spell1;
        public InputAction @Spell2 => m_Wrapper.m_Gameplay_Spell2;
        public InputAction @Spell3 => m_Wrapper.m_Gameplay_Spell3;
        public InputAction @Spell4 => m_Wrapper.m_Gameplay_Spell4;
        public InputAction @Run => m_Wrapper.m_Gameplay_Run;
        public InputAction @Dash => m_Wrapper.m_Gameplay_Dash;
        public InputAction @QuickSave => m_Wrapper.m_Gameplay_QuickSave;
        public InputAction @QuickLoad => m_Wrapper.m_Gameplay_QuickLoad;
        public InputAction @Pause => m_Wrapper.m_Gameplay_Pause;
        public InputAction @CheatConsole => m_Wrapper.m_Gameplay_CheatConsole;
        public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
        public InputAction @PreviousAndNextSpellMouseScroll => m_Wrapper.m_Gameplay_PreviousAndNextSpellMouseScroll;
        public InputAction @NextSpell => m_Wrapper.m_Gameplay_NextSpell;
        public InputAction @PreviousSpell => m_Wrapper.m_Gameplay_PreviousSpell;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Camera.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCamera;
                @Fire.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFire;
                @SecondaryFire.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryFire;
                @SecondaryFire.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryFire;
                @SecondaryFire.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryFire;
                @Spell1.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell1;
                @Spell1.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell1;
                @Spell1.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell1;
                @Spell2.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell2;
                @Spell2.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell2;
                @Spell2.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell2;
                @Spell3.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell3;
                @Spell3.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell3;
                @Spell3.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell3;
                @Spell4.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell4;
                @Spell4.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell4;
                @Spell4.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpell4;
                @Run.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                @Dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @QuickSave.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickSave;
                @QuickSave.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickSave;
                @QuickSave.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickSave;
                @QuickLoad.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickLoad;
                @QuickLoad.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickLoad;
                @QuickLoad.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickLoad;
                @Pause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @CheatConsole.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCheatConsole;
                @CheatConsole.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCheatConsole;
                @CheatConsole.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCheatConsole;
                @Interact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @PreviousAndNextSpellMouseScroll.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviousAndNextSpellMouseScroll;
                @PreviousAndNextSpellMouseScroll.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviousAndNextSpellMouseScroll;
                @PreviousAndNextSpellMouseScroll.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviousAndNextSpellMouseScroll;
                @NextSpell.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextSpell;
                @NextSpell.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextSpell;
                @NextSpell.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextSpell;
                @PreviousSpell.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviousSpell;
                @PreviousSpell.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviousSpell;
                @PreviousSpell.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviousSpell;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @SecondaryFire.started += instance.OnSecondaryFire;
                @SecondaryFire.performed += instance.OnSecondaryFire;
                @SecondaryFire.canceled += instance.OnSecondaryFire;
                @Spell1.started += instance.OnSpell1;
                @Spell1.performed += instance.OnSpell1;
                @Spell1.canceled += instance.OnSpell1;
                @Spell2.started += instance.OnSpell2;
                @Spell2.performed += instance.OnSpell2;
                @Spell2.canceled += instance.OnSpell2;
                @Spell3.started += instance.OnSpell3;
                @Spell3.performed += instance.OnSpell3;
                @Spell3.canceled += instance.OnSpell3;
                @Spell4.started += instance.OnSpell4;
                @Spell4.performed += instance.OnSpell4;
                @Spell4.canceled += instance.OnSpell4;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @QuickSave.started += instance.OnQuickSave;
                @QuickSave.performed += instance.OnQuickSave;
                @QuickSave.canceled += instance.OnQuickSave;
                @QuickLoad.started += instance.OnQuickLoad;
                @QuickLoad.performed += instance.OnQuickLoad;
                @QuickLoad.canceled += instance.OnQuickLoad;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @CheatConsole.started += instance.OnCheatConsole;
                @CheatConsole.performed += instance.OnCheatConsole;
                @CheatConsole.canceled += instance.OnCheatConsole;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @PreviousAndNextSpellMouseScroll.started += instance.OnPreviousAndNextSpellMouseScroll;
                @PreviousAndNextSpellMouseScroll.performed += instance.OnPreviousAndNextSpellMouseScroll;
                @PreviousAndNextSpellMouseScroll.canceled += instance.OnPreviousAndNextSpellMouseScroll;
                @NextSpell.started += instance.OnNextSpell;
                @NextSpell.performed += instance.OnNextSpell;
                @NextSpell.canceled += instance.OnNextSpell;
                @PreviousSpell.started += instance.OnPreviousSpell;
                @PreviousSpell.performed += instance.OnPreviousSpell;
                @PreviousSpell.canceled += instance.OnPreviousSpell;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Interface
    private readonly InputActionMap m_Interface;
    private IInterfaceActions m_InterfaceActionsCallbackInterface;
    private readonly InputAction m_Interface_TrackedDeviceOrientation;
    private readonly InputAction m_Interface_TrackedDevicePosition;
    private readonly InputAction m_Interface_RightClick;
    private readonly InputAction m_Interface_MiddleClick;
    private readonly InputAction m_Interface_ScrollWheel;
    private readonly InputAction m_Interface_Click;
    private readonly InputAction m_Interface_Point;
    private readonly InputAction m_Interface_Cancel;
    private readonly InputAction m_Interface_Submit;
    private readonly InputAction m_Interface_Navigate;
    private readonly InputAction m_Interface_Pause;
    public struct InterfaceActions
    {
        private @InputActions m_Wrapper;
        public InterfaceActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @TrackedDeviceOrientation => m_Wrapper.m_Interface_TrackedDeviceOrientation;
        public InputAction @TrackedDevicePosition => m_Wrapper.m_Interface_TrackedDevicePosition;
        public InputAction @RightClick => m_Wrapper.m_Interface_RightClick;
        public InputAction @MiddleClick => m_Wrapper.m_Interface_MiddleClick;
        public InputAction @ScrollWheel => m_Wrapper.m_Interface_ScrollWheel;
        public InputAction @Click => m_Wrapper.m_Interface_Click;
        public InputAction @Point => m_Wrapper.m_Interface_Point;
        public InputAction @Cancel => m_Wrapper.m_Interface_Cancel;
        public InputAction @Submit => m_Wrapper.m_Interface_Submit;
        public InputAction @Navigate => m_Wrapper.m_Interface_Navigate;
        public InputAction @Pause => m_Wrapper.m_Interface_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Interface; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InterfaceActions set) { return set.Get(); }
        public void SetCallbacks(IInterfaceActions instance)
        {
            if (m_Wrapper.m_InterfaceActionsCallbackInterface != null)
            {
                @TrackedDeviceOrientation.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDevicePosition.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnTrackedDevicePosition;
                @RightClick.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnRightClick;
                @MiddleClick.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnMiddleClick;
                @ScrollWheel.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnScrollWheel;
                @Click.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnClick;
                @Point.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnPoint;
                @Cancel.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnCancel;
                @Submit.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnSubmit;
                @Navigate.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnNavigate;
                @Pause.started -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_InterfaceActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_InterfaceActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
                @TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public InterfaceActions @Interface => new InterfaceActions(this);

    // AbilityChoice
    private readonly InputActionMap m_AbilityChoice;
    private IAbilityChoiceActions m_AbilityChoiceActionsCallbackInterface;
    private readonly InputAction m_AbilityChoice_TrackedDeviceOrientation;
    private readonly InputAction m_AbilityChoice_TrackedDevicePosition;
    private readonly InputAction m_AbilityChoice_RightClick;
    private readonly InputAction m_AbilityChoice_MiddleClick;
    private readonly InputAction m_AbilityChoice_ScrollWheel;
    private readonly InputAction m_AbilityChoice_Click;
    private readonly InputAction m_AbilityChoice_Point;
    private readonly InputAction m_AbilityChoice_Cancel;
    private readonly InputAction m_AbilityChoice_Submit;
    private readonly InputAction m_AbilityChoice_Navigate;
    public struct AbilityChoiceActions
    {
        private @InputActions m_Wrapper;
        public AbilityChoiceActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @TrackedDeviceOrientation => m_Wrapper.m_AbilityChoice_TrackedDeviceOrientation;
        public InputAction @TrackedDevicePosition => m_Wrapper.m_AbilityChoice_TrackedDevicePosition;
        public InputAction @RightClick => m_Wrapper.m_AbilityChoice_RightClick;
        public InputAction @MiddleClick => m_Wrapper.m_AbilityChoice_MiddleClick;
        public InputAction @ScrollWheel => m_Wrapper.m_AbilityChoice_ScrollWheel;
        public InputAction @Click => m_Wrapper.m_AbilityChoice_Click;
        public InputAction @Point => m_Wrapper.m_AbilityChoice_Point;
        public InputAction @Cancel => m_Wrapper.m_AbilityChoice_Cancel;
        public InputAction @Submit => m_Wrapper.m_AbilityChoice_Submit;
        public InputAction @Navigate => m_Wrapper.m_AbilityChoice_Navigate;
        public InputActionMap Get() { return m_Wrapper.m_AbilityChoice; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AbilityChoiceActions set) { return set.Get(); }
        public void SetCallbacks(IAbilityChoiceActions instance)
        {
            if (m_Wrapper.m_AbilityChoiceActionsCallbackInterface != null)
            {
                @TrackedDeviceOrientation.started -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDevicePosition.started -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnTrackedDevicePosition;
                @RightClick.started -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnRightClick;
                @MiddleClick.started -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnMiddleClick;
                @ScrollWheel.started -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnScrollWheel;
                @Click.started -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnClick;
                @Point.started -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnPoint;
                @Cancel.started -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnCancel;
                @Submit.started -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnSubmit;
                @Navigate.started -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_AbilityChoiceActionsCallbackInterface.OnNavigate;
            }
            m_Wrapper.m_AbilityChoiceActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
                @TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
            }
        }
    }
    public AbilityChoiceActions @AbilityChoice => new AbilityChoiceActions(this);

    // CheatsConsole
    private readonly InputActionMap m_CheatsConsole;
    private ICheatsConsoleActions m_CheatsConsoleActionsCallbackInterface;
    private readonly InputAction m_CheatsConsole_CheatConsole;
    public struct CheatsConsoleActions
    {
        private @InputActions m_Wrapper;
        public CheatsConsoleActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @CheatConsole => m_Wrapper.m_CheatsConsole_CheatConsole;
        public InputActionMap Get() { return m_Wrapper.m_CheatsConsole; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CheatsConsoleActions set) { return set.Get(); }
        public void SetCallbacks(ICheatsConsoleActions instance)
        {
            if (m_Wrapper.m_CheatsConsoleActionsCallbackInterface != null)
            {
                @CheatConsole.started -= m_Wrapper.m_CheatsConsoleActionsCallbackInterface.OnCheatConsole;
                @CheatConsole.performed -= m_Wrapper.m_CheatsConsoleActionsCallbackInterface.OnCheatConsole;
                @CheatConsole.canceled -= m_Wrapper.m_CheatsConsoleActionsCallbackInterface.OnCheatConsole;
            }
            m_Wrapper.m_CheatsConsoleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CheatConsole.started += instance.OnCheatConsole;
                @CheatConsole.performed += instance.OnCheatConsole;
                @CheatConsole.canceled += instance.OnCheatConsole;
            }
        }
    }
    public CheatsConsoleActions @CheatsConsole => new CheatsConsoleActions(this);

    // None
    private readonly InputActionMap m_None;
    private INoneActions m_NoneActionsCallbackInterface;
    private readonly InputAction m_None_Newaction;
    public struct NoneActions
    {
        private @InputActions m_Wrapper;
        public NoneActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_None_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_None; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NoneActions set) { return set.Get(); }
        public void SetCallbacks(INoneActions instance)
        {
            if (m_Wrapper.m_NoneActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_NoneActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_NoneActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_NoneActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_NoneActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public NoneActions @None => new NoneActions(this);
    private int m_ComputerSchemeIndex = -1;
    public InputControlScheme ComputerScheme
    {
        get
        {
            if (m_ComputerSchemeIndex == -1) m_ComputerSchemeIndex = asset.FindControlSchemeIndex("Computer");
            return asset.controlSchemes[m_ComputerSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnSecondaryFire(InputAction.CallbackContext context);
        void OnSpell1(InputAction.CallbackContext context);
        void OnSpell2(InputAction.CallbackContext context);
        void OnSpell3(InputAction.CallbackContext context);
        void OnSpell4(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnQuickSave(InputAction.CallbackContext context);
        void OnQuickLoad(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnCheatConsole(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnPreviousAndNextSpellMouseScroll(InputAction.CallbackContext context);
        void OnNextSpell(InputAction.CallbackContext context);
        void OnPreviousSpell(InputAction.CallbackContext context);
    }
    public interface IInterfaceActions
    {
        void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
        void OnTrackedDevicePosition(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IAbilityChoiceActions
    {
        void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
        void OnTrackedDevicePosition(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
    }
    public interface ICheatsConsoleActions
    {
        void OnCheatConsole(InputAction.CallbackContext context);
    }
    public interface INoneActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
