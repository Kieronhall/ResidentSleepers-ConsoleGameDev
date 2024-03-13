using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.PS4.ControllerSample;
using UnityEngine.Serialization;

namespace UnityEngine.InputSystem.PS4.ControllerSample
{
    public class GamepadHaptics : MonoBehaviour
    {
        [SerializeField] private SampleGamepad gamepad;
        [SerializeField] private SpriteRenderer speakerVisual;
        [SerializeField] private Gradient speakerVolumeGradient;

        [Header("Settings")]
        [SerializeField] private bool enableAudio;
        [SerializeField] private bool enableVibration;

        [Header("Triggers")]
        [SerializeField] private Audio LeftTriggerAudio;
        [SerializeField] private HapticMotorSpeeds LeftTriggerHaptics;
        [SerializeField] private Audio RightTriggerAudio;
        [SerializeField] private HapticMotorSpeeds RightTriggerHaptics;

        [Header("Touchpad Audio")]
        [SerializeField] private Audio Touch0Audio;
        [SerializeField] private Audio Touch1Audio;

        void Awake()
        {
            LeftTriggerAudio.CreateAudioSources(gameObject);
            RightTriggerAudio.CreateAudioSources(gameObject);
            Touch0Audio.CreateAudioSources(gameObject);
            Touch1Audio.CreateAudioSources(gameObject);
        }

        void Start()
        {
            gamepad.gamepadControls.Main.LeftTrigger.started += StartLeftTriggerAudio;
            gamepad.gamepadControls.Main.RightTrigger.started += StartRightTriggerAudio;
            gamepad.gamepadControls.Main.RightTrigger.canceled += StopRightTriggerAudio;

            gamepad.gamepadControls.Main.TouchpadTouch0.performed += context => PlayTouchPadAudio(0, context);
            gamepad.gamepadControls.Main.TouchpadTouch1.performed += context => PlayTouchPadAudio(1, context);
        }

        void Update()
        {
            speakerVisual.color = speakerVolumeGradient.Evaluate(GamepadOutputVolume());
        }

        void StartRightTriggerAudio(InputAction.CallbackContext obj)
        {
            RightTriggerAudio.Start(SampleGamepad.GetSlotIndex(gamepad.gamepadDevice), enableAudio, enableVibration);
            if (enableVibration)
            {
                gamepad.gamepadDevice.SetMotorSpeeds(RightTriggerHaptics.lowFrequency, RightTriggerHaptics.highFrequency);
            }
        }

        void StopRightTriggerAudio(InputAction.CallbackContext obj)
        {
            RightTriggerAudio.Stop();
            gamepad.gamepadDevice.ResetHaptics();
        }

        void StartLeftTriggerAudio(InputAction.CallbackContext obj)
        {
            LeftTriggerAudio.Start(SampleGamepad.GetSlotIndex(gamepad.gamepadDevice), enableAudio, enableVibration);
            if (enableVibration)
            {
                StartCoroutine(PlayHapticForTime(LeftTriggerAudio.audioClip.clip.length / 8f, LeftTriggerHaptics));
            }
        }

        IEnumerator PlayHapticForTime(float time, HapticMotorSpeeds motors)
        {
            gamepad.gamepadDevice.SetMotorSpeeds(motors.lowFrequency, motors.highFrequency);
            yield return new WaitForSeconds(time);
            gamepad.gamepadDevice.ResetHaptics();
        }

        void PlayTouchPadAudio(int touchID, InputAction.CallbackContext obj)
        {
            Audio audio;
            switch (touchID)
            {
                case 0:
                    audio = Touch0Audio;
                    break;
                case 1:
                    audio = Touch1Audio;
                    break;
                default:
                    Debug.LogError($"Touch {touchID} is not valid, cannot play touchpad audio");
                    return;
            }

            Vector2 touchPos = obj.ReadValue<Vector2>();

            //Ignore -1, -1 values as they are used when the touch pad is not being touched
            if (touchPos.x < 0 || touchPos.y < 0)
            {
                audio.Stop();
                return;
            }
            else
            {
                audio.StartIfNotPlaying(SampleGamepad.GetSlotIndex(gamepad.gamepadDevice), enableAudio, enableVibration);
            }

            AudioSource clipSource = audio.clipSource;

            float pitch = (1f - touchPos.y) + 0.5f;
            float clipVol = 0.25f;

            clipSource.pitch = pitch;
            clipSource.volume = clipVol;
        }

        private float GamepadOutputVolume()
        {
            return Mathf.Max(LeftTriggerAudio.GetClipSourceVolumeFromOutput(),
                RightTriggerAudio.GetClipSourceVolumeFromOutput(),
                Touch0Audio.GetClipSourceVolumeFromOutput(),
                Touch1Audio.GetClipSourceVolumeFromOutput()
            );
        }

        [Serializable]
        public struct Audio
        {
            public LoopableClip audioClip;

            [HideInInspector] public AudioSource clipSource;

            const int k_QSamples = 1024;
            static float[] s_Samples = new float[k_QSamples]; // audio samples for monitoring volume.

            public void CreateAudioSources(GameObject sourcesGameObject)
            {
                if (clipSource == null)
                {
                    clipSource = sourcesGameObject.AddComponent<AudioSource>();
                    clipSource.clip = audioClip.clip;
                    clipSource.loop = audioClip.loop;
                    clipSource.gamepadSpeakerOutputType = GamepadSpeakerOutputType.Speaker;
                }
            }

            public void Start(int slot, bool audio, bool vibration)
            {
                #if !UNITY_EDITOR
                if (audio && clipSource)
                {
                    clipSource.PlayOnGamepad(slot);
                }
                #endif
            }

            public void StartIfNotPlaying(int slot, bool audio, bool vibration)
            {
                if (audio && clipSource && !clipSource.isPlaying)
                {
                    clipSource.PlayOnGamepad(slot);
                }
            }

            public void Stop()
            {
                clipSource.Stop();
            }

            public float GetClipSourceVolumeFromOutput()
            {
                float volume = 0.0f;

                if (clipSource == null || !clipSource.isPlaying)
                {
                    return 0.0f;
                }

                if (clipSource.time > 0f)
                {
                    clipSource.GetOutputData(s_Samples, 0); // fill array with samples
                    int i;
                    var sum = 0f;

                    for (i = 0; i < k_QSamples; i++)
                        sum += s_Samples[i] * s_Samples[i]; // sum squared samples

                    volume = Mathf.Sqrt(sum / k_QSamples); // rms = square root of average
                    volume *= clipSource.volume;
                }

                return volume;
            }
        }

        [Serializable]
        public struct LoopableClip
        {
            public AudioClip clip;
            public bool loop;
        }

        [Serializable]
        public struct HapticMotorSpeeds
        {
            [Range(0,1)] public float lowFrequency;
            [Range(0,1)] public float highFrequency;
        }
    }
}

