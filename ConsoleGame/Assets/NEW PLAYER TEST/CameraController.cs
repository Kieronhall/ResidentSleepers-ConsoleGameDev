using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEWPLAYER
{
    public class CameraController : MonoBehaviour
    {
        public float cameraSmoothingFactor = 1;
        public bool invertY;
        public bool invertX;

        [SerializeField] private float lookUpMax = 60;
        [SerializeField] public float lookUpMin = -60;

        private Quaternion cameraRotation;

        void Start()
        {
            cameraRotation = transform.localRotation;
        }

        void Update()
        {
            if (invertY)
            {
                cameraRotation.x += Input.GetAxis("Y Axis") * cameraSmoothingFactor;
            }
            else
            {
                cameraRotation.x += Input.GetAxis("Y Axis") * cameraSmoothingFactor * -1;
            }

            if (invertX)
            {
                cameraRotation.y += Input.GetAxis("X Axis") * cameraSmoothingFactor * -1;
            }
            else
            {
                cameraRotation.y += Input.GetAxis("X Axis") * cameraSmoothingFactor;
            }

            cameraRotation.x = Mathf.Clamp(cameraRotation.x, lookUpMin, lookUpMax);
            
            transform.localRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, cameraRotation.z);
        }
    }
}