using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System;

namespace ThirdPerson
{
    public class ThirdPersonAim : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
        [SerializeField] private float normalSensitivity;
        [SerializeField] private float aimSensitivity;
        [SerializeField] private LayerMask aimColldierMask;
        [SerializeField] private Transform debugTransform;
        [SerializeField] private Pistol_Player playerPistol;
        [SerializeField] private HealthBar healthBar;
        private takeDamage tDamage;

        private ThirdPersonController controller;
        private PlayerControls _input;

        //Placeholder Gun
        public GameObject gun;

        //damage gun
        public float damage = 100f;

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
            }
            else
            {
                //Animations
                playerAnimAimFalse();
                gunHide();
                healthBar.ammoBar.SetActive(false);

                aimVirtualCamera.gameObject.SetActive(false);
                controller.SetSensitivity(normalSensitivity);
                controller.SetRotationOnMove(true);
            }

            if (_input.shoot)
            {
                if (hitTransform != null)
                {
                    if (playerPistol.bulletsLeft > 0)
                    {
                        playerPistol.bulletsShot = playerPistol.bulletsPerTap;
                        CheckHit(raycastHit);
                        playerPistol.Shoot(hitTransform);
                        _input.shoot = false;
                    }
                }
            }
        }

        private void CheckHit(RaycastHit raycastHit)
        {
            tDamage = raycastHit.transform.GetComponent<takeDamage>();
            if (tDamage != null)
            {
                switch (tDamage.damageType)
                {
                    case takeDamage.collisionType.head:
                        tDamage.Hit(damage);
                        break;
                    case takeDamage.collisionType.body:
                        tDamage.Hit(damage / 2);
                        break;
                    default:
                        break;
                }
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