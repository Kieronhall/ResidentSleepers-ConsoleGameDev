using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NEWPLAYER
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float speed = 6;

        private float horizontal;
        private float vertical;
        private Vector3 velocity;

        public CameraController cameraController;

        void Start()
        {
            cameraController = GetComponentInChildren<CameraController>();
        }

        void Update()
        {
            transform.Translate(velocity);
        }

        public void AddMovementInput(float forward, float right)
        {
            horizontal = forward;
            vertical = right;

            Vector3 cameraForward = cameraController.transform.forward;
            Vector3 cameraRight = cameraController.transform.right;

            Vector3 translation = horizontal * cameraController.transform.forward;
            translation += vertical * cameraController.transform.right;
            translation.y = 0;

            if (translation.magnitude > 0)
            {
                velocity = translation * speed * Time.deltaTime;
            }
            else
            {
                velocity = Vector3.zero;
            }
        }

        public float GetVelocity()
        {
            return velocity.magnitude;
        }
    }
}