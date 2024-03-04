using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Users;


#if UNITY_EDITOR && UNITY_PS5
using DualSenseGamepad = UnityEngine.InputSystem.PS5.DualSenseGamepadPC;
#elif UNITY_PS5
using DualSenseGamepad = UnityEngine.InputSystem.PS5.DualSenseGamepad;
#endif

#if UNITY_EDITOR && UNITY_PS4
using DualSensePad = UnityEngine.InputSystem.DualShock.DualShock4GamepadHID;
#elif UNITY_PS4
using DualSensePad = UnityEngine.InputSystem.PS4.DualShockGamepadPS4;
#endif

#if UNITY_PS5
using UnityEngine.PS5;
using PSInput = UnityEngine.PS5.PS5Input;
#elif UNITY_PS4
using PSInput = UnityEngine.PS4.PS4Input;
#endif

namespace UnityEngine.InputSystem.PS4.ControllerSample
{
    public class GamepadDisplay : MonoBehaviour
    {
        [SerializeField] private SampleGamepad Gamepad;

        [SerializeField] private GameObject controllerModel;

        [Header("Buttons")]
        [SerializeField] private ButtonVisuals visuals;
        [SerializeField] private ButtonImages controlImages;

        [Header("Lightbar")]
        [SerializeField] private Color startLightbarColor;
        [SerializeField] private Color uninitalizedLightbarColor;
        [SerializeField] private LightbarVisuals lightbarVisuals;

        [Header("Username")]
        [SerializeField] private TextMesh usernameTextBox;

        //Colour Lerp Routine
        Coroutine lerpLightbarColorCR;

        public void Initalize()
        {
            //Init touch pad images so they start in the correct state without yet being performed
            UpdateTouchpadTouch(controlImages.touch0, new Vector2(-1,-1));
            UpdateTouchpadTouch(controlImages.touch1, new Vector2(-1,-1));

            Gamepad.gamepadControls.Main.PressTriangle.started += _ => { StartLerpToColor(Color.blue); };
            Gamepad.gamepadControls.Main.PressCircle.started += _ => { StartLerpToColor(Color.red); };
            Gamepad.gamepadControls.Main.PressCross.started += _ => { StartLerpToColor(Color.magenta); };
            Gamepad.gamepadControls.Main.PressSquare.started += _ => { StartLerpToColor(Color.green); };

            Gamepad.gamepadControls.Main.TouchpadTouch0.performed += context =>
            {
                UpdateTouchpadTouch(controlImages.touch0, context.ReadValue<Vector2>());
            };

            Gamepad.gamepadControls.Main.TouchpadTouch1.performed += context =>
            {
                UpdateTouchpadTouch(controlImages.touch1, context.ReadValue<Vector2>());
            };

            UpdateUsernameOutput();
            SetLightbarColor(uninitalizedLightbarColor);
        }

        void OnEnable()
        {
            SetLightbarColor(startLightbarColor);
            UpdateUsernameOutput();
        }

        void OnDisable()
        {
            UpdateUsernameOutput();
            SetLightbarColor(uninitalizedLightbarColor);
        }

        void Update()
        {
            UpdateButtons();
            UpdateStickPositions();
            UpdateTriggerColours();
            UpdateGyro();
        }


        public void UpdateUsernameOutput()
        {
            if (Gamepad.gamepadDevice == null)
            {
                usernameTextBox.text = "DISCONNECTED";
                return;
            }

            int slotIndex = SampleGamepad.GetSlotIndex(Gamepad.gamepadDevice);
            PSInput.RefreshUsersDetails(slotIndex);
            PSInput.LoggedInUser user = PSInput.GetUsersDetails(slotIndex);
            PSInput.GetPadControllerInformation(slotIndex, out _, out _, out _,
                out _, out _, out PSInput.ConnectionType connectionType);
            usernameTextBox.text = $"{user.userName}\n{connectionType.ToString()}";
        }

