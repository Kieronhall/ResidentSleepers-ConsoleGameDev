using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.PS4;
using UnityEngine.InputSystem.PS4.ControllerSample;
using UnityEngine.InputSystem.Users;


namespace UnityEngine.InputSystem.PS4.ControllerSample
{
    /// <summary>
    /// This class acts as a manager to facilitate the assignment of control devices (that users are holding) to the
    /// gamepads on the screen.
    /// </summary>
    /// <remarks>Within the Input System this behaviour can be replaced with PlayerInputManagers and PlayerInputs, if you
    /// want the Input System to handle controller delegation</remarks>
    public class GamepadManager : MonoBehaviour
    {
        private const int k_MaxGamepads = 4;

        [SerializeField]
        SampleGamepad[] gamepads;
        [SerializeField]
        bool warnOnUnknownDeviceType = false;

        void OnEnable()
        {
            InputUser.listenForUnpairedDeviceActivity = 1;
            InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
            InputUser.onChange += OnInputUserChange;
        }

        void OnInputUserChange(InputUser user, InputUserChange change, InputDevice device)
        {
            SampleGamepad userGamepad = GetGamepadDisplayForDevice(device);
            if (userGamepad == null)
            {
                return;
            }

            switch (change)
            {
                case InputUserChange.DeviceLost: //Device Disconnected
                    userGamepad.UnpairUser();
                    break;
                default:
                    break;
            }
        }

        private SampleGamepad GetGamepadDisplayForDevice(InputDevice device)
        {
            if (device == null)
            {
                return null;
            }

            foreach (var gamepad in gamepads)
            {
                if (gamepad.gamepadDevice == device)
                {
                    return gamepad;
                }
            }

            return null;
        }

        void OnUnpairedDeviceUsed(InputControl inputControl, InputEventPtr eventPtr)
        {
            PairUserWithDevice(inputControl?.device);
        }

        void PairUserWithDevice(InputDevice device)
        {
            //When an unpaired device is used, bind it to the gamepad of the slotIndex
            if (device == null)
            {
                Debug.LogError("Invalid input device");
                return;
            }

            int slotIndex = -1;
            switch (device)
            {
                case DualShockGamepadPS4 dualshockPS4:
                    slotIndex = dualshockPS4.slotIndex;
                    break;
#if UNITY_EDITOR
                case DualShock4GamepadHID dualshockPC:
                    slotIndex = 0;
                    break;
#endif
                default:
                {
                    if (warnOnUnknownDeviceType)
                    {
                        Debug.LogWarning($"Unknown Device. Name: {device.name} , ID: {device.deviceId}");
                    }

                    return;
                }
            }

            if (slotIndex == -1)
            {
                Debug.LogError("Failed to get slot index");
                return;
            }

            //Check there is a valid game pad
            if (slotIndex >= k_MaxGamepads || gamepads.Length < slotIndex || gamepads[slotIndex] == null)
            {
                Debug.LogError($"No gamepad assigned to slot index {slotIndex}");
            }

            gamepads[slotIndex].SetupUserWithDevice(device);
        }

        void OnValidate()
        {
            //Only 4 gamepads are supported
            if (gamepads is { Length: > k_MaxGamepads })
            {
                Debug.LogError($"Cannot use more then {k_MaxGamepads} Gamepads");
            }
        }
    }
}
