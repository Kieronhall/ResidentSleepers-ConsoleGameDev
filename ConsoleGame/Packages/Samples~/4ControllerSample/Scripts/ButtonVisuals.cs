using System;
using UnityEngine;

namespace UnityEngine.InputSystem.PS4.ControllerSample
{
    [Serializable]
    public class ButtonVisuals
    {
        public Color m_InputOn = Color.white;
        public Color m_InputOff = Color.grey;

        public Gradient m_TriggerGradient;
    }
}
