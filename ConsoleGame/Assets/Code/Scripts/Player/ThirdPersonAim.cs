using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace ThirdPerson
{
    public class ThirdPersonAim : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
        [SerializeField] private float normalSensitivity;
        [SerializeField] private float aimSensitivity;
        [SerializeField] private LayerMask aimColldierMask;

        private ThirdPersonController controller;
        private PlayerControls _input;

        private void Awake()
        {
            _input = GetComponent<PlayerControls>();
            controller = GetComponent<ThirdPersonController>();
        }

        private void Update()
        {
            Vector3 mouseWorldPosition = Vector3.zero;

            Vector2 screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColldierMask))
            {
                
            }

            if (_input.aim)
            {
                aimVirtualCamera.gameObject.SetActive(true);
                controller.SetSensitivity(aimSensitivity);
                controller.SetRotationOnMove(false);

                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            }
            else
            {
                aimVirtualCamera.gameObject.SetActive(false);
                controller.SetSensitivity(normalSensitivity);
                controller.SetRotationOnMove(true);
            }
        }
    }
}