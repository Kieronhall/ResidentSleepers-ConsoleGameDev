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
        [SerializeField] private Transform debugTransform;

        private ThirdPersonController controller;
        private PlayerControls _input;

        //Placeholder gun code
        public GameObject gun;

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
            Transform hitTransform = null;
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColldierMask))
            {
                //debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
                hitTransform = raycastHit.transform;
            }

            if (_input.aim)
            {
                // Aim Animation
                playerAnimAim();
                gunShow();

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
                // Aim Animation
                playerAnimAimFalse();
                gunHide();

                aimVirtualCamera.gameObject.SetActive(false);
                controller.SetSensitivity(normalSensitivity);
                controller.SetRotationOnMove(true);
            }

            if (_input.shoot)
            {
                if (hitTransform != null)
                // if not null Hit something
                {
                    if (hitTransform.GetComponent<BulletTarget>() != null)
                    {
                        Debug.Log("TARGET HITTED");
                        //Instantiate(null);
                        
                    }
                    else
                    {
                        Debug.Log(" hit object");
                        //Instantiate(null);
                        
                    }
                }
                _input.shoot = false;          
            }
        }

        //Placeholder gun code
        public void gunShow()
        {
            if (gun != null)
            {
                gun.SetActive(true);
            }
        }
        public void gunHide()
        {
            if (gun != null)
            {
                gun.SetActive(false);
            }
        }

        //Animations

        public void playerAnimAim()
        {
            GetComponentInChildren<playerAnimationState>().animator.SetBool("isAiming", true);
        }
        public void playerAnimAimFalse()
        {
            GetComponentInChildren<playerAnimationState>().animator.SetBool("isAiming", false);
        }
        
        // This aren't used atm, kind of scuffed
        public void playerAnimShoot()
        {
            GetComponentInChildren<playerAnimationState>().animator.SetBool("isShooting", true);
        }
        public void playerAnimShootFalse()
        {
            GetComponentInChildren<playerAnimationState>().animator.SetBool("isShooting", false);
        }
    }
}