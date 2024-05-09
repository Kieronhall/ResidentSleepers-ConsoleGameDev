using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System;
using ThirdPerson;

namespace ThirdPerson
{
    public class ThirdPersonAim : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
        public float normalSensitivity;
        public float aimSensitivity;
        [SerializeField] private LayerMask aimColldierMask;
        [SerializeField] private Transform debugTransform;
        [SerializeField] private Pistol_Player playerPistol;
        [SerializeField] private HealthBar healthBar;

        private ThirdPersonController controller;
        private PlayerControls input;
        private CoverController coverController;

        [Header("Gun")]
        public GameObject gun;
        public float damage = 100f;

        private void Awake()
        {
            input = GetComponent<PlayerControls>();
            controller = GetComponent<ThirdPersonController>();
            coverController = GetComponent<CoverController>();
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

            if (input.aim && !coverController.inCover)
            {
                //Animations
                playerAnimAim();
                gunShow();
                healthBar.ammoBar.SetActive(true);

                aimVirtualCamera.gameObject.SetActive(true);
                controller.SetSensitivity(aimSensitivity);
                controller.SetRotationOnMove(false);

                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

                if (input.aim && input.shoot)
                {
                    if (hitTransform != null)
                    {
                        if (playerPistol.bulletsLeft > 0)
                        {
                            playerAnimShoot();
                        }

                        if (playerPistol.bulletsLeft > 0 && playerPistol.readyToShoot)
                        {
                            playerPistol.bulletsShot = playerPistol.bulletsPerTap;
                            playerPistol.Shoot(raycastHit);
                            FMODUnity.RuntimeManager.PlayOneShot("event:/Dylan/Player/Shooting", gun.transform.position);
                            input.shoot = false;
                        }
                    }
                    else
                    {
                        // Log a warning if hitTransform is null
                        Debug.LogWarning("hitTransform is null. Shooting action aborted.");
                    }
                }
            }
            else
            {
                input.shoot = false;
                //Animations
                playerAnimAimFalse();
                //gunHide();
                healthBar.ammoBar.SetActive(false);

                aimVirtualCamera.gameObject.SetActive(false);
                controller.SetSensitivity(normalSensitivity);
                controller.SetRotationOnMove(true);
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
            controller.animator.SetBool("isAiming", true);
        }
        public void playerAnimAimFalse()
        {
            controller.animator.SetBool("isAiming", false);
        }
        public void playerAnimShoot()
        {
            controller.animator.SetBool("isShooting", true);
            Invoke("ResetShootAnimation", 0.1f);
        }
        void ResetShootAnimation()
        {
            controller.animator.SetBool("isShooting", false);
        }
        public void playerAnimShootFalse()
        {
            controller.animator.SetBool("isShooting", false);
        }
    }
}