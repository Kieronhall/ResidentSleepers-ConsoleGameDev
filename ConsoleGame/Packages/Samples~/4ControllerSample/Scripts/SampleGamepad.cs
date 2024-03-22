using System;
using System.Collections;
using System.Collections.Generic;
using InputSample;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.Serialization;

#if UNITY_EDITOR && UNITY_PS4
using DualShockPad = UnityEngine.InputSystem.DualShock.DualShock4GamepadHID;
#elif UNITY_PS4
using DualShockPad = UnityEngine.InputSystem.PS4.DualShockGamepadPS4;
#endif

namespace UnityEngine.InputSystem.PS4.ControllerSample
{
    public class SampleGamepad : MonoBehaviour
    {
        [SerializeField] private GamepadDisplay gamepadDisplay;
        [SerializeField] private GamepadHaptics gamepadHaptics;

        public InputUser inputUser { get; private set; }
        public DualShockPad gamepadDevice { get; private set; }
        public GamepadControls gamepadControls { get; private set; }
        public IInputActionCollection2 actionCollection => gamepadControls;

        void Awake()
        {
            gamepadControls = new GamepadControls();
        }

        void Start()
        {
            gamepadDisplay.Initalize();
        }

        public bool SetupUserWithDevice(InputDevice device)
        {
            if (device is not DualShockPad dualShockGamepad)
            {
                Debug.LogWarning("Unexpected device type");
                return false;
            }

            inputUser = InputUser.PerformPairingWithDevice(dualShockGamepad, options: InputUserPairingOptions.UnpairCurrentDevicesFromUser);
            inputUser.AssociateActionsWithUser(actionCollection);
            actionCollection.Enable();
            gamepadDevice = dualShockGamepad;

            gamepadDisplay.enabled = true;
            gamepadHaptics.enabled = true;

            return true;
        }


        public bool UnpairUser()
        {
            InputSystem.RemoveDevice(gamepadDevice);
            inputUser.UnpairDevices();
            actionCollection.Disable();
            gamepadDevice = null;
            gamepadDisplay.UpdateUsernameOutput();
            inputUser = default(InputUser);

            gamepadDisplay.enabled = false;
            gamepadHaptics.enabled = false;

            return true;
        }

        /// <summary>
        /// Get the mock dualshock gamepad ID as it is not provided on PC
        /// </summary>
        /// <remarks>This assumes that only Dualshock pads are plugged in and no other devices</remarks>
        /// <param name="pad"></param>
        /// <returns></returns>
        public static int GetSlotIndex(DualShockPad pad)
        {
            for (int i = 0; i < DualShockPad.all.Count; i++)
            {
                if (pad == DualShockPad.all[i])
                {
                    return i;
                }
            }

            return -1;
        }
    }
}