        private void UpdateButtons()
        {
            //Control Pad (X,O,Sq,X)
            UpdateButtonDraw(controlImages.triangle, Gamepad.gamepadControls.Main.PressTriangle.IsPressed());
            UpdateButtonDraw(controlImages.circle, Gamepad.gamepadControls.Main.PressCircle.IsPressed());
            UpdateButtonDraw(controlImages.square, Gamepad.gamepadControls.Main.PressSquare.IsPressed());
            UpdateButtonDraw(controlImages.cross, Gamepad.gamepadControls.Main.PressCross.IsPressed());

            //Dpad
            Vector2 dpadValue = Gamepad.gamepadControls.Main.Dpad.ReadValue<Vector2>().normalized;
            const float dPadDeadzone = 0.2f;
            UpdateButtonDraw(controlImages.dPadUp, dpadValue.y > dPadDeadzone);
            UpdateButtonDraw(controlImages.dPadDown, dpadValue.y < -dPadDeadzone);
            UpdateButtonDraw(controlImages.dPadLeft, dpadValue.x < -dPadDeadzone);
            UpdateButtonDraw(controlImages.dPadRight, dpadValue.x > dPadDeadzone);

            //Extra buttons (options, start, share)
            UpdateButtonDraw(controlImages.share, Gamepad.gamepadControls.Main.PressShare.IsPressed());
            UpdateButtonDraw(controlImages.options, Gamepad.gamepadControls.Main.PressOptions.IsPressed());
            //Button only bound on PC
            UpdateButtonDraw(controlImages.playstation, Gamepad.gamepadControls.Main.PressPlaystationButton.IsPressed());

            //Bumpers
            UpdateButtonDraw(controlImages.l1, Gamepad.gamepadControls.Main.LeftBumper.IsPressed());
            UpdateButtonDraw(controlImages.r1, Gamepad.gamepadControls.Main.RightBumper.IsPressed());

            //Press thumbsticks
            UpdateButtonDraw(controlImages.leftStick, Gamepad.gamepadControls.Main.PressLeftStick.IsPressed());
            UpdateButtonDraw(controlImages.rightStick, Gamepad.gamepadControls.Main.PressRightStick.IsPressed());

            UpdateButtonDraw(controlImages.touchpad, Gamepad.gamepadControls.Main.PressTouchpad.IsPressed());
        }

        private void UpdateGyro()
        {
            #if !UNITY_EDITOR
            if (Gamepad.gamepadDevice == null || Gamepad.gamepadDevice.orientation == null)
            {
                return;
            }

            controllerModel.transform.rotation = Gamepad.gamepadDevice.orientation.ReadValue();
            #endif
        }

        private void UpdateStickPositions()
        {
            UpdateStickPosition(controlImages.leftStick, Gamepad.gamepadControls.Main.LeftStickPos);
            UpdateStickPosition(controlImages.rightStick, Gamepad.gamepadControls.Main.RightStickPos);
        }

        private void UpdateStickPosition(SpriteRenderer stickSprite, InputAction control)
        {
            const float moveScale = 0.4f;
            Vector2 controlPos = control.ReadValue<Vector2>();
            stickSprite.transform.localPosition = new Vector3(controlPos.x, controlPos.y) * moveScale;
        }

        private void UpdateTriggerColours()
        {
            UpdateTriggerColour(controlImages.l2, Gamepad.gamepadControls.Main.LeftTrigger);
            UpdateTriggerColour(controlImages.r2, Gamepad.gamepadControls.Main.RightTrigger);
        }

        private void UpdateTriggerColour(SpriteRenderer stickSprite, InputAction control)
        {
            float controlPos = control.ReadValue<float>();
            stickSprite.color = visuals.m_TriggerGradient.Evaluate(controlPos);
        }

        private void UpdateTouchpadTouch(SpriteRenderer touchSprite, Vector2 touchPos)
        {
            //Ignore -1, -1 values as they are used when the touch pad is not being touched
            if (touchPos.x < 0 || touchPos.y < 0)
            {
                touchSprite.enabled = false;
                return;
            }

            //Remap range of 0 to -1 from touch position to the size of the touch pad in local space
            float x = touchPos.x.Remap(0, 1, 0, 3.57f);
            float y = touchPos.y.Remap(0, 1, 0, 1.35f);

            touchSprite.enabled = true;
            touchSprite.transform.localPosition = new Vector3(x, -y, -0.1f);

        }

        private void StartLerpToColor(Color newColour)
        {
            const float timeToLerp = 0.5f;
            if (lerpLightbarColorCR != null)
            {
                StopCoroutine(lerpLightbarColorCR);
            }

            lerpLightbarColorCR = StartCoroutine(LerpToColor(newColour, timeToLerp));
        }

        private IEnumerator LerpToColor(Color newColour, float time)
        {
            float elapsedTime = 0f;
            time = Mathf.Max(0f, time);
            Color startColour = Color.white;
            while (elapsedTime < time)
            {
                SetLightbarColor(Color.Lerp(startColour, newColour, elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public void SetLightbarColor(Color color)
        {
            lightbarVisuals.SetColour(color);
            Gamepad.gamepadDevice?.SetLightBarColor(color);
        }

        private void UpdateButtonDraw(SpriteRenderer image, bool isPressed)
        {
            if (image == null || visuals == null)
            {
                return;
            }
            image.color = isPressed ? visuals.m_InputOn : visuals.m_InputOff;
        }
    }
}


public static class ExtensionMethods {

    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
